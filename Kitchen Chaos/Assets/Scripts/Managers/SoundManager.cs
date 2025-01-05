using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }
    [SerializeField]
    private AudioClipRefSO audioClipRefSO;

    private float volume = 1f;
    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSucess += DelivaryManager_OnRecipeSucess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.OnAnyPlayerPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectDroppedHere += BaseCounter_OnAnyObjectDroppedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trash = sender as TrashCounter;
        PlaySound(audioClipRefSO.trash, trash.transform.position);
    }

    private void BaseCounter_OnAnyObjectDroppedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipRefSO.objectPickUp, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DelivaryManager_OnRecipeSucess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip clip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, position, volumeMultiplier * volume);
    }
    private void PlaySound(AudioClip[] clipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(clipArray[Random.Range(0, clipArray.Length)], position, volume);
    }

    public void PlayFootStepsSound(Vector3 position)
    {
        PlaySound(audioClipRefSO.footSteps, position);
    }

    public void PlayCountDownSound()
    {
        PlaySound(audioClipRefSO.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefSO.warning, position);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
