using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FinishLine : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject[] stars;
    [SerializeField] GameObject[] threeStarTexts;
    [SerializeField] GameObject[] twoStarTexts;
    [SerializeField] GameObject[] oneStarTexts;
    [SerializeField] float threeStarsCountTime;
    [SerializeField] float twoStarsCountTime;
    private float startTime;
    private LevelManager levelManager;

    void Start()
    {
        startTime = Time.time;
        winPanel.SetActive(false);
        levelManager = FindObjectOfType<LevelManager>(); // LevelManager'ı bul
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            Win();
        }
    }

    void Win()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;
        float finishTime = Time.time - startTime;
        ShowStars(finishTime);
        levelManager.CompleteLevel(); // Seviye tamamlandı
    }

    void ShowStars(float finishTime)
    {
        int randomText = Random.Range(0, 3);
        if (finishTime <= threeStarsCountTime)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            threeStarTexts[randomText].SetActive(true);
        }
        else if (finishTime <= twoStarsCountTime)
        {
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(false);
            twoStarTexts[randomText].SetActive(true);
        }
        else
        {
            stars[0].SetActive(true);
            stars[1].SetActive(false);
            stars[2].SetActive(false);
            oneStarTexts[randomText].SetActive(true);
        }
    }
}

