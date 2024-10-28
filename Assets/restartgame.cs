using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restartgame : MonoBehaviour
{
    public void RestartCurrentLevel()
    {
        // Mevcut seviyenin adını al ve tekrar yükle.
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
