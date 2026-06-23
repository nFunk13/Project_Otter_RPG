using Unity.VisualScripting;
using UnityEngine;

public class SpriteBillboarder : MonoBehaviour
{
    public enum Direction
    {
        NORTH, SOUTH, EAST, WEST
    }

    [UnitHeaderInspectable("Sprites")]
    private SpriteInstance[] spriteInstances;

    [UnitHeaderInspectable("Camera")]
    [SerializeField] private Camera camera;

    private void OnEnable()
    {
        spriteInstances = GameObject.FindObjectsByType<SpriteInstance>(FindObjectsSortMode.None);
    }

    private void LateUpdate()
    {
        foreach (var instance in spriteInstances)
        {
            Vector3 facingAngle = instance.transform.forward;
            Vector3 dirToCamera = (instance.transform.position - camera.transform.position).normalized;
            float angle = Vector3.Angle(facingAngle, dirToCamera);
            
            if(angle >= 45 && angle <= 135)
            {
                instance.directionSet = Direction.EAST;
            }
            else if(angle >= 135 && angle <= 225)
            {
                instance.directionSet = Direction.SOUTH;
            }
            else if(angle >= 225 && angle <= 315)
            {
                instance.directionSet = Direction.WEST;
            }
            else if(angle >= 315 && angle <= 45)
            {
                instance.directionSet = Direction.NORTH;
            }
        }
    }
}
