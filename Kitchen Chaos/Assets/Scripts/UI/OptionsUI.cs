using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [Header("Buttons")]
    [SerializeField]
    private Button soundEffectsButton;
    [SerializeField]
    private Button musicButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button moveUpButton;
    [SerializeField]
    private Button moveDownButton;
    [SerializeField]
    private Button moveLeftButton;
    [SerializeField]
    private Button moveRightButton;
    [SerializeField]
    private Button interactButton;
    [SerializeField]
    private Button interactAlternateButton;
    [SerializeField]
    private Button pauseButton;

    [Header("Button Text")]
    [SerializeField]
    private TextMeshProUGUI soundEffectsText;
    [SerializeField]
    private TextMeshProUGUI musicText;
    [SerializeField]
    private TextMeshProUGUI moveUpText;
    [SerializeField]
    private TextMeshProUGUI moveDownText;
    [SerializeField]
    private TextMeshProUGUI moveLeftText;
    [SerializeField]
    private TextMeshProUGUI moveRightText;
    [SerializeField]
    private TextMeshProUGUI interactText;
    [SerializeField]
    private TextMeshProUGUI interactAlternateText;
    [SerializeField]
    private TextMeshProUGUI pauseText;

    [Header("Press any key reference")]
    [SerializeField]
    private Transform pressToRebindKeyTransform;

    private void Awake()
    {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Up);
        });

        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Down);
        });

        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Left);
        });

        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Right);
        });

        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });

        interactAlternateButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact_Alternate);
        });

        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        UpdateVisual();
        HidePressToRebind();
        Hide();
    }

    private void GameManager_OnGameResumed(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects : " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebind()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebind()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebind();
        GameInput.Instance.ReBinding(binding, () =>
        {
            HidePressToRebind();
            UpdateVisual();
        });
    }
}
