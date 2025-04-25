using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class PlayerHandler : MonoBehaviour
{
    private Animator animator;
    private float moveSpeed = 5.0f;
    private float rotationSpeed = 5.0f;
    private float orbitXSpeed = 100.0f;
    private float orbitYSpeed = 10.0f;
    private Vector3 destination;
    private Vector2 delta;

    private Quaternion oldRotation;
    private bool isMove = false;

    private CinemachineFreeLook cameraFreeLook;

    //shooting properties
    public float shootingRange = 50f;
    public Transform gunBarrel;

    //scoring properties
    public TextMeshProUGUI boxScoreText;
    public TextMeshProUGUI enemyScoreText;

    private int currentBoxScore = 0;
    private int currentEnemyScore = 0;
    public int scorePerBox = 10;
    public int scorePerEnemy = 25;


    public void OnMove(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<Vector2>().ToString());
        destination = ctx.ReadValue<Vector2>();

        if(isMove == false)
        {
            isMove = true;
            oldRotation = transform.rotation;
        }
    }

    public void OnLookAround(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValue<Vector2>().ToString());
        delta = ctx.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Shoot");
            animator.SetTrigger("shoot");

            Vector3 rayOrigin;
            rayOrigin = gunBarrel.position; 
            Vector3 rayDirection = transform.forward;
            Debug.DrawRay(rayOrigin, rayDirection * shootingRange, Color.red, 1.0f);

            RaycastHit hitInfo; 
            if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, shootingRange))
            {
                Debug.Log("Raycast mengenai: " + hitInfo.transform.name + " pada jarak: " + hitInfo.distance);

                if (hitInfo.transform.CompareTag("Monster"))
                {
                    Debug.Log("Target: " + hitInfo.transform.name);
                    Destroy(hitInfo.transform.gameObject);

                    AddScore(true);
                }
            }
            else
            {
                Debug.Log("Tembakan meleset!");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        var gameObj = GameObject.FindWithTag("MainCamera");
        cameraFreeLook = gameObj.GetComponent<CinemachineFreeLook>();
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(destination.magnitude > 0.0f)
        {
            Vector3 lookDirection = new Vector3(destination.x, 0.0f, destination.y);
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(lookRotation * oldRotation, transform.rotation, rotationSpeed * Time.deltaTime);

            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetFloat("Motion", destination.magnitude);
        }
        else
        {
            float temp = animator.GetFloat("Motion");
            isMove = false;
            animator.SetFloat("Motion", temp - (2.0f * Time.deltaTime));
        }

        cameraFreeLook.m_XAxis.Value += delta.x * orbitXSpeed * Time.deltaTime;
        cameraFreeLook.m_YAxis.Value += delta.y * orbitYSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GrabbableBox"))
        {
            Debug.Log("Box diambil: " + other.gameObject.name);
            Destroy(other.gameObject);
            AddScore(false); 
        }
    }

    void AddScore(bool isEnemyScore)
    {
        if (isEnemyScore)
        {
            currentEnemyScore += scorePerEnemy;
            Debug.Log("Enemy Score: " + currentEnemyScore);
        }
        else
        {
            currentBoxScore += scorePerBox;
            Debug.Log("Box Score: " + currentBoxScore);
        }
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
         if (boxScoreText != null) {
             boxScoreText.text = "Box: " + currentBoxScore;
         }
          if (enemyScoreText != null) {
             enemyScoreText.text = "Enemy: " + currentEnemyScore;
         }
    }
}
