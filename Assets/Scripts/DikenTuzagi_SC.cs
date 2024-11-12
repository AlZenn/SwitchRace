using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DikenTuzagi_SC : MonoBehaviour
{
    [SerializeField] private CarMaterialController carMaterialController;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        carMaterialController = GameObject.FindWithTag("Player").GetComponent<CarMaterialController>();
    }

    private void OnTriggerStay2D(Collider2D collision) // kýrmýzý araç, hareket tuzaðýndan geçer
    {
        if (carMaterialController.currentMat.name != "GreenMaterial")
        {
            Destroy(collision.gameObject);
            reloadScene(); // animasyon koyulacaksa bekleme kodu yazýlabilir.
            audioManager.PlaySFX(audioManager.deathSFX);
        }
        else if (carMaterialController.currentMat.name == "GreenMaterial")
        {
            // hiçbir þey olmayacak.
        }
    }
    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
