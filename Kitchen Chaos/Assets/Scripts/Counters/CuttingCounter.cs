using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;
    [SerializeField]
    private CuttingRecipeSO[] cutKitchenObjectSOArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchen object on top
            if (player.HasKitchenObject())
            {
                //Checking if we have input recipe 
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    //If player carrying kitchen object thT can cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
        }
        else
        {
            //There is Kitchen Object on top
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player holding a plate

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO()))
        {
            // There is kitchen object on the cutting counter that can cut
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectsSO kitchenObject = OutputForInputCuttingRecipe(GetKitchenObject().GetKitchenObjectsSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(kitchenObject, this);
            }
            
        }
        else
        {

        }
    }

    private KitchenObjectsSO OutputForInputCuttingRecipe(KitchenObjectsSO kitchenObjectsSO)
    {
        CuttingRecipeSO cuttingRecipe = GetCuttingRecipeWithInput(kitchenObjectsSO);
        if(cuttingRecipe != null)
        {
            return cuttingRecipe.output;
        }
        else
        {
            return null;
        }
        
    }

    private bool HasRecipeWithInput(KitchenObjectsSO kitchenObjectsSO)
    {
        CuttingRecipeSO cuttingRecipe = GetCuttingRecipeWithInput(kitchenObjectsSO);
        return cuttingRecipe != null;
    }

    private CuttingRecipeSO GetCuttingRecipeWithInput(KitchenObjectsSO kitchenObjectsSO)
    {
        foreach (CuttingRecipeSO cuttingRecipe in cutKitchenObjectSOArray)
        {
            if (cuttingRecipe.input == kitchenObjectsSO)
            {
                return cuttingRecipe;
            }
        }
        return null;
    }
}
