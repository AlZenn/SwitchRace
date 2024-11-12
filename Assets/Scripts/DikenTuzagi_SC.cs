using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DikenTuzagi_SC : MonoBehaviour
{
    [SerializeField] private CarMaterialController carMaterialController;
    void Awake()
    {
        carMaterialController = GameObject.FindWithTag("Player").GetComponent<CarMaterialController>();
    }

    private void OnTriggerStay2D(Collider2D collision) // kýrmýzý araç, hareket tuzaðýndan geçer
    {
        if (carMaterialController.currentMat.name != "GreenMaterial")
        {
            //if(!collision.gameObject) { Destroy(collision.gameObject); }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (carMaterialController.currentMat.name == "GreenMaterial")
        {
            // hiçbir þey olmayacak.
        }
    }
}
