using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting; // deneme kütüphane
using UnityEngine;
//using UnityEngine.PlayerLoop; // deneme kütüphane
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarTopController_SC : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    //[SerializeField] private Collider2D _tavanCollider;
    // animator eklenebilir public death animation için

    [SerializeField] private float _contactTime = 0f; // temas süresi default 0
    [SerializeField] private float _tavanLifeTime = 2f; // temas süresi
    [SerializeField] private bool isContact = false; // temas ediyor mu

    [SerializeField] private GameObject denemeYazi;                    // deneme
    [SerializeField] private Text denemeYaziText;                    // deneme

    private void Awake()
    {
        denemeYazi.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // tavan "ground" layeri ile temas ederse kodu
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isContact = true;
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        // tavan "ground" layeri ile temas etmiyorsa
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isContact = false;
            _contactTime = 0f;
            denemeYazi.SetActive(false);                      // deneme
        } 
    }

    void Update()
    {
        if (isContact) // eðer temas varsa saniye saysýn
        {
            _contactTime += Time.deltaTime; // süre arttýr
            denemeYaziText.text = _contactTime.ToString("F2") + " uwu";                     // deneme
            denemeYazi.SetActive(true);                     // deneme

            if (_contactTime >= _tavanLifeTime)
            {
                StartCoroutine(PlayerDeath());
            }

        }
    }

    private IEnumerator PlayerDeath()
    {
        #region Animasyon kodu 
        //animator için gerekli kod
        /*
        if (animator != null)
        {
            animator.SetTrigger("Death");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
        animasyon süresi kadar bekle
        }
         */
        #endregion

        //playeri yok et ve sahneyi tekrar yükle
        //Destroy(_player);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        yield return null; // cooroutine hatasýný engellemek için.
    }

}
