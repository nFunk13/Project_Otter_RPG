using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    protected PlayerSystems playerManager;
    protected PlayableCharacterData characterData;
    [SerializeField] private static HealthBarUI healthBar;
    protected PlayerActions playerActions;

    public enum InputKeyNames
    {
        upArrow,
        downArrow,
        rightArrow,
        leftArrow
    }

    private void Awake()
    {
        characterData = Resources.Load<PlayableCharacterData>("ScriptableObjects/PlayableCharacterData/HarteData");
        playerActions = new PlayerActions();
        characterData.characterCurrentHealth = characterData.characterMaxHealth;
        healthBar = GameObject.Find("Harte_Health_Bar").GetComponent<HealthBarUI>();
        healthBar.SetHaxHealth(characterData.characterMaxHealth);
    }

    public virtual void Init(PlayerSystems system)
    {
        this.playerManager = system;
        playerActions.Enable();
    }

    public virtual void Tick()
    {
        if (characterData != null)
        {
            healthBar.SetHealth(characterData.characterCurrentHealth);
        }
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
