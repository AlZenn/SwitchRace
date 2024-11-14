using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireMovement_SC : MonoBehaviour
{
    [Header("Tire Rigidbodies")]
    [SerializeField] private Rigidbody2D _frontTireRB;
    [SerializeField] private Rigidbody2D _backTireRB;

    [Header("Car Properties")]
    [SerializeField] private float _speed = 350f;  // Tekerlek hareket hızı
    [SerializeField] public float maxSpeed = 3500f;  // Tekerlekler için maksimum hız
    [SerializeField] private float minSpeed = 4f;   // Tekerlekler için minimum hız
    [SerializeField] private float rotationSpeed = 180f;  // Araç dönüş hızı
    [SerializeField] private float airRotation = 30f; // Araç havadayken dönüş hızı

    [Header("NitroProperties")]
    public float nitroSpeed = 0f; // Nitro hızı

    [Header("Audio Settings")]
    public AudioSource engineAudioSource; // Motor sesi kaynağı
    public AudioClip engineClip;          // Motor sesi klibi
    public float minPitch = 0.8f;         // Minimum pitch değeri (rölanti sesi)
    public float maxPitch = 2.0f;         // Maksimum pitch değeri (hızlı sürüş sesi)

    private Rigidbody2D rb;
    public bool isGrounded = false;       // Araç yere temas ediyor mu?
    private bool isInteractButtonGas;     // Gaz butonuna basılma durumu
    private bool isInteractButtonBreak;   // Fren butonuna basılma durumu

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Player'in rigidbodysini aldık
        
        // Motor sesi kaynağını ayarlıyoruz
        engineAudioSource.clip = engineClip;
        engineAudioSource.loop = true;
        engineAudioSource.Play();
    }

    void Update()
    {
        // W ve S tuşları ile gaz ve fren kontrolü
        if (Input.GetKeyDown(KeyCode.W))
        {
            gasTrue();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            gasFalse();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            breakTrue();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            breakFalse();
        }
    }
    
    void FixedUpdate()
    {
        // Gaz butonuna basıldığında hızlanma ve ses tonunu artırma
        if (isGrounded && isInteractButtonGas)
        {
            if (Mathf.Abs(_frontTireRB.angularVelocity) < maxSpeed)
            {
                _frontTireRB.AddTorque((-_speed + nitroSpeed) * Time.fixedDeltaTime);
            }

            if (Mathf.Abs(_backTireRB.angularVelocity) < maxSpeed)
            {
                _backTireRB.AddTorque((-_speed + nitroSpeed) * Time.fixedDeltaTime);
            }
        }

        // Fren butonuna basıldığında fren kuvveti uygulama
        if (isGrounded && isInteractButtonBreak)
        {
            ApplyBrakes();
        }

        // Aracın havada dönüş hareketi
        if (!isGrounded)
        {
            AirRotation();
        }

        UpdateEngineSound(); // Motor sesini güncelle
    }

    // Gaz ve Fren kontrol fonksiyonları
    public void gasTrue() => isInteractButtonGas = true;
    public void gasFalse() => isInteractButtonGas = false;
    public void breakTrue() => isInteractButtonBreak = true;
    public void breakFalse() => isInteractButtonBreak = false;

    private void UpdateEngineSound()
    {
        // Calculate the current speed factor relative to max speed
        float speedFactor = Mathf.Abs(_frontTireRB.angularVelocity) / maxSpeed;
    
        // Map speed to pitch range
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, speedFactor);

        // Apply the pitch only if gas is pressed; otherwise, return to min pitch
        if (isInteractButtonGas)
        {
            engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, targetPitch, Time.deltaTime * 2f);
        }
        else
        {
            engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, minPitch, Time.deltaTime * 2f);
        }
    }



    private void ApplyBrakes()
    {
        // Fren kuvveti uygula ve durdurma işlemi
        if (_backTireRB.angularVelocity > minSpeed && _backTireRB.angularVelocity < maxSpeed)
        {
            _backTireRB.AddTorque(_speed * Time.fixedDeltaTime);
        }
        else
        {
            _backTireRB.angularVelocity = 0;
        }

        if (_frontTireRB.angularVelocity > minSpeed && _frontTireRB.angularVelocity < maxSpeed)
        {
            _frontTireRB.AddTorque(_speed * Time.fixedDeltaTime);
        }
        else
        {
            _frontTireRB.angularVelocity = 0;
        }
    }

    private void AirRotation()
    {
        float rotationAmount = rotationSpeed * Time.fixedDeltaTime;

        if (isInteractButtonGas)
        {
            rb.MoveRotation(rb.rotation + rotationAmount);
        }
        else if (isInteractButtonBreak)
        {
            rb.MoveRotation(rb.rotation - rotationAmount);
        }
        else
        {
            rb.MoveRotation(rb.rotation - airRotation * Time.fixedDeltaTime);
        }
    }
}
