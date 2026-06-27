using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class OverworldPlayerMovement : MonoBehaviour
{
    private InputEventManager IEM;
    private Camera cam;

    // values
    private float verticalInputRaw;  
    private float horizontalInputRaw;
    private CharacterController characterController;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;

    private void OnEnable()
    {
        IEM = InputEventManager.Instance;
        if(IEM != null)
        {
            IEM.onVerticalInputChanged.AddListener(ReadVerticalInput);
            IEM.onHorizontalInputChanged.AddListener(ReadHorizontalInput);
        }

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
            if (verticalInputRaw != 0)
            {
                Vector3 faceDir = verticalInputRaw > 0 ? camForward : -camForward;
                transform.rotation = Quaternion.LookRotation(faceDir);
            }
            else if (horizontalInputRaw != 0)
            {
                Vector3 faceDir = horizontalInputRaw > 0 ? camRight : -camRight;
                transform.rotation = Quaternion.LookRotation(faceDir);
            }
        }
        characterController.Move(moveSpeed * Time.deltaTime * moveDir);
    }

    private void Update()
    {
        Movement();
    }
}
