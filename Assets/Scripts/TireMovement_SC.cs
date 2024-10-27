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
    [SerializeField] private float _speed = 150f;  // Tekerlek hareket hýzý
    [SerializeField] public float maxSpeed = 300f;  // Tekerlekler için maksimum tork
    [SerializeField] private float minSpeed = 0f;   // Tekerlekler için minimum tork
    [SerializeField] private float rotationSpeed = 100f;  // Araç dönme hýzý
    [SerializeField] private float airRotation = 30f; // Araç havadayken dönme hýzý

    public float nitroSpeed = 0f; // Nitro hýzý ekledik

    private Rigidbody2D rb;
    public bool isGrounded = false; // Araç yere temas ediyor mu?
    private bool isInteractButtonGas; // Ýleri butonuna temas kontrolü
    private bool isInteractButtonBreak; // Fren butonuna temas kontrolü

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
        if (isGrounded)
        {
            // gas butonuna basýlýnca hareket etmesi
            if (isInteractButtonGas)
            {
                if (Mathf.Abs(_frontTireRB.angularVelocity) < maxSpeed)
                {
                    _frontTireRB.AddTorque((-_speed + nitroSpeed) * Time.fixedDeltaTime); // Nitro hýzýný ekle
                }

                if (Mathf.Abs(_backTireRB.angularVelocity) < maxSpeed)
                {
                    _backTireRB.AddTorque((-_speed + nitroSpeed) * Time.fixedDeltaTime); // Nitro hýzýný ekle
                }
            }

            // Apply braking force when Break button is pressed
            if (isInteractButtonBreak)
            {
                if (_backTireRB.angularVelocity > minSpeed && _backTireRB.angularVelocity < maxSpeed)
                {
                    _backTireRB.AddTorque(_speed * Time.fixedDeltaTime);  // Ýleriye tork uygula
                }
                else
                {
                    _backTireRB.angularVelocity = 0; // hýz 0 ise tekerleði durdur
                }

                if (_frontTireRB.angularVelocity > minSpeed && _frontTireRB.angularVelocity < maxSpeed)
                {
                    _frontTireRB.AddTorque(_speed * Time.fixedDeltaTime);  // Tersine tork uygula
                }
                else
                {
                    _frontTireRB.angularVelocity = 0; // hýz 0 ise tekerleði durdur
                }
            }
        }

        // Aracýn dönmesi ve hava dönüþü
        if (!isGrounded)
        {
            float rotationAmount = rotationSpeed * Time.fixedDeltaTime;

            if (isInteractButtonGas)
            {
                rb.MoveRotation(rb.rotation - rotationAmount); // Ýleri döndürme
            }
            else if (isInteractButtonBreak)
            {
                rb.MoveRotation(rb.rotation + rotationAmount); // Geri döndürme
            }
            else
            {
                rb.MoveRotation(rb.rotation - airRotation * Time.fixedDeltaTime); // Dönüþ yavaþlar
            }
        }
    }

    // Gaz ve Fren kontrol fonksiyonlarý
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
