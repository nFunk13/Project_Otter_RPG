using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using Sequence = DG.Tweening.Sequence;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Header("Misc")]
    [SerializeField] private CinemachineCamera harteCam; // the camera that follows the player
    [SerializeField] private int basePriority = 10; 
    [SerializeField] private int activePriority = 20;

    [SerializeField] private CinemachineCamera currentCam;
    private bool isTransitioning;

    private void Awake()
    {
        // im singletoning it so hard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if(currentCam == null)
        {
            currentCam = harteCam;
        }
    }

    public void SwitchCamera(CinemachineCamera newCam)
    {
        if (isTransitioning) return; // prevent switching cameras while transitioning
        if (currentCam != null)
        {
            currentCam.Priority = basePriority;
        }
        newCam.Priority = activePriority;

        Debug.Log("Switching camera to: " + newCam.name + " From " + currentCam.name);

        currentCam = newCam;

        OverworldEventManager.Instance.onCameraSwitch.Invoke(newCam);
    }
}
