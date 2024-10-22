using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement_SC : MonoBehaviour
{
    [Header("Character Movement Buttons")] // ara� kontrol butonlar�n� atama yeri
    [SerializeField] public Button button_gas; // ileri butonu atanacak.
    [SerializeField] private Button button_break; // geri butonu atanacak.

    [Header("Car Properties")] // Arac�n �zellikleri
    [SerializeField] public float acceleration = 10f; // ara� h�zlanma de�eri.
    [SerializeField] private float deceleration = 25f; // ara� yava�lama de�eri.

    [SerializeField] private float maxSpeed = 50f;  // arac�n maksimum h�z�.
    [SerializeField] private float minSpeed = 0f;   // arac�n minimum h�z�
    [SerializeField] private float rotationSpeed = 100f;  // arac�n d�nme h�z�.
    [SerializeField] private float airRotation = 30f;

    private float currentSpeed = 0f; // arac�n mevcut/ba�lang�� h�z�.

    private bool isGrounded = false; // arac�n yerde olup olmad�g�, rotation de�eri i�in gerekli.
    private bool isInteractButtonGas; // gas butonu ile temas
    private bool isInteractButtonBreak; // fren butonu ile temas

    public float surtunme = 1f;

    private Rigidbody2D rb;
    public static CarMovement_SC instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (isGrounded == true) // ara� hareket kodu, ara� yerde ise �al���r
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
            else if (!Input.anyKey && currentSpeed > minSpeed)
            {
                currentSpeed -= surtunme;
                //Debug.Log(currentSpeed);
            }


            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);  // Araban�n yatay h�z�n� ayarla
        }
        if (!isGrounded) // ara� rotation kodu, ara� havada ise �al���r
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

    // -zen1.1- bu k�s�m butonlara event trigger olarak atal� -zen1.1-
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
    /// -zen1.1- bu k�s�m butonlara event trigger olarak atal� -zen1.1-


    /// -zen1.2- ara� yerde mi kontrol scripti -zen1.2-
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
    /// -zen1.2- ara� yerde mi kontrol scripti -zen1.2-
}
