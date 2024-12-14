using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField]
    private BaseCounter baseCounter;
    [SerializeField]
    private GameObject[] visualCounterArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        if (visualCounterArray != null)
        {
            foreach (GameObject visualCounter in visualCounterArray)
            {
                visualCounter.SetActive(true);
            }
        }
    }

    private void Hide()
    {
        if (visualCounterArray != null)
        {
            foreach (GameObject visualCounter in visualCounterArray) { 
                visualCounter.SetActive(false); 
            }
        }
    }
}
