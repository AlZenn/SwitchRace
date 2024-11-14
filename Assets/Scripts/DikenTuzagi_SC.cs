using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DikenTuzagi_SC : MonoBehaviour
{
    [SerializeField] private CarMaterialController carMaterialController;
    private DeathAnimation deathAnimation;
    private bool hasTriggeredDeath = false;
    void Awake()
    {
        carMaterialController = GameObject.FindWithTag("Player").GetComponent<CarMaterialController>();
        deathAnimation = GameObject.Find("DeathManager").GetComponent<DeathAnimation>();

    }

    private void OnTriggerStay2D(Collider2D collision) // k�rm�z� ara�, hareket tuza��ndan ge�er
    {
        if (carMaterialController.currentMat.name != "GreenMaterial" && !hasTriggeredDeath)
        {
            deathAnimation.TriggerDeathAnimation(); // Trigger death animation
            hasTriggeredDeath = true; // öldü olarak kabul et
        }
        else if (carMaterialController.currentMat.name == "GreenMaterial")
        {
            // hi�bir �ey olmayacak.
        }
    }
}
