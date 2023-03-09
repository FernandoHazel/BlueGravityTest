using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] GameObject pauseButton;
    private void OnEnable() 
    {
        pauseButton.SetActive(false);
    }
    private void OnDisable() {
        pauseButton.SetActive(true);
    }
}
