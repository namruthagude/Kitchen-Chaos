using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeDelivered;
    public event EventHandler OnRecipeSucess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance {
        get;
        private set;
    }
    [SerializeField]
    private RecipeListSO recipes;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int sucessfulRecipesAmount = 0;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if( GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
            {
                int waitingRecipeIndex = UnityEngine.Random.Range(0, recipes.recipes.Count);
                SpawnWaitingRecipeSOClientRpc(waitingRecipeIndex);
            }
        }
    }

    [ClientRpc]
    public void SpawnWaitingRecipeSOClientRpc(int waitingRecipeIndex)
    {
        RecipeSO waitingRecipe = recipes.recipes[waitingRecipeIndex];
        waitingRecipeSOList.Add(waitingRecipe);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipe = waitingRecipeSOList[i];
            if(waitingRecipe.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has same number of ingredients

                bool plateContentMatchesRecipe = true;
                foreach(KitchenObjectsSO recipeKitchenObjectSo in waitingRecipe.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    // Cycling through all the recipes
                    foreach (KitchenObjectsSO plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all the plate
                        if(recipeKitchenObjectSo == plateKitchenObjectSo)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // If ingredient is not present
                        plateContentMatchesRecipe = false;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    // All ingrdients matches
                    Debug.Log("Player delivered correct recipe");
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }

        }
        // No matches found
        //Player didn't deliver correct recipe
        Debug.Log("Player did not deliver correct recipe");
        DeliverIncorrectRecipeServerRpc();

    }

    [ServerRpc(RequireOwnership =false)]
    public void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }

    [ClientRpc]
    public void DeliverIncorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    } 

    [ServerRpc(RequireOwnership = false)]
    public void DeliverCorrectRecipeServerRpc(int waitingRecipeSoListIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSoListIndex);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSOListIndex)
    {
        sucessfulRecipesAmount++;
        waitingRecipeSOList.RemoveAt(waitingRecipeSOListIndex);
        OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
        OnRecipeSucess?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSucessfulRecipesAmount()
    {
        return sucessfulRecipesAmount;
    }
}
