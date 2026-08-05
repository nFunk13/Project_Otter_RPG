using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using static OverworldPlayerMovement;

public class OverworldEventManager : MonoBehaviour
{
    public static OverworldEventManager Instance { get; private set; } // singleton instance

    // events
    [HideInInspector] public UnityEvent<CinemachineCamera> onCameraSwitch;
    [HideInInspector] public UnityEvent<Interactable> onInteraction;

    void Awake()
    {
        // im singletoning it so hard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
