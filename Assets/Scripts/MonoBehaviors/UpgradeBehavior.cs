using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeBehavior : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI upgradeText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Button buyButton;

    public void FillData(Sprite spriteData, string upgradeTextData, string costTextData, string percentage, bool sold)
    {
        //fill all the fields
        image.sprite = spriteData;
        upgradeText.text = $"{percentage}% {upgradeTextData}";
        costText.text = costTextData;

        //If the upgrade is already sold
        //The buy button must be uninteractable
        buyButton.interactable = !sold;
    }

}
