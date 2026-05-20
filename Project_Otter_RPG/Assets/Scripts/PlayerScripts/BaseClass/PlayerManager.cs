using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    protected PlayerSystems playerManager;
    [SerializeField] private PlayableCharacterData characterData;
    [SerializeField] protected static HealthBarUI healthBar;

    private void Awake()
    {
        characterData.characterCurrentHealth = characterData.characterMaxHealth;
        healthBar = GameObject.Find("Harte_Health_Bar").GetComponent<HealthBarUI>();
        healthBar.SetHaxHealth(characterData.characterMaxHealth);
    }

    public virtual void Init(PlayerSystems system)
    {
        this.playerManager = system;
    }

    public virtual void Tick()
    {
        healthBar.SetHealth(characterData.characterCurrentHealth);
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
