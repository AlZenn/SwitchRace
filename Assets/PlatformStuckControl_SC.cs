using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlatformStuckControl_SC : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private Text warningText; // Uyarý mesajý için UI Text
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
        warningText.gameObject.SetActive(false); // Baþlangýçta uyarý gizli
        //button1.onClick.AddListener(OnButtonPressed); // fren
        button2.onClick.AddListener(OnButtonPressed); // gas
    }

    void Update()
    {
        Debug.Log(stuckTime);
        // Butonlardan birine basýlmadýysa iþlem yapma
        if (!isButtonPressed) return;

        // Aracýn hareket etmediðini kontrol et
        if (Vector3.Distance(initialPosition, transform.position) < stuckArea)
        {
            stuckTime += Time.deltaTime;

            // 5 saniyede uyarý göster
            if (stuckTime >= warning1 && !isStuckWarningShown)
            {
                ShowWarning("Aracýnýz sýkýþtý, ilerlemiyor!");
                isStuckWarningShown = true;
            }

            // 10 saniyede sahneyi yeniden yükle
            if (stuckTime >= warning2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            // Araba hareket ettiðinde pozisyon ve zaman sýfýrlanýr
            initialPosition = transform.position;
            stuckTime = 0f;
            HideWarning();
            isStuckWarningShown = false;
            isButtonPressed = false; // Ýþlem bittiðinde buton durumu sýfýrlanýr
        }
    }

    // Butonlara basýldýðýnda kontrolü baþlat
    private void OnButtonPressed()
    {
        isButtonPressed = true;
    }

    private void ShowWarning(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true);
    }

    private void HideWarning()
    {
        warningText.gameObject.SetActive(false);
    }
}
