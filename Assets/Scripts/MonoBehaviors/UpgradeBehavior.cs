using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeBehavior : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI percent;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject buyButton;
    [SerializeField] GameObject sellButton;
    private Canvas canvas;
    private Button button;
    public string DCDialogue;
    
    private void Awake() 
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        button = GetComponent<Button>();
    }

    public void FillData
        (Sprite spriteData,
        string upgradeNameData, 
        string sellerDialogue, 
        string costTextData, 
        string percentage, 
        bool sold)
    {
        //fill all the fields
        image.sprite = spriteData;
        percent.text = percentage + "%";
        upgradeName.text = upgradeNameData;
        costText.text = costTextData + " proteins";
        DCDialogue = sellerDialogue;

        //Switch button depending if the upgrade is 
        //For sell o to buy
        if(sold == true)
        {
            sellButton.SetActive(true);
            buyButton.SetActive(false);
        }
        else
        {
            sellButton.SetActive(false);
            buyButton.SetActive(true);
        }
    }

    public void BuyThis()
    {
        //Send the upgrade name to the canvas
        canvas.Buy(upgradeName.text);
        //Debug.Log("Buying "+ upgradeName.text);
    }

    public void SellThis()
    {
        //Send the upgrade name to the canvas
        canvas.Sell(upgradeName.text);
        //Debug.Log("Selling "+ upgradeName.text);
    }

    public void IWasClicked()
    {
        //This is triggered when the update
        //object is clicked
        canvas.DisplayDialogue(DCDialogue);
    }

}
