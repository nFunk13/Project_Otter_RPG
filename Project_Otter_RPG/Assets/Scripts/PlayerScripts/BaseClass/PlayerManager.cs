using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    protected PlayerSystems playerManager;
    [SerializeField] private PlayableCharacterData characterData;

    private void Awake()
    {
        characterData.characterCurrentHealth = characterData.characterMaxHealth;
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

    public PlayableCharacterData GetPlayableCharacterData()
    {
        return characterData;
    }
}
