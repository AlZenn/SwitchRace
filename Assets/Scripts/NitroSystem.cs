using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NitroSystem : MonoBehaviour
{
    public CarMaterialController carMaterialController;  // Arabanın materyalini kontrol eden script
    public Material[] nitroMaterials;  // Nitro için 4 farklı materyal
    public float speedBoost = 5f;      // Hız artırma miktarı
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

            yield return new WaitForSeconds(boostDuration); // 5 saniye bekle

            // Oyuncunun hızını normale döndür
            player.GetComponent<CarMovement_SC>().acceleration = normalSpeed;
            nitroParticles.Stop();
            isBoosted = false;
        }
    }

    private void EndGame()
    {
        // Oyun sonlandırma işlemi (örneğin, sahneyi yeniden başlat)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
