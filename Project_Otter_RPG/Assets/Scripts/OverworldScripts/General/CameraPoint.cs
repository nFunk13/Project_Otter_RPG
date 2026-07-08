using Unity.Cinemachine;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float pushBackDistance;
    private CinemachineCamera cam;

    private void Start()
    {
        cam = transform.GetComponentInChildren<CinemachineCamera>();
        cam.transform.position = cam.transform.position + -cam.transform.forward * pushBackDistance;
    }
}
