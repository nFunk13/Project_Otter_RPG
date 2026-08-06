using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    protected PlayerSystems playerManager;
    protected PlayableCharacterData characterData;
    [SerializeField] private static HealthBarUI healthBar;
    protected PlayerActions playerActions;
    private CombatState combatState = CombatState.IDLE_COMBAT;

    protected static SpriteInstance spriteInstance;

    public enum CombatState
    {
        IDLE_COMBAT, THINKING_COMBAT
    }

    public enum InputKeyNames
    {
        upArrow,
        downArrow,
        rightArrow,
        leftArrow
    }

    private void Awake()
    {
        spriteInstance = gameObject.GetComponent<SpriteInstance>();
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
        Debug.Log("PLAYER ACTION MOVEMENT ENABLED: " + playerActions.Combat.AddTileMovement.enabled);
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

    public PlayerActions GetPlayerActions()
    {
        return playerActions;
    }

    public void SetCombatState(CombatState newCombatState)
    {
        if (combatState == newCombatState)
        {
            return;
        }
        //spriteInstance.Stop(combatState.ToString().ToLower());
        combatState = newCombatState;
        if (spriteInstance.currentAnim.looping && !string.IsNullOrEmpty(spriteInstance.currentAnim.animOnEnd))
        {
            spriteInstance.Stop(this.gameObject, combatState.ToString().ToLower());
        }
        else
        {
            spriteInstance.Play(combatState.ToString().ToLower());
        }
    }

    public CombatState GetCombatState()
    {
        return combatState;
    }
}
