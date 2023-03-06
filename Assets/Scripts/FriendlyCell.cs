using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyCell : MonoBehaviour
{
    public static List<GameObject> friendlyCells = new List<GameObject>();
    private void Awake() 
    {
        //Add the friendly cell to a static array
        friendlyCells.Add(gameObject);
    }

}
