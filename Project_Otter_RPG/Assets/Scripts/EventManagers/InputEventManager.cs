using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputEventManager : MonoBehaviour
{
    public static InputEventManager Instance { get; private set; } // singleton instance

    [Header("Keyboard")]
    [SerializeField] private InputAction verticalInput;
    [SerializeField] private InputAction horizontalInput;

    // events
    [HideInInspector] public UnityEvent<float> onVerticalInputChanged;
    [HideInInspector] public UnityEvent<float> onHorizontalInputChanged;

    public bool canInput = true;

    void Awake()
    {
        // im singletoning it so hard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // bind input actions to events
        verticalInput.started += ctx => onVerticalInputChanged.Invoke(verticalInput.ReadValue<float>());
        horizontalInput.started += ctx => onHorizontalInputChanged.Invoke(horizontalInput.ReadValue<float>());
        verticalInput.canceled += ctx => onVerticalInputChanged.Invoke(verticalInput.ReadValue<float>());
        horizontalInput.canceled += ctx => onHorizontalInputChanged.Invoke(horizontalInput.ReadValue<float>());
    }

    private void OnEnable()
    {
        verticalInput.Enable();
        horizontalInput.Enable();
    }

    private void OnDisable()
    {
        verticalInput.Disable();
        horizontalInput.Disable();
    }
}
