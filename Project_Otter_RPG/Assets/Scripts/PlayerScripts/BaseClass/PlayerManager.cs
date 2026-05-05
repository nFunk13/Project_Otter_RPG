using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    protected PlayerSystems playerManager;

    private void Awake()
    {
        
    }

    public virtual void Init(PlayerSystems system)
    {
        this.playerManager = system;
    }

    public virtual void Tick()
    {

    }

    public virtual void FixedTick()
    {

    }

    public virtual void LateTick()
    {

    }
}
