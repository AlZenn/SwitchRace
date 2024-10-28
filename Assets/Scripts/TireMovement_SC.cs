using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TireMovement_SC : MonoBehaviour
{
    [Header("Tire Rigidbodies")]
    [SerializeField] private Rigidbody2D _frontTireRB;
    [SerializeField] private Rigidbody2D _backTireRB;

    [Header("Car Properties")]
    [SerializeField] private float _speed = 150f;  // Tekerlek hareket h�z�
    [SerializeField] public float maxSpeed = 300f;  // Tekerlekler i�in maksimum tork
    [SerializeField] private float minSpeed = 0f;   // Tekerlekler i�in minimum tork
    [SerializeField] private float rotationSpeed = 100f;  // Ara� d�nme h�z�
    [SerializeField] private float airRotation = 30f; // Ara� havadayken d�nme h�z�

    public float nitroSpeed = 0f; // Nitro h�z� ekledik

    private Rigidbody2D rb;
    public bool isGrounded = false; // Ara� yere temas ediyor mu?
    private bool isInteractButtonGas; // �leri butonuna temas kontrol�
    private bool isInteractButtonBreak; // Fren butonuna temas kontrol�

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
        if (isGrounded)
        {
            // gas butonuna bas�l�nca hareket etmesi
            if (isInteractButtonGas)
            {
                if (Mathf.Abs(_frontTireRB.angularVelocity) < maxSpeed)
                {
                    _frontTireRB.AddTorque((-_speed + nitroSpeed) * Time.fixedDeltaTime); // Nitro h�z�n� ekle
                }

                if (Mathf.Abs(_backTireRB.angularVelocity) < maxSpeed)
                {
                    _backTireRB.AddTorque((-_speed + nitroSpeed) * Time.fixedDeltaTime); // Nitro h�z�n� ekle
                }
            }

            // Apply braking force when Break button is pressed
            if (isInteractButtonBreak)
            {
                if (_backTireRB.angularVelocity > minSpeed && _backTireRB.angularVelocity < maxSpeed)
                {
                    _backTireRB.AddTorque(_speed * Time.fixedDeltaTime);  // �leriye tork uygula
                }
                else
                {
                    _backTireRB.angularVelocity = 0; // h�z 0 ise tekerle�i durdur
                }

                if (_frontTireRB.angularVelocity > minSpeed && _frontTireRB.angularVelocity < maxSpeed)
                {
                    _frontTireRB.AddTorque(_speed * Time.fixedDeltaTime);  // Tersine tork uygula
                }
                else
                {
                    _frontTireRB.angularVelocity = 0; // h�z 0 ise tekerle�i durdur
                }
            }
        }

        // Arac�n d�nmesi ve hava d�n���
        if (!isGrounded)
        {
            float rotationAmount = rotationSpeed * Time.fixedDeltaTime;

            if (isInteractButtonGas)
            {
                rb.MoveRotation(rb.rotation + rotationAmount); // �leri d�nd�rme
            }
            else if (isInteractButtonBreak)
            {
                rb.MoveRotation(rb.rotation - rotationAmount); // Geri d�nd�rme
            }
            else
            {
                rb.MoveRotation(rb.rotation + airRotation * Time.fixedDeltaTime); // D�n�� yava�lar
            }
        }
    }

    // Gaz ve Fren kontrol fonksiyonlar�
    public void gasTrue() => isInteractButtonGas = true;
    public void gasFalse() => isInteractButtonGas = false;
    public void breakTrue() => isInteractButtonBreak = true;
    public void breakFalse() => isInteractButtonBreak = false;

    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }
}
