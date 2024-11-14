using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevelIndex;
    public int maxUnlockedLevel;

    void Start()
    {
        // Daha önce açılmış en yüksek seviyeyi al
        maxUnlockedLevel = PlayerPrefs.GetInt("MaxUnlockedLevel", 1); // Varsayılan olarak ilk seviye açık
    }

    public void CompleteLevel()
    {
        if (currentLevelIndex >= maxUnlockedLevel)
        {
            // Yeni bir seviye açılırsa güncelle
            maxUnlockedLevel = currentLevelIndex + 1;
            PlayerPrefs.SetInt("MaxUnlockedLevel", maxUnlockedLevel); // Yeni açılan seviyesi kaydet
            PlayerPrefs.Save();
        }
    }

}

