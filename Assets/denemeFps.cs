using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class denemeFps : MonoBehaviour
{
    public Text fpsText; // UI Text element to display FPS
    private float deltaTime = 0.0f;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        // Calculate deltaTime for FPS calculation
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        // Calculate FPS
        float fps = 1.0f / deltaTime;

        // Display FPS as integer
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}