using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAreaSlider_SC : MonoBehaviour
{
    [Header("Player & Target")]
    [SerializeField] private GameObject player; // player atamas�
    [SerializeField] private GameObject target; // hedef end game atamas�

    [Header("Slider & SliderText")]
    [SerializeField] private Slider GameArenaSlider; // slider
    [SerializeField] private Text HandleText; // slider handle text

    private float initialDistance; // ba�lang�� mesafesi

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("FinishLine");
    }

    void Start()
    {
        // �lk mesafeyi kaydediyoruz ki oyuncu hedefe yakla�t�k�a bu mesafeye g�re slider de�erini hesaplayabilelim
        initialDistance = Vector3.Distance(player.transform.position, target.transform.position); // vector3.distance 2 de�er aras�ndaki mesafeyi hesaplar.
        

        // Bugfix
        GameArenaSlider.value = 0f;
        GameArenaSlider.interactable = false;
    }

    void Update()
    {
        // Anl�k mesafeyi hesaplay�p text'e yaz�yoruz
        float currentDistance = Vector3.Distance(player.transform.position, target.transform.position);
        HandleText.text = currentDistance.ToString("F0") + "m"; 

        // Slider de�erini g�ncelliyoruz, hedefe yakla�t�k�a 1'e yakla��r
        GameArenaSlider.value = Mathf.Clamp01(1 - (currentDistance / initialDistance)); // mathf.clamp 0 ile 1 aras�nda de�er d�nd�r�r slider i�in gerekli slider 0-1 aras�nda value al�yor. 
        // mevcut mesafeden ba�lang�c� ��kart�yoruz.
    }
}