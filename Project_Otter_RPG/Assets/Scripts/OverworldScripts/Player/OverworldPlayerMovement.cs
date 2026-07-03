using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class OverworldPlayerMovement : MonoBehaviour
{
    private InputEventManager IEM;
    private Camera cam;

    public enum PlayerState
    {
        IDLE, WALK
    }

    // values
    private float verticalInputRaw;
    private float horizontalInputRaw;
    private CharacterController characterController;
    private bool commitedToVertical;

    [Header("References")]
    [SerializeField] private SpriteInstance SI;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerState playerState;

    [Header("Invalid Direction Check Settings")]
    [Tooltip("Set this to be about the Controller's capsule radius, plus a small margin (~0.4 - 0.6f).")]
    [SerializeField] private float invalidDirectionCheckMagnitude;
    [SerializeField] private float startingRaycastHeight;
    [SerializeField] private float raycastLength;

    [Header("Gravity")]
    [SerializeField] private float gravity = 20f;

    [Tooltip("Set this to be a small negative so Harte \"sticks\" to the ground. Leaving it at -2 is fine.")]
    [SerializeField] private float groundedVelocity = -2f;

    private float verticalVelocity;

    private void OnEnable()
    {
        IEM = InputEventManager.Instance;
        if (IEM != null)
        {
            IEM.onVerticalInputChanged.AddListener(ReadVerticalInput);
            IEM.onHorizontalInputChanged.AddListener(ReadHorizontalInput);
        }

        SetPlayerState(PlayerState.IDLE);
        cam = Camera.main;
    }

    private void OnDisable() // i stg if i forget to remove listeners properly again in this project im gonna explode
    {
        if (IEM != null)
        {
            IEM.onVerticalInputChanged.RemoveListener(ReadVerticalInput);
            IEM.onHorizontalInputChanged.RemoveListener(ReadHorizontalInput);
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void ReadVerticalInput(float value) { verticalInputRaw = value; }
    void ReadHorizontalInput(float value) { horizontalInputRaw = value; }

    private void Movement()
    {
        Vector3 camForward = cam.transform.forward; camForward.y = 0; camForward.Normalize();
        Vector3 camRight = cam.transform.right; camRight.y = 0; camRight.Normalize();
        Vector3 horizontalDir = horizontalInputRaw * camRight + verticalInputRaw * camForward;
        horizontalDir.Normalize();

        if (horizontalDir != Vector3.zero)
        {
            bool horizontalHeld = horizontalInputRaw != 0;
            bool verticalHeld = verticalInputRaw != 0;

            if (commitedToVertical && verticalHeld)
            {
                Vector3 faceDir = verticalInputRaw > 0 ? camForward : -camForward;
                transform.rotation = Quaternion.LookRotation(faceDir);
            }
            else if (!commitedToVertical && horizontalHeld)
            {
                Vector3 faceDir = horizontalInputRaw > 0 ? camRight : -camRight;
                transform.rotation = Quaternion.LookRotation(faceDir);
            }
            else
            {
                if (verticalHeld && horizontalHeld) commitedToVertical = true;
                else if (verticalHeld) commitedToVertical = true;
                else if (horizontalHeld) commitedToVertical = false;
            }
        }

        // check if the player is trying to move into an invalid direction (maybe into a ledge or into surfaces like water)
        bool negXCheck = Physics.Raycast(transform.position + Vector3.left * invalidDirectionCheckMagnitude + Vector3.up * startingRaycastHeight, Vector3.down, raycastLength);
        bool posXCheck = Physics.Raycast(transform.position + Vector3.right * invalidDirectionCheckMagnitude + Vector3.up * startingRaycastHeight, Vector3.down, raycastLength);
        bool negZCheck = Physics.Raycast(transform.position + Vector3.back * invalidDirectionCheckMagnitude + Vector3.up * startingRaycastHeight, Vector3.down, raycastLength);
        bool posZCheck = Physics.Raycast(transform.position + Vector3.forward * invalidDirectionCheckMagnitude + Vector3.up * startingRaycastHeight, Vector3.down, raycastLength);

        if (!posXCheck && horizontalDir.x > 0) horizontalDir.x = 0;
        if (!negXCheck && horizontalDir.x < 0) horizontalDir.x = 0;
        if (!posZCheck && horizontalDir.z > 0) horizontalDir.z = 0;
        if (!negZCheck && horizontalDir.z < 0) horizontalDir.z = 0;

        if (horizontalDir != Vector3.zero)
        {
            if(!Physics.Raycast(transform.position + horizontalDir.normalized * invalidDirectionCheckMagnitude + Vector3.up * startingRaycastHeight, Vector3.down, out RaycastHit surfaceHit, raycastLength))
            {
                horizontalDir = Vector3.zero;
                // possibly play bump animation
            }
        }

        verticalVelocity = characterController.isGrounded ? groundedVelocity : verticalVelocity - gravity * Time.deltaTime;
        characterController.Move((horizontalDir * moveSpeed + Vector3.up * verticalVelocity) * Time.deltaTime);
        SetPlayerState(horizontalDir != Vector3.zero ? PlayerState.WALK : PlayerState.IDLE);
    }

    private void SetPlayerState(PlayerState state)
    {
        if (state == playerState) return;
        playerState = state;
        SI.Play(playerState.ToString().ToLower());
    }

    private void Update()
    {
        Movement();
    }
}
