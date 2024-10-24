using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NitroSystem : MonoBehaviour
{
    public CarMaterialController carMaterialController;  // Arabanın materyalini kontrol eden script
    public Material[] nitroMaterials;  // Nitro için 4 farklı materyal
    public float speedBoost = 10f;      // Hız artırma miktarı
    public float boostDuration = 5f;   // Hızlanma süresi
    public GameObject player;          // Oyuncu GameObject'i
    public ParticleSystem nitroParticles;

    private bool isBoosted = false;    // Hızlanma durumu kontrolü
    private float normalSpeed = 0f;    // Oyuncunun normal hızı

    private void Start()
    {
        // Oyuncunun normal hızını kaydet
        normalSpeed = player.GetComponent<CarMovement_SC>().acceleration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Nitro")) // Nitro'ya temas kontrolü
        {
            Material playerMaterial = carMaterialController.carRenderer.material; // Butonlardan alınan güncel oyuncu materyali
            Material nitroMaterial = other.GetComponent<Renderer>().material; // Nitro'nun materyalini al

            // Materyallerin renklerini karşılaştırarak eşleşme kontrolü yapıyoruz
            if (playerMaterial.color == nitroMaterial.color)
            {
                StartCoroutine(SpeedBoost()); // Hız artırma işlemi başlat
            }
            else
            {
                Debug.Log("Yanlış materyal! Oyun bitti.");
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
            player.GetComponent<CarMovement_SC>().acceleration += speedBoost;
            nitroParticles.Play();
            Destroy(GameObject.FindWithTag("Nitro"), .5f);

            float elapsedTime = 0f; // Geçen süre
            float decreaseAmount = 2f; // Hız azalması miktarı

            while (elapsedTime < boostDuration)
            {
                player.GetComponent<CarMovement_SC>().acceleration -= decreaseAmount * Time.deltaTime; // Hızı azalt
                elapsedTime += Time.deltaTime; // Geçen süreyi güncelle
                yield return null; // Bir sonraki frame'e geç
            }

            // Süre sonunda oyuncunun hızını normale döndür
            player.GetComponent<CarMovement_SC>().acceleration = normalSpeed;
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