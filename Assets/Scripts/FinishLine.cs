using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FinishLine : MonoBehaviour
{
    [SerializeField] GameObject winPanel; // Win ekranı
    [SerializeField] GameObject[] stars;  // Yıldızlar (1, 2 veya 3 adet olabilir)
    [SerializeField] GameObject[] threeStarTexts;
    [SerializeField] GameObject[] twoStarTexts;
    [SerializeField] GameObject[] oneStarTexts;
    [SerializeField] float threeStarsCountTime;
    [SerializeField] float twoStarsCountTime;
    private float startTime;
    private LevelManager levelManager;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        startTime = Time.time; // Oyunun başlangıç süresini kaydet
        winPanel.SetActive(false); // Oyuna başlarken win ekranını gizle
        levelManager = FindObjectOfType<LevelManager>(); // LevelManager'ı bul
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            Win(); // Bitiş çizgisine ulaşırsa Win fonksiyonunu çağır
        }
    }

    void Win()
    {
        winPanel.SetActive(true); // Win ekranını göster
        Time.timeScale = 0; // Oyun hareketini durdur
        float finishTime = Time.time - startTime; // Geçen süreyi hesapla
        ShowStars(finishTime); // Süreye göre yıldız sayısını belirle
        levelManager.CompleteLevel(); // Seviye tamamlandı
        audioManager.PlaySFX(audioManager.winSFX);
    }

    void ShowStars(float finishTime)
    {
        int randomText = Random.Range(0, 3);
        if (finishTime <= threeStarsCountTime) // En iyi süre aralığı
        {
            stars[0].GetComponent<Image>().enabled = true; // 3 yıldız göster
            stars[1].GetComponent<Image>().enabled = true;
            stars[2].GetComponent<Image>().enabled = true;
            threeStarTexts[randomText].SetActive(true);
        }
        else if (finishTime <= twoStarsCountTime) // Orta süre aralığı
        {
            stars[0].GetComponent<Image>().enabled = true; // 2 yıldız göster
            stars[1].GetComponent<Image>().enabled = true;
            stars[2].GetComponent<Image>().enabled = false;
            twoStarTexts[randomText].SetActive(true);
        }
        else // Daha uzun sürede bitirme
        {
            stars[0].GetComponent<Image>().enabled = true; // 1 yıldız göster
            stars[1].GetComponent<Image>().enabled = false;
            stars[2].GetComponent<Image>().enabled = false;
            oneStarTexts[randomText].SetActive(true);
        }
    }
}
