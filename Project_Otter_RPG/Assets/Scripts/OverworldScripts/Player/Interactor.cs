using UnityEngine;
using static BillboardManager;

public class Interactor : MonoBehaviour
{
    private InputEventManager IEM;
    [SerializeField] float checkRadius;

    [Tooltip("Forward offset from current player position.")]
    [SerializeField] float checkDistance;
    
    [SerializeField] LayerMask layerMask;

    private void Start()
    {
        IEM = InputEventManager.Instance;
        if (IEM != null)
        {
            IEM.onInteract.AddListener(InteractableCheck);
        }
    }

    private void OnDestroy()
    {
        if (IEM != null)
        {
            IEM.onInteract.RemoveListener(InteractableCheck);
        }
    }

    [ContextMenu("InteractableCheck")]
    public void InteractableCheck()
    {
        if (checkRadius > 0)
        {
            Vector3 checkOrigin = transform.position + transform.forward * checkDistance;
            Collider[] hitColliders = Physics.OverlapSphere(checkOrigin, checkRadius, layerMask);
            
            if (hitColliders.Length <= 0)
            {
                Debug.Log("no valid interactables in check");
                return;
            }
            else
                Debug.Log(hitColliders.Length + " valid collider(s) in range");

            Collider closest = hitColliders[0];
            foreach (Collider collider in hitColliders)
            {
                if(Vector3.Distance(transform.position, collider.transform.position) < Vector3.Distance(transform.position, closest.transform.position))
                    closest = collider;
            }

            if (closest.GetComponentInParent<Interactable>() is Interactable interactable)
                interactable.Interact();
            else
                Debug.LogWarning($"{closest.name} is on the interactable layer but has no Interactable component");
        }
        else
            Debug.LogWarning("interact radius has to be larger than zero");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 checkOrigin = transform.position + transform.forward * checkDistance;
        Gizmos.DrawWireSphere(checkOrigin, checkRadius);
    }
}
