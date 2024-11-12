using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    
    private float timer, refresh, avgFramerate;
    private string display = "{0} FPS";
    public TextMeshProUGUI fpsText;

    private void Awake()
    {
        if (pausePanel!=null)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        
    }

    private void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
            float timelapse = Time.smoothDeltaTime;
            timer = timer <= 0 ? refresh : timer -= timelapse;

            if(timer <=0) avgFramerate = (int) (1f / timelapse);
            fpsText.text = string.Format(display,avgFramerate.ToString());
        
    }

    public void Level1Loader()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void MainMenuLoader()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void LevelAgainLoader()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ContiniousLevelLoader()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        Time.timeScale = 1;
    }

    public void PauseButton()
    {
        if (pausePanel!=null)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        
        
    }

    public void ResumeButton()
    {
        if (pausePanel!=null)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    
    // Seviye adını string ile alarak sahneyi yükleyen fonk
    public void LoadLevelString(string levelName)
    {
        // Sahnenin mevcut olup olmadığını kontrol et
        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
            Debug.Log($"{levelName} sahnesi yüklendi.");
        }
        else
        {
            Debug.LogError($"{levelName} isimli bir sahne mevcut değil.");
        }
    }
}

