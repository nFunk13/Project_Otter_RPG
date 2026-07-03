using System.Collections.Generic;
using UnityEngine;

public class BillboardManager : MonoBehaviour
{
    public static BillboardManager Instance { get; private set; }

    public enum Direction
    {
        SOUTH, EAST, NORTH, WEST
    }

    public List<SpriteInstance> spriteInstances;
    public List<GameObject> silhouettes;
    private Camera cam;

    [Header("Direction Settings")]
    public int numOfDetectionWedges = 4;
    public int sizeOfDetectionWedges = 90; //degrees

    private void Awake()
    {
        Instance = this;
        if (spriteInstances == null) spriteInstances = new List<SpriteInstance>();
        if (silhouettes == null) silhouettes = new List<GameObject>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
            instance.CurrentDirection = (Direction)(((Mathf.RoundToInt(angle / sizeOfDetectionWedges) % numOfDetectionWedges) + numOfDetectionWedges) % numOfDetectionWedges);
        }

        foreach (var silhouette in silhouettes)
        {
            silhouette.transform.rotation = cam.transform.rotation;
        }
    }
}
