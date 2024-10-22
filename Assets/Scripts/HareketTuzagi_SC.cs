using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HareketTuzagi_SC : MonoBehaviour
{
    [Header("Hareket Tuzagi Properties")]
    [SerializeField] private Transform target1; // Birinci hedef
    [SerializeField] private Transform target2; // Ýkinci hedef
    [SerializeField] private float speed = 5f;  // Hareket hýzý 

    [SerializeField] private CarMaterialController carMaterialController;

    private Transform currentTarget;     // Þu anki hedef
    private bool reachedTarget1 = false; // Ýlk hedefe ulaþýp ulaþmadýðýný kontrol eder
    

    void Start()
    {
        currentTarget = target1; // Baþlangýçta ilk hedefi ayarla
        carMaterialController = GameObject.FindWithTag("Player").GetComponent<CarMaterialController>();
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        // GameObject'i currentTarget'e doðru hareket ettir
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Hedefe ulaþýp ulaþmadýðýný kontrol et
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (!reachedTarget1)
            {
                // Ýlk hedefe ulaþýldýysa, ikinci hedefe geç
                currentTarget = target2;
                reachedTarget1 = true;
            }
            else
            {
                // Ýkinci hedefe ulaþýldýysa, ilk hedefe geri dön
                currentTarget = target1;
                reachedTarget1 = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // kýrmýzý araç, hareket tuzaðýndan geçer
    {
        if (carMaterialController.currentMat.name != "RedMaterial")
        {
            Destroy(collision.gameObject);
            reloadScene(); // animasyon koyulacaksa bekleme kodu yazýlabilir.
        }
        else if (carMaterialController.currentMat.name == "RedMaterial")
        {
            // hiçbir þey olmayacak.
        }
    }

    private void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
