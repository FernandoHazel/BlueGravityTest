using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    [SerializeField] GameObject shopButton;
    [SerializeField] GameObject ShopPanel;

    //The dendritic cells calls these methods
    public void DisplayShopButton()
    {
        shopButton.SetActive(true);
    }
    public void DisableShopButton()
    {
        shopButton.SetActive(false);
    }
    public void EnterTheShop()
    {
        ShopPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ExitTheShop() 
    {
        ShopPanel.SetActive(true);
        Time.timeScale = 1;
    }
}
