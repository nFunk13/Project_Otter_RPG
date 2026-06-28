using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    protected PlayerSystems playerManager;
    [SerializeField] private PlayableCharacterData characterData;
    [SerializeField] protected static HealthBarUI healthBar;
    protected PlayerActions playerActions;

    private void Awake()
    {
        playerActions = new PlayerActions();
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
        Debug.Log("Harte Health: " + characterData.characterCurrentHealth);
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
