using UnityEngine;

public class FollowParent : MonoBehaviour
{
    [SerializeField] private GameObject parent;

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.transform.position;
    }
}
