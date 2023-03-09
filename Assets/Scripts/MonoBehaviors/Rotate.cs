using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    
    [SerializeField] float rotationSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rotate in z axis
        Vector3 direction = new Vector3(0,0,1);
        transform.Rotate(direction * rotationSpeed* Time.deltaTime);
    }
}
