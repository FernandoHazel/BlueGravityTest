using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    // Just show a text for few seconds
    void Start()
    {
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }
}
