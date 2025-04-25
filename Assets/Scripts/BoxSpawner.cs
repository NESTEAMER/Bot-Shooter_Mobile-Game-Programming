using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;
    public float spawnInterval = 5.0f;

    //SpawnArea
    public float spawnAreaMinX = -50f;
    public float spawnAreaMaxX = 50f;
    public float spawnAreaMinZ = -50f;
    public float spawnAreaMaxZ = 50f;

    public float spawnHeight = 20.0f;

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
            SpawnBox();
            spawnTimer = spawnInterval;//Reset timer
        }
    }

    void SpawnBox()
    {
        float randomX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float randomZ = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);

        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        GameObject spawnedBox = Instantiate(boxPrefab, spawnPosition, randomRotation);

        Debug.Log("Box spawned at: " + spawnPosition + " with Y rotation: " + randomRotation.eulerAngles.y);
    }
}