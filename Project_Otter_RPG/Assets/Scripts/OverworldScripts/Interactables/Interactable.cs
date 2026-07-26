using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Events to trigger on Interact() call.")]
    public UnityEvent trigger;
    
    [SerializeField] private bool oneTimer;
    [SerializeField] private bool canInteract = true;

    [Tooltip("If this specific Interactable script has been interacted with.")]
    public bool interacted = false;

    public void Interact()
    {
        if (!canInteract)
        {
            Debug.Log("canInteract is false");
            return;
        }

        trigger.Invoke();
        interacted = true;
        if (oneTimer) canInteract = false;
    }
}
