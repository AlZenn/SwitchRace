using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        startTime = Time.time; // Oyunun başlangıç süresini kaydet
        winPanel.SetActive(false); // Oyuna başlarken win ekranını gizle
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
    }

    void ShowStars(float finishTime)
    {
        int randomText = Random.Range(0, 3);
        if (finishTime <= threeStarsCountTime) // En iyi süre aralığı
        {
            stars[0].SetActive(true); // 3 yıldız göster
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            threeStarTexts[randomText].SetActive(true);
        }
        else if (finishTime <= twoStarsCountTime) // Orta süre aralığı
        {
            stars[0].SetActive(true); // 2 yıldız göster
            stars[1].SetActive(true);
            stars[2].SetActive(false);
            twoStarTexts[randomText].SetActive(true);
        }
        else // Daha uzun sürede bitirme
        {
            stars[0].SetActive(true); // 1 yıldız göster
            stars[1].SetActive(false);
            stars[2].SetActive(false);
            oneStarTexts[randomText].SetActive(true);
        }
    }
}
