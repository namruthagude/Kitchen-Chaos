using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField]
    private KitchenObjectsSO kitchenObjectsSO;

   
    public  override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player not carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObjectsSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty); 
        }
    }

}
