using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using static System.TimeZoneInfo;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Door destination;

    [Tooltip("Where the player will appear when they arrive at this door")]
    public Transform arrivalPoint;
    public Quaternion teleportRotation;

    [Tooltip("The camera to switch to when the player arrives at this door")]
    public CinemachineCamera cameraToSwitchTo;

    [Header("Transition")]
    [SerializeField] private float transitionTime;
    private CanvasGroup fadeImage;

    private void Start()
    {
        fadeImage = GameObject.Find("fadeImage").GetComponent<CanvasGroup>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (destination == null)
        {
            Debug.LogWarning("No destination assigned to this door.");
            return;
        }

        if(col.gameObject.tag == "Player")
        {
            InputEventManager.Instance.canInput = false; 
            CharacterController CC = col.gameObject.GetComponent<CharacterController>();
            if(CC != null)
            {
                StartTransition(CC);
            }
        }
    }

    public void StartTransition(CharacterController player)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append
            (fadeImage.DOFade(1, transitionTime / 2f)
                .OnComplete(() =>
                {
                    InputEventManager.Instance.canInput = false;
                    player.enabled = false;
                    player.transform.SetPositionAndRotation(destination.arrivalPoint.position, destination.teleportRotation);
                    CameraManager.Instance.SwitchCamera(destination.cameraToSwitchTo);
                }));
        sequence.AppendInterval(0.01f); // small delay to ensure camera switch is processed
        sequence.Append
            (fadeImage.DOFade(0, transitionTime / 2f)
                .OnComplete(() =>
                {
                    InputEventManager.Instance.canInput = true;
                    player.enabled = true;
                }));
        sequence
            .SetUpdate(true);
    }
}
