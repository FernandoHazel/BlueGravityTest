using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DendriticCell : MonoBehaviour
{
    private Canvas canvas;
    private void Awake() 
    {
        //This cell will call the methods to activate the shop button
        //On the canvas
        canvas = GameObject.FindObjectOfType<Canvas>();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            canvas.DisplayShopButton();
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            canvas.DisableShopButton();
        }
    }
}
