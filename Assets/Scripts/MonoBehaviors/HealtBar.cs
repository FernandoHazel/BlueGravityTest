using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealtBar : MonoBehaviour
{
    [SerializeField] protected Image greenHPBar;
    [SerializeField] protected Image redHPBar;

    private void Start() 
    {
        //Starts with all the life and no bar is displayed
        redHPBar.fillAmount = 1f;
        greenHPBar.fillAmount = 1f;

        redHPBar.enabled = false;
        greenHPBar.enabled = false;
    }

    public void On_damaged(float maxHealt, float healt)
    {
        //Display the bars
        redHPBar.enabled = true;
        greenHPBar.enabled = true;

        //make a shake effect
        greenHPBar.transform.parent.gameObject.transform.DOShakePosition(0.5f, 1, 10, 45, false, true);

        //get healt percent
        float percent = maxHealt/healt;
        
        //Adapt the size of the green bar
        greenHPBar.rectTransform.sizeDelta = new Vector2(percent, greenHPBar.rectTransform.sizeDelta.y);
    }
}
