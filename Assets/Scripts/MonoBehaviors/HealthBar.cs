using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected Image greenHPBar;
    [SerializeField] protected Image redHPBar;

    private void Start() 
    {
        //Starts with all the life and no bar is displayed
        redHPBar.fillAmount = 1f;
        greenHPBar.fillAmount = 1f;
    }

    public void On_damaged(float percent)
    {
        //make a shake effect
        /* greenHPBar.transform.parent.gameObject.transform.
        DOShakeScale(0.5f, 1, 1, 10, true); */

        
        //Adapt the size of the green bar
        greenHPBar.rectTransform.sizeDelta = new Vector2
        (
            redHPBar.rectTransform.sizeDelta.x * percent,
            greenHPBar.rectTransform.sizeDelta.y
        );
    }
}
