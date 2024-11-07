using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D; // SpriteShapeRenderer i�in gerekli

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
        // Tekerleklerin collider'lar�n� al�yoruz
        tireFrontCollider = tireFront.GetComponent<Collider2D>();
        tireBackCollider = tireBack.GetComponent<Collider2D>();

        // CarMaterialController scriptine eri�im sa�l�yoruz
        carMaterialController = GetComponent<CarMaterialController>();
    }

    private void Update()
    {
        CheckPlatformCollision();
    }

    private void CheckPlatformCollision()
    {
        // �n ve arka tekerle�in temas etti�i "Ground" katman�ndaki platformlar� kontrol ediyoruz
        CheckTireCollision(tireFront.transform.position);
        CheckTireCollision(tireBack.transform.position);
    }

    private void CheckTireCollision(Vector2 position)
    {
        #region Platform Renk Uyumu Kodu

        // OverlapCircle ile "Ground" katman�ndaki en yak�n platformu buluyoruz
        Collider2D collider = Physics2D.OverlapCircle(position, 0.3f, LayerMask.GetMask("Ground")); // Bugfixlendi 0.3f ideal de�er, daha az yapmay�n.

        if (collider != null)
        {
            // Platformun SpriteShapeRenderer bile�enine eri�iyoruz
            SpriteShapeRenderer platformRenderer = collider.GetComponent<SpriteShapeRenderer>();
            if (platformRenderer != null)
            {
                // Platform materyali
                Material platformMaterial = platformRenderer.materials[1]; // Fill materiali al�yor.
                Debug.Log(platformMaterial);

                // Renk uyumunu kontrol ediyoruz
                bool isColorMatch = false;


                // Materyal isimlerini kar��la�t�r
                if (CleanMaterialName(platformMaterial.name) == "PlatformWhite")
                {
                    isColorMatch = true;
                }
                else if (IsColorMatch(platformMaterial, PlatforMaterials[0], carMaterialController.redMaterial)) // IsColorMatch fonksiyonuna g�nderiyor kontrol sa�l�yor.
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
                // E�er renk uyumluysa isTrigger'� false yap, de�ilse true yap
                if (isColorMatch)
                {
                    // Renk uyumlu ise hemen ge�
                    collider.isTrigger = false;
                }
                else
                {
                    // Renk uyumsuz ise 0.5 saniye bekleyip ge�
                    StartCoroutine(SetTriggerWithDelay(collider, true, colorChangeDelay));
                }
                #endregion

            }
        }
        #endregion
    }

    // Platformlar aras� ge�i�te s�k�nt� yaratmamas� i�in delay eklendi.
    private IEnumerator SetTriggerWithDelay(Collider2D collider, bool triggerValue, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.isTrigger = triggerValue; // Uyumlu de�ilse trigger'� true yap

        // bekledikten sonra isGrounded'� false yap
        StartCoroutine(SetIsGroundedFalseWithDelay()); // Platformlar aras� rotation sorununun ��z�lmesi i�in eklendi.
    }

    private IEnumerator SetIsGroundedFalseWithDelay()
    {
        yield return new WaitForSeconds(0.2f); // min de�er 0.2f azalt�lmamal�, artt�r�labilir duruma g�re.
        tireMovementSc.isGrounded = false; // gecikmeli olarak isGrounded false yap�l�r platform ge�i�lerindeki bugu �nlemesi i�in yap�ld�.
    }

    #region Platform + Ara� Material Kontrol�
    private bool IsColorMatch(Material platformMaterial, Material platformReferenceMaterial, Material carMaterial)
    {
        return CleanMaterialName(platformMaterial.name) == CleanMaterialName(platformReferenceMaterial.name) && carMaterialController.currentMat == carMaterial; //mevcut platform + koda atanan material + araban�n rengi
    }
    #endregion

    #region Instance Yazisini Kald�r (Clean Name)

    private string CleanMaterialName(string materialName) // Instance yaz�s�n� yok ediyoruz.
    {
        return materialName.Replace(" (Instance)", "");
    }
    #endregion
}
