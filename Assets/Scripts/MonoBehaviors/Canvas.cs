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
    [SerializeField] TextMeshProUGUI friendlyCells;
    [SerializeField] TextMeshProUGUI activeEnemies;
    [SerializeField] TextMeshProUGUI shopProteins;
    [SerializeField] TextMeshProUGUI DialogueText;
    [SerializeField] GameObject player;
    private List<GameObject> prefabList = new List<GameObject>();
    private List<GameObject> accessoriesList = new List<GameObject>();

    private void Awake() 
    {
        if(player == null)
        player = GameObject.Find("Player");

        //Start the game with no upgrades
        foreach (Upgrade upgrade in upgrades_SO.upgrades)
        {
            upgrade.sold = false;
        }
    }

    private void OnEnable() 
    {
        Bacteria.bacteriaBorn += UpdateActiveEnemies;
        Bacteria.bacteriaDied += UpdateActiveEnemies;
        FriendlyCell.FriendlyCellDied += UpdateActiveFriendlyCells;
    }
    private void OnDisable() 
    {
        Bacteria.bacteriaBorn -= UpdateActiveEnemies;
        Bacteria.bacteriaDied -= UpdateActiveEnemies;
        FriendlyCell.FriendlyCellDied -= UpdateActiveFriendlyCells;
    }
    
    private void Start() 
    {
        UpdateActiveEnemies();
        UpdateActiveFriendlyCells();
        canvasProteins.text = mainCharacter_SO.inGameProteins.ToString() + " Proteins";
        shopProteins.text = mainCharacter_SO.inGameProteins.ToString() + " Proteins";
        GenerateUpgrades();
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

    public void UpdateActiveEnemies()
    {
        activeEnemies.text = GameManager.activeEnemies + " active enemies";
    }
    private void UpdateActiveFriendlyCells()
    {
        friendlyCells.text = GameManager.friendlyCells + " friendly cells";
    }

    public void GenerateUpgrades()
    {
        //First erase all the previous changes and to start again
        foreach (GameObject item in accessoriesList)
        {
            Destroy(item);
        }
        accessoriesList.Clear();

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
                    upgrade.DCDialogue,
                    upgrade.BuyCost.ToString(),
                    upgrade.Upercent.ToString(),
                    upgrade.sold
                );

                Unequip(upgrade);
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
                    upgrade.DCDialogue,
                    upgrade.SellCost.ToString(),
                    upgrade.Upercent.ToString(),
                    upgrade.sold
                );

                //Equip upgrades
                Equip(upgrade);
            }
            
        }

        //Update the currency in the UI
        canvasProteins.text = mainCharacter_SO.inGameProteins.ToString() + " Proteins";
        shopProteins.text = mainCharacter_SO.inGameProteins.ToString() + " Proteins";
    }

    public void DisplayDialogue(string dialogue)
    {
        //The store keeper will display its dialogue here
        DialogueText.text = dialogue;
    }

    public void Reject(string refuseDialogue)
    {
        //The store keeper will display its dialogue here
        DialogueText.text = refuseDialogue;
    }

    public void Buy(string buyUpdateName)
    {
        //Look for the update in the database
        foreach (Upgrade upgrade in upgrades_SO.upgrades)
        {
            if(upgrade.UpgradeName == buyUpdateName)
            {
                //When we found it check if the player has enough proteins to buy
                if(mainCharacter_SO.inGameProteins >= upgrade.BuyCost)
                {
                    //We charge the player and mark the upgrade as sold
                    mainCharacter_SO.inGameProteins -= upgrade.BuyCost;
                    upgrade.sold = true;

                    //Regenerate Updates for UI and the rest of the game
                    GenerateUpgrades();
                }
                else
                {
                    //Reject the player payment
                    Reject(upgrade.refuseDialogue);
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
                mainCharacter_SO.inGameProteins += upgrade.SellCost;
                upgrade.sold = false;

                //Regenerate Updates for UI and the rest of the game
                GenerateUpgrades();
            }
        }
    }

    //This is called when we collect proteins
    public void TakeProteins(int amount)
    {
        mainCharacter_SO.inGameProteins += amount;
        canvasProteins.text = mainCharacter_SO.inGameProteins.ToString() + " Proteins";
        shopProteins.text = mainCharacter_SO.inGameProteins.ToString() + " Proteins";
    }

    public void Equip(Upgrade upgrade)
    {
        //Equip the accessory
        GameObject accessory = Instantiate(upgrade.accessory);
        accessory.transform.SetParent(player.transform);
        accessory.transform.localPosition = new Vector3(0,0,0);
        accessoriesList.Add(accessory);

        //Modify the stats
        switch (upgrade.skill) {
            case Upgrade.skills.maxHealt:
                
                if(upgrade.modified == false)
                mainCharacter_SO.modifiedMaxHealth += 
                (mainCharacter_SO.maxHealth * upgrade.Upercent/100);
                break;
            case Upgrade.skills.force:
                
                if(upgrade.modified == false)
                mainCharacter_SO.modifiedForce += 
                (mainCharacter_SO.force * upgrade.Upercent/100);
                break;
            case Upgrade.skills.damage:
                
                if(upgrade.modified == false)
                mainCharacter_SO.modifiedDamage += 
                (mainCharacter_SO.damage * upgrade.Upercent/100);
                break;
            case Upgrade.skills.attackRange:
                
                if(upgrade.modified == false)
                mainCharacter_SO.modifiedAttackRange += 
                (mainCharacter_SO.attackRange * upgrade.Upercent/100);
                break;
            
        }

        upgrade.modified = true;
    }

    public void Unequip(Upgrade upgrade)
    {
        //The accessory is already destroyed 
        //when GenerateUpdates() runs
        
        //Modify the stats
        //because this code runs at the begginig of the game 
        //Verify if the stats are not already at default
        switch (upgrade.skill) {
            case Upgrade.skills.maxHealt:

                if(upgrade.modified == true)
                mainCharacter_SO.modifiedMaxHealth -= 
                (mainCharacter_SO.maxHealth * upgrade.Upercent/100);
                break;
            case Upgrade.skills.force:
                
                if(upgrade.modified == true)
                mainCharacter_SO.modifiedForce -= 
                (mainCharacter_SO.force * upgrade.Upercent/100);
                break;
            case Upgrade.skills.damage:
                
                if(upgrade.modified == true)
                mainCharacter_SO.modifiedDamage -= 
                (mainCharacter_SO.damage * upgrade.Upercent/100);
                break;
            case Upgrade.skills.attackRange:
                
                if(upgrade.modified == true)
                mainCharacter_SO.modifiedAttackRange -= 
                (mainCharacter_SO.attackRange * upgrade.Upercent/100);
                break;
        }

        upgrade.modified = false;
    }
}
