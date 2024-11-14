using UnityEngine;
using UnityEngine.UI;

public class CarMaterialController : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public AudioSource audioSource;
    public AudioClip soundClip;
    
    public Renderer[] renderers; // Arabanın Renderer bileşeni (Materyal için)
    public Button redButton;     // Kırmızı buton
    public Button blueButton;    // Mavi buton
    public Button greenButton;   // Yeşil buton
    public Button yellowButton;  // Sarı buton

    public Material redMaterial;     // Kırmızı materyal
    public Material blueMaterial;    // Mavi materyal
    public Material greenMaterial;   // Yeşil materyal
    public Material yellowMaterial;  // Sarı materyal
    public Material whiteMaterial;   // Beyaz başlangıç materyali

    public Material currentMat; // Şu anki materyal

    private void Awake()
    {
        currentMat = whiteMaterial; // Başlangıç materyali
    }

    void Start()
    {
        // Buton tıklamaları için olay dinleyicileri ekliyoruz
        redButton.onClick.AddListener(() => ChangeCarMaterial(redMaterial));    // Kırmızı materyal
        blueButton.onClick.AddListener(() => ChangeCarMaterial(blueMaterial));  // Mavi materyal
        greenButton.onClick.AddListener(() => ChangeCarMaterial(greenMaterial)); // Yeşil materyal
        yellowButton.onClick.AddListener(() => ChangeCarMaterial(yellowMaterial)); // Sarı materyal
    }

    void Update()
    {
        // Yön tuşları ile renk değişimi
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeCarMaterial(greenMaterial); // Yukarı ok tuşu ile yeşil
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeCarMaterial(yellowMaterial); // Sağ ok tuşu ile sarı
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeCarMaterial(redMaterial); // Sol ok tuşu ile kırmızı
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeCarMaterial(blueMaterial); // Aşağı ok tuşu ile mavi
        }
    }

    // Arabanın materyalini değiştiren fonksiyon
    void ChangeCarMaterial(Material newMaterial)
    {
        particleSystem.Play();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(soundClip);
        foreach (Renderer renderer in renderers)
        {
            renderer.material = newMaterial;
            currentMat = newMaterial; // Şu anki materyali güncelle
        }
    }
}
