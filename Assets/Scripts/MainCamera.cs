using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject tracker;
    [SerializeField] Vector2 minCamPos, maxCamPos;
    [SerializeField] float smoothTime;  //this is the time of delay of the camera movement
    private Vector2 velocity;
    private void Start() 
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //We save the position of the tracker in new variables
        //The "smoothDamp" function creates a transition delay between to points and needs a reference of velocity and time
        float posX = Mathf.SmoothDamp(transform.position.x, tracker.transform.position.x, ref velocity.x, smoothTime);
        float posY = Mathf.SmoothDamp(transform.position.y, tracker.transform.position.y, ref velocity.y, smoothTime);

        //We move the camera to that position
        //We use the "Clampt" function to limit the camera movement between a minimum and a maximum position in X and Y axis
        transform.position = new Vector3 (
            Mathf.Clamp(posX, minCamPos.x, maxCamPos.x),
            Mathf.Clamp(posY, minCamPos.y, maxCamPos.y),
            transform.position.z);
    }
}