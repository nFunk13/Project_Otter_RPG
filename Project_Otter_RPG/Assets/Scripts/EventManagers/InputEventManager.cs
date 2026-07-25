using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputEventManager : MonoBehaviour
{
    public static InputEventManager Instance { get; private set; } // singleton instance
    private InputActionMap actionMap;

    [Header("Actions")]
    [SerializeField] private InputActionReference verticalInput;
    [SerializeField] private InputActionReference horizontalInput;
    [SerializeField] private InputActionReference interactInput;

    // events
    [HideInInspector] public UnityEvent<float> onVerticalInput;
    [HideInInspector] public UnityEvent<float> onHorizontalInput;
    [HideInInspector] public UnityEvent onInteract;

    private bool _canInput = true;
    public bool CanInput
    {
        get { return _canInput; }
        set
        {
            _canInput = value;
            if (_canInput) actionMap.Enable();
            else actionMap.Disable();
        }
    }

    void Awake()
    {
        // im singletoning it so hard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        actionMap = InputSystem.actions.FindActionMap("Overworld", throwIfNotFound: true);
    }

    private void HandleActions(bool bind)
    {
        if (bind)
        {
            horizontalInput.action.performed += HorizontalCTX;
            horizontalInput.action.canceled += HorizontalCTX;

            verticalInput.action.performed += VerticalCTX;
            verticalInput.action.canceled += VerticalCTX;

            interactInput.action.performed += InteractCTX;
        }
        else
        {
            horizontalInput.action.performed -= HorizontalCTX;
            horizontalInput.action.canceled -= HorizontalCTX;

            verticalInput.action.performed -= VerticalCTX;
            verticalInput.action.canceled -= VerticalCTX;

            interactInput.action.performed -= InteractCTX;
        }
    }

    private void HorizontalCTX(InputAction.CallbackContext ctx) => onHorizontalInput.Invoke(ctx.ReadValue<float>());
    private void VerticalCTX(InputAction.CallbackContext ctx) => onVerticalInput.Invoke(ctx.ReadValue<float>());
    private void InteractCTX(InputAction.CallbackContext ctx) => onInteract.Invoke();

    private void OnEnable()
    {
        HandleActions(true);
    }

    private void OnDisable()
    {
        HandleActions(false);
    }
}
