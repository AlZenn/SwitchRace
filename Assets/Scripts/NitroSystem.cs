using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroSystem : MonoBehaviour
{
    public TireMovement_SC tireMovement;  // TireMovement_SC referansı
    public Material[] nitroMaterials;      // Nitro için 4 farklı materyal
    public float speedBoost = 10f;         // Hız artırma miktarı
    public float boostDuration = 5f;       // Hızlanma süresi
    public ParticleSystem nitroParticles;   // Nitro partikül sistemi
    public GameObject bodyMaterial;

    private bool isBoosted = false;         // Hızlanma durumu kontrolü
    private float normalSpeed = 0f;         // Oyuncunun normal hızı
    private float nitroSpeed = 0f;          // Nitro hızı

    private void Start()
    {
        // Oyuncunun normal hızını kaydet
        normalSpeed = tireMovement.MaxSpeed; // TireMovement_SC içindeki maksimum hızı kullan
    }

    private void Update()
    {
        // Nitro hızını azalt
        if (nitroSpeed > 0)
        {
            nitroSpeed -= (speedBoost / boostDuration) * Time.deltaTime; // Nitro hızı azalacak
            if (nitroSpeed < 0)
            {
                nitroSpeed = 0; // Hız 0'ın altına düşmesin
            }

            // Tekerlek hareketinde nitro hızını uygula
            tireMovement.nitroSpeed = nitroSpeed; // Nitro hızını TireMovement'a aktar
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Nitro")) // Nitro'ya temas kontrolü
        {
            Material playerMaterial = bodyMaterial.GetComponent<SpriteRenderer>().material; // Oyuncunun materyalini al
            Material nitroMaterial = other.GetComponent<Renderer>().material; // Nitro'nun materyalini al

            // Materyallerin renklerini karşılaştırarak eşleşme kontrolü yapıyoruz
            if (playerMaterial.color == nitroMaterial.color)
            {
                StartCoroutine(SpeedBoost()); // Hız artırma işlemi başlat
                Destroy(other.gameObject);
                Debug.Log("Nitro alındı");
            }
            else
            {
                EndGame();
            }
        }
    }

    private IEnumerator SpeedBoost()
    {
        if (!isBoosted) // Eğer oyuncu daha önce hızlanmadıysa
        {
            isBoosted = true;
            // Oyuncunun hızını artır
            tireMovement.MaxSpeed += speedBoost; // TireMovement_SC içindeki MaxSpeed'i artır
            nitroSpeed = speedBoost; // Nitro hızını ayarla
            nitroParticles.Play();

            float elapsedTime = 0f; // Geçen süre

            while (elapsedTime < boostDuration)
            {
                elapsedTime += Time.deltaTime; // Geçen süreyi güncelle
                yield return null; // Bir sonraki frame'e geç
            }

            // Süre sonunda oyuncunun hızını normale döndür
            tireMovement.MaxSpeed -= speedBoost; // Nitro hızını geri al
            nitroParticles.Stop();
            isBoosted = false;
        }
    }

    private void EndGame()
    {
        // Oyun sonlandırma işlemi (örneğin, sahneyi yeniden başlat)
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
