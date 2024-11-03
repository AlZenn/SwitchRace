using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAreaSlider_SC : MonoBehaviour
{
    public GameObject player; // player atamasý
    public GameObject target; // hedef end game atamasý

    public Slider GameArenaSlider; // slider
    public Text HandleText; // slider handle text

    private float initialDistance; // baþlangýç mesafesi

    void Start()
    {
        // Ýlk mesafeyi kaydediyoruz ki oyuncu hedefe yaklaþtýkça bu mesafeye göre slider deðerini hesaplayabilelim
        initialDistance = Vector3.Distance(player.transform.position, target.transform.position); // vector3.distance 2 deðer arasýndaki mesafeyi hesaplar.
        GameArenaSlider.value = 0f;
        GameArenaSlider.interactable = false;
    }

    void Update()
    {
        // Anlýk mesafeyi hesaplayýp text'e yazýyoruz
        float currentDistance = Vector3.Distance(player.transform.position, target.transform.position);
        HandleText.text = currentDistance.ToString("F0") + "m";

        // Slider deðerini güncelliyoruz, hedefe yaklaþtýkça 1'e yaklaþýr
        GameArenaSlider.value = Mathf.Clamp01(1 - (currentDistance / initialDistance)); // mathf.clamp 0 ile 1 arasýnda deðer döndürür slider için gerekli slider 0-1 arasýnda value alýyor. 
        // mevcut mesafeden baþlangýcý çýkartýyoruz.
    }
}