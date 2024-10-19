using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement_SC : MonoBehaviour
{
    [Header("Character Movement Buttons")] // araç kontrol butonlarýný atama yeri
    [SerializeField] public Button button_gas; // ileri butonu atanacak.
    [SerializeField] private Button button_break; // geri butonu atanacak.

    [Header("Car Properties")] // Aracýn özellikleri
    [SerializeField] private float acceleration = 10f; // araç hýzlanma deðeri.
    [SerializeField] private float deceleration = 25f; // araç yavaþlama deðeri.

    [SerializeField] private float maxSpeed = 50f;  // aracýn maksimum hýzý.
    [SerializeField] private float minSpeed = 0f;   // aracýn minimum hýzý
    [SerializeField] private float rotationSpeed = 100f;  // aracýn dönme hýzý.
    [SerializeField] private float airRotation = 30f;

    private float currentSpeed = 0f; // aracýn mevcut/baþlangýç hýzý.

    private bool isGrounded = false; // aracýn yerde olup olmadýgý, rotation deðeri için gerekli.
    private bool isInteractButtonGas; // gas butonu ile temas
    private bool isInteractButtonBreak; // fren butonu ile temas

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (isGrounded == true) // araç hareket kodu, araç yerde ise çalýþýr
        {
            if (Input.GetKey(KeyCode.W) || isInteractButtonGas == true)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) && currentSpeed > minSpeed || isInteractButtonBreak == true && currentSpeed > minSpeed)
            {
                currentSpeed -= deceleration * Time.deltaTime;
            }
            else if (currentSpeed < minSpeed)
            {
                currentSpeed = 0f;
            }

            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }
        if (!isGrounded) // araç rotation kodu, araç havada ise çalýþýr
        {
            if (Input.GetKey(KeyCode.W) || isInteractButtonGas == true)
            {
                rb.rotation -= rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) && currentSpeed > minSpeed || isInteractButtonBreak == true && currentSpeed > minSpeed)
            {
                rb.rotation += rotationSpeed * Time.deltaTime;
            }
            else if (!Input.anyKey)
            {
                rb.rotation -= airRotation * Time.deltaTime;
            }
        }
    }

    // -zen1.1- bu kýsým butonlara event trigger olarak atalý -zen1.1-
    public void gasTrue() 
    {
        isInteractButtonGas = true;
    }
    public void gasFalse()
    {
        isInteractButtonGas = false;
    }
    public void breakTrue()
    {
        isInteractButtonBreak = true;
    }
    public void breakFalse()
    {
        isInteractButtonBreak = false;
    }
    /// -zen1.1- bu kýsým butonlara event trigger olarak atalý -zen1.1-


    /// -zen1.2- araç yerde mi kontrol scripti -zen1.2-
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    /// -zen1.2- araç yerde mi kontrol scripti -zen1.2-
}
