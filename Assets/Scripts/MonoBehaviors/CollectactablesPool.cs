using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectactablesPool : MonoBehaviour
{
    private MainCamera cam;
    [SerializeField] GameObject YProteinPrefab; //Yellow protein prefab
    [SerializeField] GameObject BProteinPrefab; //Blue protein prefab
    [Tooltip("Blue protein spawn time")]
    [SerializeField] float BPSpawnTime;
    [Tooltip("Yellow protein spawn time")]
    [SerializeField] float YPSpawnTime;

    private void Awake() 
    {
        cam = GameObject.FindObjectOfType<MainCamera>();
    }

    private void Start() 
    {
        //Start the proteins generation
        StartCoroutine(GenerateBProtein());
        StartCoroutine(GenerateYProtein());
    }

    //Generate blue protein
    IEnumerator GenerateBProtein()
    {
        yield return new WaitForSeconds(BPSpawnTime);
        
        //Instantiate the protein in a random position
        GameObject BlueProtein = Instantiate(BProteinPrefab);
        BlueProtein.transform.SetParent(transform);
        BlueProtein.transform.position = GenerateRandomPosition();

        //This will repeat all the game
        StartCoroutine(GenerateBProtein());
    }

    //Generate yellow protein
    IEnumerator GenerateYProtein()
    {
        yield return new WaitForSeconds(YPSpawnTime);
        
        //Instantiate the protein in a random position
        GameObject YellowProtein = Instantiate(YProteinPrefab);
        YellowProtein.transform.SetParent(transform);
        YellowProtein.transform.position = GenerateRandomPosition();

        //This will repeat all the game
        StartCoroutine(GenerateYProtein());
    }

    private Vector3 GenerateRandomPosition()
    {
        //Get a random position inside the camera level limit
        //This is to avoid the object to spawn outside the level
        Vector3 spawnPosition = new Vector3
        (
            Random.Range(cam.minCamPos.x, cam.maxCamPos.x),
            Random.Range(cam.minCamPos.y, cam.maxCamPos.y),
            0
        );

        return spawnPosition;
    }
}
