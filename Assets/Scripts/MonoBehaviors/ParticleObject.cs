using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleObject : MonoBehaviour
{
    private ParticleSystem ps;
    private void Awake() 
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable() 
    {
        ps.Play();
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
