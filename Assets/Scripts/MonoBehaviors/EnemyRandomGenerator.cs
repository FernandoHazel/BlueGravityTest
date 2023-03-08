using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomGenerator : MonoBehaviour
{
    //Every certain amount of time I generate a random
    //amount of enemies within a given range from a random 
    //generator that I have on the map, I can change 
    //the amount of generators and its positions on the level
    [SerializeField] int generationSeconds = 10;
    [SerializeField] int MaxAmount = 500;
    [SerializeField] GameObject enemyPool;
    [SerializeField] GameObject enemyPrefab;
    [Tooltip("How many enemies whin the time frame will be spawned")]
    [SerializeField] Vector2 enemySpawnRange = new Vector2(10,30);
    private List<Transform> generatorsList = new List<Transform>();
    private List<GameObject> enemyList = new List<GameObject>();
    private GameObject enemy;

    private void Start() 
    {
        //First we instantiate all the enemies for the pool
        for (int i = 1; i<=MaxAmount; i++)
        {
            enemy = Instantiate(enemyPrefab);
            enemy.transform.SetParent(enemyPool.transform);
            enemy.SetActive(false);
            enemyList.Add(enemy);
        }

        //Every child of this object are taken as generators
        for (int i = 0; i < transform.childCount; i++)
        {
            generatorsList.Add(transform.GetChild(i));
        }
        StartCoroutine(WaitToGenerate());
    }
    private void GenerateEnemies()
    {
        //Get the amount of enemies to generate
        float enemyCount = Random.Range(enemySpawnRange.x, enemySpawnRange.y);

        //Take the generator randomly
        int generatorIndex = Random.Range(0, generatorsList.Count-1);
        Transform generator = generatorsList[generatorIndex];

        //Generate the enemies on the generator position
        int generatedCount = 0;
        foreach (GameObject enemy in enemyList)
        {
            if(enemy.activeSelf == false && generatedCount <= enemyCount)
            {
                enemy.SetActive(true);
                enemy.transform.position = generator.transform.position;
                generatedCount++;
            }
        }
    }

    IEnumerator WaitToGenerate()
    {
        yield return new WaitForSeconds(generationSeconds);
        GenerateEnemies();
        StartCoroutine(WaitToGenerate());
    }
}
