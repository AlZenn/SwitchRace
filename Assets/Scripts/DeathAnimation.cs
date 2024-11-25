using System.Collections;
using UnityEngine;
using Cinemachine;

public class DeathAnimation : MonoBehaviour
{
    public GameObject explosionPrefab;       // Assign your Explosion prefab here
    public Transform explosionPosition;      // Position where the explosion should spawn
    public CinemachineVirtualCamera virtualCamera;
    public AudioClip DeathSFX;
    private AudioManager audioManager;// Reference to the Cinemachine camera
    public float restartTime = 2f;
    public float zoomDuration = 1.0f;        // Duration for the zoom effect
    public float targetOrthographicSize = 3f; // Target orthographic size for the zoom-in effect
    private float initialOrthographicSize;
    private GameObject canvasGroup;
    public GameObject Player;
    private bool isDeathHandled = false; // Yeni bir bayrak ekleyin


    private void Start()
    {
        initialOrthographicSize = virtualCamera.m_Lens.OrthographicSize; // Store initial orthographic size
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        canvasGroup = GameObject.Find("GameplayHuds");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(HandleDeath());
        }
    }

    public void TriggerDeathAnimation()
    {
        if (isDeathHandled) return; // Eğer zaten tetiklendiyse, çık
        isDeathHandled = true; // İlk kez tetiklendiği anda bayrağı işaretle
        StartCoroutine(HandleDeath());
    }
    private IEnumerator HandleDeath()
    {
        // Deactivate the car
        Player.SetActive(false);
        
        // Spawn the explosion prefab at the car's position
        GameObject explosion = Instantiate(explosionPrefab, explosionPosition.position, Quaternion.identity);
        audioManager.PlaySFX(DeathSFX);
        
        if (canvasGroup!=null)
        {
            canvasGroup.GetComponent<CanvasGroup>().alpha = 0f;
        }

        // Set the Cinemachine target to the explosion
        virtualCamera.Follow = explosion.transform;

        // Start camera shake
        CameraShake();

        // Start zoom-in effect
        StartCoroutine(ZoomIn());

        // Wait for 2 seconds before restarting
        yield return new WaitForSeconds(restartTime);

        // Reload the scene (or reset the level)
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    private void CameraShake()
    {
        // Ensure the noise component is added to the virtual camera
        CinemachineImpulseSource source = virtualCamera.gameObject.GetComponent<CinemachineImpulseSource>();
        source.GenerateImpulseWithForce(1f);
    }

    private IEnumerator ZoomIn()
    {
        Debug.Log("Starting Zoom");

        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            Debug.Log("Current Orthographic Size: " + virtualCamera.m_Lens.OrthographicSize);
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetOrthographicSize;
        Debug.Log("Zoom Complete");
    }
}
