using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KutuTuzagi_SC : MonoBehaviour
{
    [SerializeField] private CarMaterialController carMaterialController;
    void Start()
    {
        carMaterialController = GameObject.FindWithTag("Player").GetComponent<CarMaterialController>();
    }

    private void OnTriggerStay2D(Collider2D collision) // kýrmýzý araç, hareket tuzaðýndan geçer
    {
        if (carMaterialController.currentMat.name != "BlueMaterial")
        {
            Destroy(collision.gameObject);
            reloadScene(); // animasyon koyulacaksa bekleme kodu yazýlabilir.
        }
        else if (carMaterialController.currentMat.name == "BlueMaterial")
        {
            // hiçbir þey olmayacak.
        }
    }
    private void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
