using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D; // SpriteShapeRenderer için gerekli

public class PlatformController_SC : MonoBehaviour
{
    [Header("Tire Objects")]
    public GameObject tireFront;
    public GameObject tireBack;
    private Collider2D tireFrontCollider;
    private Collider2D tireBackCollider;

    [Header("Scripts")]
    public TireMovement_SC tireMovementSc;
    private CarMaterialController carMaterialController;

    [Header("Platform Settings")]
    [SerializeField] private float colorChangeDelay = 0.1f;
    public Material[] PlatforMaterials;

    private void Start()
    {
        // Tekerleklerin collider'larýný alýyoruz
        tireFrontCollider = tireFront.GetComponent<Collider2D>();
        tireBackCollider = tireBack.GetComponent<Collider2D>();

        // CarMaterialController scriptine eriþim saðlýyoruz
        carMaterialController = GetComponent<CarMaterialController>();
    }

    private void Update()
    {
        CheckPlatformCollision();
    }

    private void CheckPlatformCollision()
    {
        // Ön ve arka tekerleðin temas ettiði "Ground" katmanýndaki platformlarý kontrol ediyoruz
        CheckTireCollision(tireFront.transform.position);
        CheckTireCollision(tireBack.transform.position);
    }

    private void CheckTireCollision(Vector2 position)
    {
        #region Platform Renk Uyumu Kodu

        // OverlapCircle ile "Ground" katmanýndaki en yakýn platformu buluyoruz
        Collider2D collider = Physics2D.OverlapCircle(position, 0.3f, LayerMask.GetMask("Ground")); // Bugfixlendi 0.3f ideal deðer, daha az yapmayýn.

        if (collider != null)
        {
            // Platformun SpriteShapeRenderer bileþenine eriþiyoruz
            SpriteShapeRenderer platformRenderer = collider.GetComponent<SpriteShapeRenderer>();
            if (platformRenderer != null)
            {
                // Platform materyali
                Material platformMaterial = platformRenderer.material; // Fill materiali alýyor.

                // Renk uyumunu kontrol ediyoruz
                bool isColorMatch = false;


                // Materyal isimlerini karþýlaþtýr
                if (CleanMaterialName(platformMaterial.name) == "White_Platform_Fill")
                {
                    isColorMatch = true;
                }
                else if (IsColorMatch(platformMaterial, PlatforMaterials[0], carMaterialController.redMaterial)) // IsColorMatch fonksiyonuna gönderiyor kontrol saðlýyor.
                {
                    isColorMatch = true;
                }
                else if (IsColorMatch(platformMaterial, PlatforMaterials[1], carMaterialController.greenMaterial))
                {
                    isColorMatch = true;
                }
                else if (IsColorMatch(platformMaterial, PlatforMaterials[2], carMaterialController.blueMaterial))
                {
                    isColorMatch = true;
                }
                else if (IsColorMatch(platformMaterial, PlatforMaterials[3], carMaterialController.yellowMaterial))
                {
                    isColorMatch = true;
                }

                #region Renk Kontrol
                // Eðer renk uyumluysa isTrigger'ý false yap, deðilse true yap
                if (isColorMatch)
                {
                    // Renk uyumlu ise hemen geç
                    collider.isTrigger = false;
                }
                else
                {
                    // Renk uyumsuz ise 0.5 saniye bekleyip geç
                    StartCoroutine(SetTriggerWithDelay(collider, true, colorChangeDelay));
                }
                #endregion

            }
        }
        #endregion
    }

    // Platformlar arasý geçiþte sýkýntý yaratmamasý için delay eklendi.
    private IEnumerator SetTriggerWithDelay(Collider2D collider, bool triggerValue, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.isTrigger = triggerValue; // Uyumlu deðilse trigger'ý true yap

        // bekledikten sonra isGrounded'ý false yap
        StartCoroutine(SetIsGroundedFalseWithDelay()); // Platformlar arasý rotation sorununun çözülmesi için eklendi.
    }

    private IEnumerator SetIsGroundedFalseWithDelay()
    {
        yield return new WaitForSeconds(0.2f); // min deðer 0.2f azaltýlmamalý, arttýrýlabilir duruma göre.
        tireMovementSc.isGrounded = false; // gecikmeli olarak isGrounded false yapýlýr platform geçiþlerindeki bugu önlemesi için yapýldý.
    }

    #region Platform + Araç Material Kontrolü
    private bool IsColorMatch(Material platformMaterial, Material platformReferenceMaterial, Material carMaterial)
    {
        return CleanMaterialName(platformMaterial.name) == CleanMaterialName(platformReferenceMaterial.name) && carMaterialController.currentMat == carMaterial; //mevcut platform + koda atanan material + arabanýn rengi
    }
    #endregion

    #region Instance Yazisini Kaldýr (Clean Name)

    private string CleanMaterialName(string materialName) // Instance yazýsýný yok ediyoruz.
    {
        return materialName.Replace(" (Instance)", "");
    }
    #endregion
}
