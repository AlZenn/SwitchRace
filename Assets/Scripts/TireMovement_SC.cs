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
    [SerializeField] private float _speed = 350f;  // Tekerlek hareket hızı
    [SerializeField] public float maxSpeed = 3500f;  // Tekerlekler için maksimum hız
    [SerializeField] private float minSpeed = 4f;   // Tekerlekler için minimum hız
    [SerializeField] private float rotationSpeed = 180f;  // Araç dönüş hızı
    [SerializeField] private float airRotation = 30f; // Araç havadayken dönüş hızı

    public float nitroSpeed = 0f; // Nitro hızı

    private Rigidbody2D rb;
    public bool isGrounded = false; // Araç yere temas ediyor mu?
    private bool isInteractButtonGas; // Gaz butonuna basılma durumu
    private bool isInteractButtonBreak; // Fren butonuna basılma durumu

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        if (isGrounded)
        {
            // Gaz butonuna basıldığında hareket etmesi
            if (isInteractButtonGas)
            {
                if (Mathf.Abs(_frontTireRB.angularVelocity) < maxSpeed)
                {
                    _frontTireRB.AddTorque((-_speed + nitroSpeed) * Time.fixedDeltaTime); // Nitro hızını ekle
                }

                if (Mathf.Abs(_backTireRB.angularVelocity) < maxSpeed)
                {
                    _backTireRB.AddTorque((-_speed + nitroSpeed) * Time.fixedDeltaTime); // Nitro hızını ekle
                }
            }

            // Fren butonuna basıldığında fren kuvveti uygula
            if (isInteractButtonBreak)
            {
                if (_backTireRB.angularVelocity > minSpeed && _backTireRB.angularVelocity < maxSpeed)
                {
                    _backTireRB.AddTorque(_speed * Time.fixedDeltaTime);  // Tork uygula
                }
                else
                {
                    _backTireRB.angularVelocity = 0; // Hız 0 ise tekerleği durdur
                }

                if (_frontTireRB.angularVelocity > minSpeed && _frontTireRB.angularVelocity < maxSpeed)
                {
                    _frontTireRB.AddTorque(_speed * Time.fixedDeltaTime);  // Tork uygula
                }
                else
                {
                    _frontTireRB.angularVelocity = 0; // Hız 0 ise tekerleği durdur
                }
            }
        }

        // Aracın dönüş hareketi ve havada dönüş
        if (!isGrounded)
        {
            float rotationAmount = rotationSpeed * Time.fixedDeltaTime;

            if (isInteractButtonGas)
            {
                rb.MoveRotation(rb.rotation + rotationAmount); // İleri dönüş
            }
            else if (isInteractButtonBreak)
            {
                rb.MoveRotation(rb.rotation - rotationAmount); // Geri dönüş
            }
            else
            {
                rb.MoveRotation(rb.rotation - airRotation * Time.fixedDeltaTime); // Yavaş dönüş
            }
        }
    }

    // Gaz ve Fren kontrol fonksiyonları
    public void gasTrue() => isInteractButtonGas = true;
    public void gasFalse() => isInteractButtonGas = false;
    public void breakTrue() => isInteractButtonBreak = true;
    public void breakFalse() => isInteractButtonBreak = false;

    public float MaxSpeed // Nitro için hızı public yaptım
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }
}
