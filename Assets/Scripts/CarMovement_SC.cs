using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement_SC : MonoBehaviour
{
    [Header("Character Movement Buttons")] // araç kontrol butonlarını atama yeri
    [SerializeField] public Button button_gas; // ileri butonu atanacak.
    [SerializeField] private Button button_break; // geri butonu atanacak.

    [Header("Car Properties")] // Aracın özellikleri
    [SerializeField] public float acceleration = 10f; // araç hızlanma değeri.
    [SerializeField] private float deceleration = 25f; // araç yavaşlama değeri.

    [SerializeField] private float maxSpeed = 50f;  // aracın maksimum hızı
    [SerializeField] private float minSpeed = 0f;   // aracın minimum hızı

    [SerializeField] private float rotationSpeed = 100f;  // aracın dönme hızı.
    [SerializeField] private float airRotation = 30f;
    [SerializeField] private float speedControl;

    //private float currentSpeed = 0f; // aracın mevcut/başlangıç hızı.

    private bool isGrounded = false; // aracın yerde olup olmadıgı, rotation degeri için gerekli.
    private bool isInteractButtonGas; // gas butonu ile temas
    private bool isInteractButtonBreak; // fren butonu ile temas

    public float surtunme = 0.5f;

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

        if (isGrounded == true) // araç hareket kodu, araç yerde ise çalışır
        {
            if (Input.GetKey(KeyCode.W) || isInteractButtonGas == true)
            {
                if (rb.velocity.x < maxSpeed)
                {
                rb.AddForce(Vector2.right * acceleration * speedControl);
                }
            }
            else if (Input.GetKey(KeyCode.S) && rb.velocity.x > minSpeed || isInteractButtonBreak == true && rb.velocity.x > minSpeed)
            {
                if (rb.velocity.x > minSpeed + 1) // burda addforce ile araç minik minik geriye gidiyordu +1 koyunca düzeldi ama 1f ileri gideceginden de direkt 1 hızı olursa otomatik fren yapacak
                {
                    rb.AddForce(Vector2.left * deceleration * speedControl);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            else if (!Input.anyKey && rb.velocity.x > minSpeed)
            {
                rb.AddForce(Vector2.left * surtunme);
            }
        }

        if (!isGrounded) // araç rotation kodu, araç havada ise çalışır
        {
            if (Input.GetKey(KeyCode.W) || isInteractButtonGas == true)
            {
                rb.rotation -= rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) && rb.velocity.x > minSpeed || isInteractButtonBreak == true && rb.velocity.x > minSpeed)
            {
                rb.rotation += rotationSpeed * Time.deltaTime;
            }
            else if (!Input.anyKey)
            {
                rb.rotation -= airRotation * Time.deltaTime;
            }
        }
    }

    // -zen1.1- bu kısım butonlara event trigger olarak atalı -zen1.1-
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
    /// -zen1.1- bu kısım butonlara event trigger olarak atalı -zen1.1-

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
