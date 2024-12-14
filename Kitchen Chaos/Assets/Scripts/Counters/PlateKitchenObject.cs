using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO KitchenObjectsSO;
    }

    [SerializeField]
    private List<KitchenObjectsSO> validKitchenObjects;

    private List<KitchenObjectsSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectsSO>();
    }

    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectSO)
    {
        if (!validKitchenObjects.Contains(kitchenObjectSO))
        {
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs(){
                KitchenObjectsSO = kitchenObjectSO
            });
            return true;
        }
    }
     
    public List<KitchenObjectsSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
