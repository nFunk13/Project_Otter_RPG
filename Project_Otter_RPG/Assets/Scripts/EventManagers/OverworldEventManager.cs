using UnityEngine;
using UnityEngine.Events;
using static OverworldPlayerMovement;

public class OverworldEventManager : MonoBehaviour
{
    public static OverworldEventManager Instance { get; private set; } // singleton instance

    // events
    // nothing here lmao

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
