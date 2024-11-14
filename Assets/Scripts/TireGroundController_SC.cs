using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TireGroundController_SC : MonoBehaviour
{
    public TireMovement_SC tiresc;
    private Coroutine groundCheckCoroutine;

    //public Text denemeYazi;

    [Header("Tire Colliders")]
    public Collider2D tireCollider1; // İlk tekerlek collider'ı
    public Collider2D tireCollider2; // İkinci tekerlek collider'ı

    [SerializeField] private float groundCheckRadius = 0.45f; // Tekerleklerin çevresindeki kontrol yarıçapı
    [SerializeField] private float destroyTime = 5f; // Zeminle temasın olmadığı süre

    private void Awake()
    {
        tiresc = this.gameObject.GetComponent<TireMovement_SC>();
        tireCollider1 = GameObject.FindWithTag("tire1").GetComponent<CircleCollider2D>();
        tireCollider2 = GameObject.FindWithTag("tire2").GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        // Her frame tekerleklerin zeminde olup olmadığını kontrol et
        CheckGroundStatus();
    }

    private void CheckGroundStatus()
    {
        // İlk tekerleğin çevresinde Ground layer'ında obje var mı kontrol et
        bool isTire1Grounded = Physics2D.OverlapCircle(tireCollider1.transform.position, groundCheckRadius, LayerMask.GetMask("Ground")) != null;

        // İkinci tekerleğin çevresinde Ground layer'ında obje var mı kontrol et
        bool isTire2Grounded = Physics2D.OverlapCircle(tireCollider2.transform.position, groundCheckRadius, LayerMask.GetMask("Ground")) != null;

        // Eğer her iki tekerlek de zemindeyse, isGrounded true olsun
        if (isTire1Grounded || isTire2Grounded)
        {
            tiresc.isGrounded = true;
            // Eğer coroutine çalışıyorsa, durdurun
            if (groundCheckCoroutine != null)
            {
                StopCoroutine(groundCheckCoroutine);
                groundCheckCoroutine = null;
            }
        }
        else
        {
            tiresc.isGrounded = false;
            // Eğer isGrounded false ise coroutine'i başlat
            if (groundCheckCoroutine == null)
            {
                groundCheckCoroutine = StartCoroutine(CheckGroundedStatusCoroutine());
            }
        }
    }

    private IEnumerator CheckGroundedStatusCoroutine()
    {
        // Zeminle temas kontrolü için coroutine başlatıyoruz
        float elapsedTime = 0f;
        while (elapsedTime < destroyTime)
        {
            // Her 0.5 saniyede bir kontrol yapalım
            if (tiresc.isGrounded)
            {
                groundCheckCoroutine = null;
                yield break;
            }

            elapsedTime += 0.5f;
            //denemeYazi.text = elapsedTime.ToString("F1");
            yield return new WaitForSeconds(0.5f);
        }

        // Eğer 5 saniye boyunca zeminde değilsen, sahneyi yeniden yükle
        if (!tiresc.isGrounded)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        groundCheckCoroutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmos rengi ve konumu ayarlanıyor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(tireCollider1.transform.position, groundCheckRadius);
        Gizmos.DrawWireSphere(tireCollider2.transform.position, groundCheckRadius);
    }
}
