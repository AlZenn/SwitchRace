using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KutuTuzagi_SC : MonoBehaviour
{
    [SerializeField] private CarMaterialController carMaterialController;
    AudioManager audioManager;
    private DeathAnimation deathAnimation;
    private bool hasTriggeredDeath = false; // Bayrak ekledik

    void Awake()
    {
        carMaterialController = GameObject.FindWithTag("Player").GetComponent<CarMaterialController>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        deathAnimation = GameObject.Find("DeathManager").GetComponent<DeathAnimation>();
    }

    private void OnTriggerStay2D(Collider2D collision) // mavi araç, hareket tuzağından geçer
    {
        if (carMaterialController.currentMat.name != "BlueMaterial" && !hasTriggeredDeath) // Bayrağa da bakıyoruz
        {
            deathAnimation.TriggerDeathAnimation(); // Trigger death animation
            hasTriggeredDeath = true; // Bayrağı true yapıyoruz, böylece tekrar tetiklenmez
        }
        else if (carMaterialController.currentMat.name == "BlueMaterial")
        {
            // Hiçbir şey olmayacak.
        }
    }

    private void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}