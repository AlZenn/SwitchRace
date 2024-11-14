using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
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
    
    public float timeElapsed = 0f; // Geçen süreyi tutacak
    public TextMeshProUGUI timerTextTMP; // TextMeshPro kullanıyorsan
    
    private float startTime;
    private LevelManager levelManager;
    AudioManager audioManager;
    private GameObject gameplayhud, Camera;
    private bool win = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        gameplayhud = GameObject.Find("GameplayHuds");
        Camera = GameObject.Find("Cinemachine VirtualCamera");
    }

    void Start()
    {
        startTime = Time.time; // Oyunun başlangıç süresini kaydet
        winPanel.SetActive(false); // Oyuna başlarken win ekranını gizle
        levelManager = FindObjectOfType<LevelManager>(); // LevelManager'ı bul
    }
    
    private void Update()
    {
        // Süreyi güncelle
        timeElapsed += Time.deltaTime;

        // Saniye ve milisaniyeyi al
        string seconds = Mathf.Floor(timeElapsed).ToString("00"); // Saniye kısmı
        string milliseconds = ((timeElapsed % 1) * 1000).ToString("00"); // Milisaniye kısmı

        if (timerTextTMP != null)
            timerTextTMP.text = seconds + ":" + milliseconds;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FinishLine") && win == false)
        {
            Win(other.gameObject); // Bitiş çizgisine ulaşırsa Win fonksiyonunu çağır
        }
    }

    void Win(GameObject finishLine)
    {
        win = true;
        winPanel.SetActive(true); // Win ekranını göster

        gameplayhud.GetComponent<CanvasGroup>().alpha = 0f;
        Camera.GetComponent<CinemachineVirtualCamera>().Follow = finishLine.transform;
        
        float finishTime = Time.time - startTime; // Geçen süreyi hesapla
        ShowStars(finishTime); // Süreye göre yıldız sayısını belirle
        if (levelManager !=null)
        {
            levelManager.CompleteLevel(); // Seviye tamamlandı

        }
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
