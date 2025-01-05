using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            InteractLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }

}
