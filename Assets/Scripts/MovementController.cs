using System.Collections;
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

    [SerializeField] private float _stopJumpingTime = 0.5f;

    [SerializeField] private float _gravityMultiplier = 10;

    private bool _canJump = true;

    private Coroutine _jumpCoroutine;

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
        rb.linearVelocity = new Vector3(_speed * moveValue.x * Time.fixedDeltaTime, rb.linearVelocity.y, rb.linearVelocity.z);

        bool isGrounded = IsGrounded();

        bool isWallDetected = IsWallDetected();

        if (jumpAction.IsPressed() && CanJump() && isGrounded)
        {
            rb.AddForce(new Vector3(0, _jumpForce * Time.fixedDeltaTime, 0), ForceMode.Force);
            //rb.linearVelocity =  new Vector3(rb.linearVelocity.x, _jumpForce * Time.fixedDeltaTime, rb.linearVelocity.z);

            if (_jumpCoroutine != null)
                StopCoroutine(_jumpCoroutine);
            _jumpCoroutine = StartCoroutine(StopJumpingFor(_stopJumpingTime));
        }

        if(!isGrounded)
        {
            // simulate gravity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y - (_gravityMultiplier * Time.fixedDeltaTime), rb.linearVelocity.z);
        }
    }

    private IEnumerator StopJumpingFor(float delay)
    {
        _canJump = false;
        yield return new WaitForSeconds(delay);
        _canJump = true;
    }

    private bool CanJump()
    {
        return _canJump;
    }

    private bool IsWallDetected()
    {
        var cols = Physics.OverlapBox(new Vector3(col.bounds.center.x + 0.25f, 0, 0), col.bounds.extents);
        foreach (var c in cols)
        {
            if (c.gameObject != gameObject)
                return true;
        }
        return false;
    }

    private bool IsGrounded()
    {
        var cols = Physics.OverlapSphere(new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius / 2);

        foreach (var c in cols)
        {
            if (c.gameObject != gameObject)
                return true;
        }

        return false;
    }
}
