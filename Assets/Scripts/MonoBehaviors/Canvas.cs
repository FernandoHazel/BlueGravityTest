using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    [SerializeField] GameObject shopButton;
    [SerializeField] GameObject ShopPanel;
    [SerializeField] GameObject upgradePrefab;
    [SerializeField] GameObject upgradesPanel;
    [SerializeField] Upgrades_SO upgrades_SO;

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
        GenerateUpgrades();
    }

    public void ExitTheShop() 
    {
        ShopPanel.SetActive(true);
        Time.timeScale = 1;
    }

    public void GenerateUpgrades()
    {
        //Instance and update the updates in the Shelving
        foreach (Upgrade upgrade in upgrades_SO.upgrades)
        {
            //Create the upgrade on the shelving
            GameObject prefab = Instantiate(upgradePrefab);
            prefab.transform.SetParent(upgradesPanel.transform);

            //Send the data to the upgrade prefab
            //To fill the fields
            prefab.GetComponent<UpgradeBehavior>().FillData
            (
                upgrade.sprite,
                upgrade.UpgradeName,
                upgrade.cost.ToString(),
                upgrade.Upercent.ToString(),
                upgrade.sold
            );
        }
    }
}
