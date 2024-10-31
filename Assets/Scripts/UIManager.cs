using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    private void Awake()
    {
        pausePanel.SetActive(false);
    }
    public void Level1Loader()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void MainMenuLoader()
    {
        SceneManager.LoadScene(0);
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
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeButton()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
