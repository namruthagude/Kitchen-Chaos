using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI recipeName;
    [SerializeField]
    private Transform iconContainer;
    [SerializeField]
    private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecipeSO recipe)
    {
        recipeName.text = recipe.name;

        foreach (Transform child in iconContainer)
        {
            if(child == iconContainer)
            {
                continue;
            }
            Destroy(child.gameObject);

        }

        foreach(KitchenObjectsSO kitchenObjectsSO in recipe.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            Image image = iconTransform.GetComponent<Image>();
            image.enabled = true;
            image.sprite = kitchenObjectsSO.sprite;
        }
    }
}
