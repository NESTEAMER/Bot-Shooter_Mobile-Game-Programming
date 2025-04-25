using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHandler : MonoBehaviour
{
    public float moveSpeed = 3.0f;    
    private Transform playerTransform; 
    private const string PLAYER_TAG = "Player";

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(PLAYER_TAG);

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            Debug.Log("Player found by monster.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayerSimple();
    }

    void MoveTowardsPlayerSimple()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0;

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}