using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow_SC : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform target;           // cinemachine target, araç atanacak

    [Header("Camera Properties")]
    [SerializeField] private float followDistance = 10f; // kamera uzaklýgý
    [SerializeField] private float followHeight = 5f;    // kemara yükseklik ayarý
    [SerializeField] private float cameraZoom = 5f;      // kamera yakýnlýgý
    [SerializeField] private float dampTime = 5f;        // kamera takip hýzý

    private CinemachineTransposer transposer;
    private CinemachineFramingTransposer framingTransposer;

    private void Start()
    {
        if (virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0,followHeight,-followDistance);
        }
    }

    private void Update()
    {
        if (transposer != null && target != null)
        {
            transposer.m_FollowOffset = new Vector3(0, followHeight, -followDistance);
            virtualCamera.m_Lens.OrthographicSize = cameraZoom;
        }

        if (framingTransposer != null)
        {
            framingTransposer.m_XDamping = dampTime;
            framingTransposer.m_YDamping = dampTime;
        }
    }
}
