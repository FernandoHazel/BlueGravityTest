using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Canvas : MonoBehaviour
{
    [SerializeField] GameObject shopButton;
    [SerializeField] GameObject ShopPanel;
    [SerializeField] GameObject upgradePrefab;
    [SerializeField] GameObject upgradesPanel;
    [SerializeField] GameObject EquippedPanel;
    [SerializeField] Upgrades_SO upgrades_SO;
    [SerializeField] MainCharacter_SO mainCharacter_SO;
    [SerializeField] TextMeshProUGUI canvasProteins;
    [SerializeField] TextMeshProUGUI shopProteins;
    private List<GameObject> prefabList = new List<GameObject>();
    public delegate void ActionReject();
    public static event ActionReject rejected;
    
    private void Start() 
    {
        canvasProteins.text = mainCharacter_SO.proteins.ToString() + " Proteins";
        shopProteins.text = mainCharacter_SO.proteins.ToString() + " Proteins";
    }

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
        ShopPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GenerateUpgrades()
    {
        //first we destroy the previous prefabs and 
        //clear the previous list
        foreach (GameObject prefab in prefabList)
        {
            Destroy(prefab);
        }
        prefabList.Clear();

        //Instance and update the updates in the Shelving
        foreach (Upgrade upgrade in upgrades_SO.upgrades)
        {
            if(upgrade.sold == false)
            {
                //If the upgrade is not sold we put it in the buy section
                //Create the upgrade on the shelving
                GameObject prefab = Instantiate(upgradePrefab);
                prefab.transform.SetParent(upgradesPanel.transform);
                prefabList.Add(prefab);

                //Send the data to the upgrade prefab
                //To fill the fields
                prefab.GetComponent<UpgradeBehavior>().FillData
                (
                    upgrade.sprite,
                    upgrade.UpgradeName,
                    upgrade.BuyCost.ToString(),
                    upgrade.Upercent.ToString(),
                    upgrade.sold
                );
            }
            else
            {
                //If the upgrade is sold we put it in the sell section
                GameObject prefab = Instantiate(upgradePrefab);
                prefab.transform.SetParent(EquippedPanel.transform);
                prefabList.Add(prefab);

                //Send the data to the upgrade prefab
                //To fill the fields
                prefab.GetComponent<UpgradeBehavior>().FillData
                (
                    upgrade.sprite,
                    upgrade.UpgradeName,
                    upgrade.SellCost.ToString(),
                    upgrade.Upercent.ToString(),
                    upgrade.sold
                );
            }
            
        }

        //Update the currency in the UI
        canvasProteins.text = mainCharacter_SO.proteins.ToString() + " Proteins";
        shopProteins.text = mainCharacter_SO.proteins.ToString() + " Proteins";
    }

    public void Buy(string buyUpdateName)
    {
        //Look for the update in the database
        foreach (Upgrade upgrade in upgrades_SO.upgrades)
        {
            if(upgrade.UpgradeName == buyUpdateName)
            {
                //When we found it check if the player has enough proteins to buy
                if(mainCharacter_SO.proteins >= upgrade.BuyCost)
                {
                    //We charge the player and mark the upgrade as sold
                    mainCharacter_SO.proteins -= upgrade.BuyCost;
                    upgrade.sold = true;

                    //Regenerate Updates for UI and the rest of the game
                    GenerateUpgrades();
                }
                else
                {
                    //Reject the player to buy
                    rejected();
                }
            }
        }
    }
    public void Sell(string sellUpdateName)
    {
        //Look for the update in the database
        foreach (Upgrade upgrade in upgrades_SO.upgrades)
        {
            if(upgrade.UpgradeName == sellUpdateName)
            {
                //Pay the player and mark the update as not sold
                mainCharacter_SO.proteins += upgrade.SellCost;
                upgrade.sold = false;

                //Regenerate Updates for UI and the rest of the game
                GenerateUpgrades();
            }
        }
    }

    public void TakeProteins(int amount)
    {
        mainCharacter_SO.proteins += amount;
        canvasProteins.text = mainCharacter_SO.proteins.ToString() + " Proteins";
        shopProteins.text = mainCharacter_SO.proteins.ToString() + " Proteins";
    }
}
