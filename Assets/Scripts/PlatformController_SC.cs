using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D; // SpriteShapeRenderer için gerekli

public class PlatformController_SC : MonoBehaviour
{
    [Header("Tire Object")]
    public GameObject tire; // Tek bir tekerlek GameObject
    public Collider2D ColliderZen; // Tek bir Collider2D'nin adı ColliderZen olarak değiştirildi

    [Header("Scripts")]
    public TireMovement_SC tireMovementSc;
    private CarMaterialController carMaterialController;

    [Header("Platform Settings")]
    [SerializeField] private float colorChangeDelay = 0f;
    public Material[] PlatforMaterials;
    [SerializeField] private float radius = 1.6f; // Radius alanı eklendi
    public float groundTime = 1f;

    private bool isColorMatch = false; // Global değişken olarak isColorMatch tanımlanıyor

    private void Awake()
    {
        tire = GameObject.Find("Player");
        // Tekerlek collider'ını alıyoruz
        ColliderZen = tire.GetComponent<Collider2D>(); // collider yerine ColliderZen kullanılıyor

        // CarMaterialController scriptine erişim sağlıyoruz
        carMaterialController = GetComponent<CarMaterialController>();
    }

    private void Update()
    {
        CheckPlatformCollision();
    }

    private void CheckPlatformCollision()
    {
        // Tekerleğin temas ettiği "Ground" katmanındaki platformları kontrol ediyoruz
        Vector2 position = tire.transform.position;
        CheckTireCollision(position);
    }

    private void CheckTireCollision(Vector2 position)
    {
        #region Platform Renk Uyumu Kodu

        // OverlapCircleAll ile "Ground" katmanındaki tüm temas eden colliderları buluyoruz
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, LayerMask.GetMask("Ground"));
        Collider2D nearestCollider = null;
        float minDistance = float.MaxValue;

        // En yakın collider'ı buluyoruz
        foreach (var col in colliders)
        {
            float distance = Vector2.Distance(position, col.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCollider = col;
            }
        }

        if (nearestCollider != null)
        {
            // Platformun SpriteShapeRenderer bileşenine erişiyoruz
            SpriteShapeRenderer platformRenderer = nearestCollider.GetComponent<SpriteShapeRenderer>();
            if (platformRenderer != null)
            {
                // Platform materyali
                Material platformMaterial = platformRenderer.materials[1]; // Fill materyalini alıyoruz.
                Debug.Log(platformMaterial);

                // Renk uyumunu kontrol ediyoruz
                isColorMatch = false; // Önce isColorMatch'i false yapıyoruz

                // Materyal isimlerini karşılaştır
                if (CleanMaterialName(platformMaterial.name) == "PlatformWhite")
                {
                    isColorMatch = true;
                }
                else if (IsColorMatch(platformMaterial, PlatforMaterials[0], carMaterialController.redMaterial))
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
                // Eğer renk uyumluysa isTrigger'ı false yap, değilse true yap
                if (isColorMatch)
                {
                    // Renk uyumlu ise hemen geç
                    nearestCollider.isTrigger = false;
                }
                else
                {
                    // Renk uyumsuz ise 0.5 saniye bekleyip geç
                    StartCoroutine(SetTriggerWithDelay(nearestCollider, true, colorChangeDelay));
                }
                #endregion
            }
        }
        #endregion
    }

    private IEnumerator SetTriggerWithDelay(Collider2D collider, bool triggerValue, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.isTrigger = triggerValue; // Uyumlu değilse trigger'ı true yap

        // CheckPlatformCollision metodunu tekrar çağırarak isColorMatch kontrolünü yap
        CheckPlatformCollision(); // Platform kontrolünü yeniden yap

        // Eğer renk uyumsuzsa (isColorMatch false ise), SetIsGroundedFalseWithDelay'i başlat
        if (isColorMatch) // isColorMatch global bir değişken olarak kontrol ediliyor
        {
            StartCoroutine(SetIsGroundedFalseWithDelay()); // Platformlar arası rotation sorununun çözülmesi için eklendi.
        }
    }

    private IEnumerator SetIsGroundedFalseWithDelay()
    {
        yield return new WaitForSeconds(groundTime); // min değer 0.2f azaltılmamalı, arttırılabilir duruma göre.
        tireMovementSc.isGrounded = false; // gecikmeli olarak isGrounded false yapılır platform geçişlerindeki bugu önlemesi için yapıldı.
    }

    #region Platform + Araç Material Kontrolü
    private bool IsColorMatch(Material platformMaterial, Material platformReferenceMaterial, Material carMaterial)
    {
        return CleanMaterialName(platformMaterial.name) == CleanMaterialName(platformReferenceMaterial.name) && carMaterialController.currentMat == carMaterial; // mevcut platform + koda atanan material + arabanın rengi
    }
    #endregion

    #region Instance Yazısını Kaldır (Clean Name)
    private string CleanMaterialName(string materialName) // Instance yazısını yok ediyoruz.
    {
        return materialName.Replace(" (Instance)", "");
    }
    #endregion

    // Gizmos çizimi için OnDrawGizmosSelected metodu
    private void OnDrawGizmosSelected()
    {
        if (tire != null)
        {
            // Gizmos rengi ve konumu ayarlanıyor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(tire.transform.position, radius);
        }
    }
}
