using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static BillboardManager;

public class Interactor : MonoBehaviour
{
    [SerializeField] SpriteInstance SI;
    [SerializeField] float checkRadius;

    [Tooltip("Forward offset from current player position.")]
    [SerializeField] float checkDistance;
    
    [SerializeField] LayerMask layerMask;

    private void Start()
    {
        //subscribe to interact input event
        SI = gameObject.GetComponentInChildren<SpriteInstance>();
    }

    [ContextMenu("InteractableCheck")]
    public void InteractableCheck()
    {
        if (checkRadius > 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + (SpriteDirectionToVector3Direction(SI.CurrentDirection) * checkDistance), checkRadius, layerMask);
            
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

            //invoke interact event passing in closest
        }
        else
            Debug.LogWarning("interact radius has to be larger than zero");
    }

    private Vector3 SpriteDirectionToVector3Direction(Direction dir)
    {
        switch (dir)
        {
            case Direction.NORTH: 
                return Vector3.forward;
            case Direction.SOUTH: 
                return Vector3.back;
            case Direction.EAST: 
                return Vector3.right;
            case Direction.WEST: 
                return Vector3.left;
            default: 
                return Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (SpriteDirectionToVector3Direction(SI.CurrentDirection) * checkDistance), checkRadius);
    }
}
