using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlatformStuckControl_SC : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject warningText; // Uyar� mesaj� i�in UI Text
    [SerializeField] private Button button1; // fren buton
    [SerializeField] private Button button2; // gas buton

    [Header("Stuck Control Script")]
    private Vector3 initialPosition;
    [SerializeField] private float stuckTime = 0f;
    [SerializeField] private bool isStuckWarningShown = false;
    [SerializeField] private bool isButtonPressed = false;

    [Header("Stuck Control Developer")]
    [SerializeField] private float stuckArea = 1f;
    [SerializeField] private float warning1 = 3f;
    [SerializeField] private float warning2 = 6f;


    void Start()
    {
        initialPosition = transform.position;
        warningText.gameObject.SetActive(false); // Ba�lang��ta uyar� gizli
        //button1.onClick.AddListener(OnButtonPressed); // fren
        button2.onClick.AddListener(OnButtonPressed); // gas
    }

    void Update()
    {
        Debug.Log(stuckTime);
        // Butonlardan birine bas�lmad�ysa i�lem yapma
        if (!isButtonPressed) return;

        // Arac�n hareket etmedi�ini kontrol et
        if (Vector3.Distance(initialPosition, transform.position) < stuckArea)
        {
            stuckTime += Time.deltaTime;

            // 5 saniyede uyar� g�ster
            if (stuckTime >= warning1 && !isStuckWarningShown)
            {
                ShowWarning("Arac�n�z s�k��t�, ilerlemiyor!");
                isStuckWarningShown = true;
            }

            // 10 saniyede sahneyi yeniden y�kle
            if (stuckTime >= warning2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            // Araba hareket etti�inde pozisyon ve zaman s�f�rlan�r
            initialPosition = transform.position;
            stuckTime = 0f;
            HideWarning();
            isStuckWarningShown = false;
            isButtonPressed = false; // ��lem bitti�inde buton durumu s�f�rlan�r
        }
    }

    // Butonlara bas�ld���nda kontrol� ba�lat
    private void OnButtonPressed()
    {
        isButtonPressed = true;
    }

    private void ShowWarning(string message)
    {
        warningText.gameObject.SetActive(true);
    }

    private void HideWarning()
    {
        warningText.gameObject.SetActive(false);
    }
}
