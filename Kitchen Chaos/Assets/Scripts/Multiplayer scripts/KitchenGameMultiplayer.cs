using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance {  get; private set; }

    [SerializeField]
    private KitchenObjectListSO kitchenObjectList;
    private void Awake()
    {
        Instance = this;
    }

    public  void SpawnKitchenObject(KitchenObjectsSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        int kitchenObjectSoIndex = GetKitchenObjectSOIndex(kitchenObjectSO);
        SpawnKitchenObjectServerRpc(kitchenObjectSoIndex, parent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int  kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        KitchenObjectsSO kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        NetworkObject kitchenNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenNetworkObject.Spawn(true);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }

    private int GetKitchenObjectSOIndex(KitchenObjectsSO kitchenObjectsSO)
    {
        return kitchenObjectList.kitchenObjectsList.IndexOf(kitchenObjectsSO);
    }

    private KitchenObjectsSO GetKitchenObjectSOFromIndex(int kitchenObjectSOIndex)
    {
        return kitchenObjectList.kitchenObjectsList[kitchenObjectSOIndex];
    }
}
