using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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
    [SerializeField] private float colorChangeDelay = 0f; // Renk değişim gecikmesi ayarlanabilir
    [SerializeField] private float groundedFalseDelay = 0.5f; // isGrounded false gecikmesi
    public Material[] PlatforMaterials;

    private bool lastColorMatch = false; // Son isColorMatch durumunu takip eden bir değişken

    private void Start()
    {
        tireFrontCollider = tireFront.GetComponent<Collider2D>();
        tireBackCollider = tireBack.GetComponent<Collider2D>();

        // TireMovement_SC ve CarMaterialController komponentlerini kontrol et
        tireMovementSc = GetComponent<TireMovement_SC>();
        carMaterialController = GetComponent<CarMaterialController>();

        if (tireMovementSc == null)
        {
            Debug.LogError("TireMovement_SC component is not assigned!");
        }

        if (carMaterialController == null)
        {
            Debug.LogError("CarMaterialController component is not assigned!");
        }
    }


    private void Update()
    {
        CheckPlatformCollision();
    }

 private void CheckPlatformCollision()
{
    // Ön tekerleği referans alarak kontrol et
    Vector2 frontTirePosition = tireFront.transform.position;

    // Tekerleklerin pozisyonlarına göre platform çarpışmalarını kontrol et
    CheckPlatformCollisionAtPosition(frontTirePosition); // Ön tekerlek pozisyonu ile kontrol et
    CheckPlatformCollisionAtPosition(tireBack.transform.position); // Arka tekerlek pozisyonu ile kontrol et
}

private void CheckPlatformCollisionAtPosition(Vector2 position)
{
    // Platformların etrafındaki çarpışma kontrolleri
    Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.7f, LayerMask.GetMask("Ground"));

    foreach (var collider in colliders)
    {
        if (collider != null)
        {
            SpriteShapeRenderer platformRenderer = collider.GetComponent<SpriteShapeRenderer>();
            if (platformRenderer != null)
            {
                Material platformMaterial = platformRenderer.materials[1];
                Debug.Log(platformMaterial);

                bool isColorMatch = false;

                // Burada renk eşleşmesini kontrol ediyoruz
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

                // Son duruma göre gecikmeli işlem yapılması
                if (isColorMatch != lastColorMatch) // Renk durumu değişmişse
                {
                    if (isColorMatch)
                    {
                        collider.isTrigger = false;
                        tireMovementSc.isGrounded = true; // Renk uyumlu olduğunda isGrounded true yapılır
                    }
                    else
                    {
                        StartCoroutine(SetTriggerWithDelay(collider, true, colorChangeDelay));
                    }
                    lastColorMatch = isColorMatch; // Son renk eşleşme durumunu güncelle
                }
            }
        }
    }
}


    private IEnumerator SetTriggerWithDelay(Collider2D collider, bool triggerValue, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.isTrigger = triggerValue;

        // Renk uyumsuz olduğunda gecikmeli olarak isGrounded false yapılır
        StartCoroutine(SetIsGroundedFalseWithDelay());
    }

    private IEnumerator SetIsGroundedFalseWithDelay()
    {
        yield return new WaitForSeconds(groundedFalseDelay); // Gecikmeli olarak isGrounded false yapar
        tireMovementSc.isGrounded = false;
    }

    private bool IsColorMatch(Material platformMaterial, Material platformReferenceMaterial, Material carMaterial)
    {
        return CleanMaterialName(platformMaterial.name) == CleanMaterialName(platformReferenceMaterial.name) && carMaterialController.currentMat == carMaterial;
    }

    private string CleanMaterialName(string materialName)
    {
        return materialName.Replace(" (Instance)", "");
    }

    private void OnDrawGizmos()
    {
        if (tireFront != null)
        {
            // Tire'nin etrafında çizilecek çemberin yarıçapı
            Gizmos.color = Color.green; // Çemberin rengi
            Gizmos.DrawWireSphere(tireFront.transform.position, 0.5f); // Tire pozisyonunda 0.5 yarıçapında çember çizer
        }
    }
}