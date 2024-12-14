using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
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
    private float spawnRecipeTimer;
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
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if( GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipe = recipes.recipes[UnityEngine.Random.Range(0, recipes.recipes.Count)];
                Debug.Log(waitingRecipe.name);
                waitingRecipeSOList.Add(waitingRecipe);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
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
                    sucessfulRecipesAmount++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
                    OnRecipeSucess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }

        }
        // No matches found
        //Player didn't deliver correct recipe
        Debug.Log("Player did not deliver correct recipe");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

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
