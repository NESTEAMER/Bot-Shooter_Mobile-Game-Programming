using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnInterval = 10.0f;  
    public float planeSizeX = 100f;
    public float planeSizeZ = 100f;
    public float spawnHeight = 0.5f;

    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnMonster();
            spawnTimer = spawnInterval; //Reset timer
        }
    }

    void SpawnMonster()
    {
        float minX = -(planeSizeX / 2f);
        float maxX = planeSizeX / 2f;
        float minZ = -(planeSizeZ / 2f);
        float maxZ = planeSizeZ / 2f;

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Monster spawned at: " + spawnPosition);
    }
}