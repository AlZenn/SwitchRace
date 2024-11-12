using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KutuTuzagi_SC : MonoBehaviour
{
    [SerializeField] private CarMaterialController carMaterialController;
    AudioManager audioManager;
    void Awake()
    {
        carMaterialController = GameObject.FindWithTag("Player").GetComponent<CarMaterialController>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerStay2D(Collider2D collision) // mavi ara�, hareket tuza��ndan ge�er
    {
        if (carMaterialController.currentMat.name != "BlueMaterial")
        {
            Destroy(collision.gameObject);
            reloadScene(); // animasyon koyulacaksa bekleme kodu yaz�labilir.
            audioManager.PlaySFX(audioManager.deathSFX);
        }
        else if (carMaterialController.currentMat.name == "BlueMaterial")
        {
            // hi�bir �ey olmayacak.
        }
    }
    private void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
