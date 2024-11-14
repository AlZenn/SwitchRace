using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HareketTuzagi_SC : MonoBehaviour
{
    [Header("Hareket Tuzagi Properties")]
    [SerializeField] private Transform target1; // Birinci hedef
    [SerializeField] private Transform target2; // �kinci hedef
    [SerializeField] private float speed = 5f;  // Hareket h�z� 
    private DeathAnimation deathAnimation;
    private bool hasTriggeredDeath = false;

    [SerializeField] private CarMaterialController carMaterialController;

    private Transform currentTarget;     // �u anki hedef
    private bool reachedTarget1 = false; // �lk hedefe ula��p ula�mad���n� kontrol eder
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        deathAnimation = GameObject.Find("DeathManager").GetComponent<DeathAnimation>();
    }

    void Start()
    {
        currentTarget = target1; // Ba�lang��ta ilk hedefi ayarla
        carMaterialController = GameObject.FindWithTag("Player").GetComponent<CarMaterialController>();
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        // GameObject'i currentTarget'e do�ru hareket ettir
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Hedefe ula��p ula�mad���n� kontrol et
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (!reachedTarget1)
            {
                // �lk hedefe ula��ld�ysa, ikinci hedefe ge�
                currentTarget = target2;
                reachedTarget1 = true;
            }
            else
            {
                // �kinci hedefe ula��ld�ysa, ilk hedefe geri d�n
                currentTarget = target1;
                reachedTarget1 = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // k�rm�z� ara�, hareket tuza��ndan ge�er
    {
        if (carMaterialController.currentMat.name != "RedMaterial" && !hasTriggeredDeath)
        {
            deathAnimation.TriggerDeathAnimation(); // Trigger death animation
            hasTriggeredDeath = true; // öldü olarak kabul et
        }
        else if (carMaterialController.currentMat.name == "RedMaterial")
        {
            // hi�bir �ey olmayacak.
        }
    }

    private void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
