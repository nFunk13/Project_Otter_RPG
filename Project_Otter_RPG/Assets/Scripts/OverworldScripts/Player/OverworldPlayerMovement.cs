using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class OverworldPlayerMovement : MonoBehaviour
{
    private InputEventManager IEM;
    private OverworldEventManager OEM;
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
    [SerializeField] [Tooltip("For position snapping.")] private float pixelsPerUnit;

    private void OnEnable()
    {
        IEM = InputEventManager.Instance;
        if (IEM != null)
        {
            IEM.onVerticalInputChanged.AddListener(ReadVerticalInput);
            IEM.onHorizontalInputChanged.AddListener(ReadHorizontalInput);
        }

        OEM = OverworldEventManager.Instance;
        SetPlayerState(PlayerState.IDLE);

        characterController = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void OnDisable() // i stg if i forget to remove listeners properly again in this project im gonna explode
    {
        if (IEM != null)
        {
            IEM.onVerticalInputChanged.RemoveListener(ReadVerticalInput);
            IEM.onHorizontalInputChanged.RemoveListener(ReadHorizontalInput);
        }
    }

    void ReadVerticalInput(float value) { verticalInputRaw = value; }
    void ReadHorizontalInput(float value) { horizontalInputRaw = value; }

    private void Movement()
    {
        Vector3 camForward = cam.transform.forward; camForward.y = 0; camForward.Normalize();
        Vector3 camRight = cam.transform.right; camRight.y = 0; camRight.Normalize();
        Vector3 moveDir = horizontalInputRaw * camRight + verticalInputRaw * camForward;
        moveDir.Normalize();

        if (moveDir != Vector3.zero)
        {
            SetPlayerState(PlayerState.WALK);
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
        else
        {
            SetPlayerState(PlayerState.IDLE);
        }

        characterController.Move(moveSpeed * Time.deltaTime * moveDir);
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
