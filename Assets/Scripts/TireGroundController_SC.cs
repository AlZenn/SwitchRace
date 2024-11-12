using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TireGroundController_SC : MonoBehaviour
{
    public TireMovement_SC tiresc;
    private Coroutine groundCheckCoroutine;
    [SerializeField] private float destoryTime = 5f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            tiresc.isGrounded = true;

            // Daha önce çalýþan bir coroutine varsa onu iptal ediyoruz
            if (groundCheckCoroutine != null)
            {
                StopCoroutine(groundCheckCoroutine);
                groundCheckCoroutine = null;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Coroutine baþlatýyoruz ve 0.5 saniye bekliyoruz
            if (groundCheckCoroutine == null)
            {
                groundCheckCoroutine = StartCoroutine(CheckGroundedStatus());
            }
        }
    }

    private IEnumerator CheckGroundedStatus()
    {
        // 7 saniye boyunca isGrounded durumunu kontrol eder
        float elapsedTime = 0f;
        while (elapsedTime < destoryTime)
        {
            Debug.Log(elapsedTime);
            if (Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Ground")))
            {
                // Eðer zeminle temas saðlanýrsa isGrounded'i true yap ve coroutine'i sonlandýr
                tiresc.isGrounded = true;
                groundCheckCoroutine = null;
                yield break;
            }

            // Eðer hala zeminle temas yoksa isGrounded'i false olarak tut ve süreyi arttýr
            tiresc.isGrounded = false;
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        // 7 saniye boyunca isGrounded false kaldýysa, sahneyi yeniden yükle
        if (!tiresc.isGrounded)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        groundCheckCoroutine = null;
    }
}
