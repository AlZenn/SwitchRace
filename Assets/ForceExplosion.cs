using UnityEngine;

public class ForceExplosion : MonoBehaviour
{
    public float forceAmount = 10f; // Uygulanacak kuvvetin büyüklüğü
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Rastgele bir yön oluştur ve kuvvet uygula
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDirection * forceAmount, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Rigidbody2D bulunamadı!");
        }
    }
}