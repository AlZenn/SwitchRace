using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SC : MonoBehaviour
{
    [Header("Hareket Tuzagi Properties")]
    [SerializeField] private Transform target1; // Birinci hedef
    [SerializeField] private Transform target2; // Ýkinci hedef
    [SerializeField] private float speed = 5f;  // Hareket hýzý 

    private Transform currentTarget;     // Þu anki hedef
    private bool reachedTarget1 = false; // Ýlk hedefe ulaþýp ulaþmadýðýný kontrol eder

    void Start()
    {
        currentTarget = target1; // Baþlangýçta ilk hedefi ayarla
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        // GameObject'i currentTarget'e doðru hareket ettir
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Hedefe ulaþýp ulaþmadýðýný kontrol et
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (!reachedTarget1)
            {
                // Ýlk hedefe ulaþýldýysa, ikinci hedefe geç
                currentTarget = target2;
                reachedTarget1 = true;
            }
            else
            {
                // Ýkinci hedefe ulaþýldýysa, ilk hedefe geri dön
                currentTarget = target1;
                reachedTarget1 = false;
            }
        }
    }


}
