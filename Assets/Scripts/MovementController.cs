using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    private Rigidbody rb;
    private CapsuleCollider col;

    [SerializeField] private float _speed = 10;

    [SerializeField] private float _jumpForce = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        // Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // Read the "Move" action value, which is a 2D vector
        // and the "Jump" action state, which is a boolean value

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // Go to left or right
        rb.linearVelocity = new Vector3(_speed * moveValue.x * Time.deltaTime, rb.linearVelocity.y, rb.linearVelocity.z);

        if (jumpAction.IsPressed() && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, _jumpForce * Time.deltaTime, rb.linearVelocity.z);
        }
    }

    private bool IsGrounded()
    {
        Vector3 point1 = new Vector3(col.transform.position.x, col.transform.position.y / 4);
        Vector3 point2 = new Vector3(col.transform.position.x, col.transform.position.y / 4);
        var cols = Physics.OverlapCapsule(point1, point2, col.radius);

        if (cols == null || cols.Length == 0)
            return false;
        return true;
    }
}
