using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "PopUp";

    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TextMeshProUGUI messageText;
    [SerializeField]
    private Color sucessColor;
    [SerializeField]
    private Color failureColor;
    [SerializeField]
    private Sprite sucessSprite;
    [SerializeField]
    private Sprite failureSprite;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSucess += DeliveryManager_OnRecipeSucess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = failureColor;
        iconImage.sprite = failureSprite;
        messageText.text = "DELIVERY\nFAILED";
    }

    private void DeliveryManager_OnRecipeSucess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color=sucessColor;
        iconImage.sprite = sucessSprite;
        messageText.text = "DELIVERY\nSUCCESS";
    }
}
