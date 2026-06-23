using System.Collections.Generic;
using UnityEngine;

public class BillboardManager : MonoBehaviour
{
    public static BillboardManager Instance { get; private set; }

    public enum Direction
    {
        NORTH, WEST, SOUTH, EAST
    }

    public List<SpriteInstance> spriteInstances;

    [Header("Camera")]
    [SerializeField] private Camera cam;

    [Header("Direction Settings")]
    [SerializeField] private int numOfDetectionWedges = 4;
    [SerializeField] private int sizeOfDetectionWedges = 90; //degrees

    private void Awake()
    {
        Instance = this;
        if (spriteInstances == null) spriteInstances = new List<SpriteInstance>();
    }

    private void LateUpdate()
    {
        foreach (var instance in spriteInstances)
        {
            Vector3 facingAngle = instance.transform.parent.forward;
            Vector3 dirToCamera = (cam.transform.position - instance.transform.position).normalized;
            facingAngle.y = 0;
            dirToCamera.y = 0;
            float angle = Vector3.SignedAngle(facingAngle, dirToCamera, Vector3.up);

            instance.transform.rotation = cam.transform.rotation;
            instance.directionSet = (Direction)(((Mathf.RoundToInt(angle / sizeOfDetectionWedges) % numOfDetectionWedges) + numOfDetectionWedges) % numOfDetectionWedges);
        }
    }
}
