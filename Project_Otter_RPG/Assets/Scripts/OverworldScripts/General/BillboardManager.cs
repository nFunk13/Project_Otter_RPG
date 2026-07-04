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
    public List<GameObject> additionalSprites;
    private Camera cam;

    [Header("Direction Settings")]
    public int numOfDetectionWedges = 4;

    private void Awake()
    {
        // im singletoning it so hard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (spriteInstances == null) spriteInstances = new List<SpriteInstance>();
        if (additionalSprites == null) additionalSprites = new List<GameObject>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        foreach (var instance in spriteInstances)
        {
            Vector3 facingAngle = instance.transform.parent.forward;
            Vector3 dirToCamera = (cam.transform.position - instance.transform.position).normalized;

            instance.transform.rotation = cam.transform.rotation;
            instance.CurrentDirection = (Direction)GetDirectionFromSubjectToViewer(facingAngle, dirToCamera, numOfDetectionWedges);
        }

        foreach (var sprite in additionalSprites)
        {
            sprite.transform.rotation = cam.transform.rotation;
        }
    }

    public int GetDirectionFromSubjectToViewer(Vector3 facing, Vector3 dirToViewer, int numOfDetectionWedges)
    {
        facing.y = 0; dirToViewer.y = 0;
        int sizeOfDetectionWedges = 360 / numOfDetectionWedges;
        float angle = Vector3.SignedAngle(facing, dirToViewer, Vector3.up); 
        return (Mathf.RoundToInt(angle / sizeOfDetectionWedges) % numOfDetectionWedges + numOfDetectionWedges) % numOfDetectionWedges;
    }
}
