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
    private float _originalSpeed;

    [SerializeField] private float _jumpForce = 10;

    [SerializeField] private float _stopJumpingTime = 0.5f;

    [SerializeField] private float _gravityMultiplier = 10;
    private float _originalGravityMultiplier;

    [SerializeField] private Animator _animator;

    [Header("Slow motion options")]
    [SerializeField] private float _slowMotionSpeed = 200;
    [SerializeField] private float _slowMotionGravityMultiplier = 0.1f;

    private Vector2 _moveValue;

    private bool _canJump = true;

    [SerializeField] private GameObject _artObject;

    private Coroutine _jumpCoroutine;

    private bool _isSlowMotion = false;

    private bool _isEnabled = true;

    public bool EnableCompletely = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        _originalSpeed = _speed;
        _originalGravityMultiplier = _gravityMultiplier;
    }

    private void Start()
    {
        // Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        if (!EnableCompletely)
            return;

        if (_isEnabled)
        {
            // Read the "Move" action value, which is a 2D vector
            // and the "Jump" action state, which is a boolean value

            _moveValue = moveAction.ReadValue<Vector2>();
        } else
        {
            // If the player is not enabled, we should not move or jump
            _moveValue = Vector2.zero;
        }

        if (_moveValue.x > 0)
            LookRight();
        else if (_moveValue.x < 0)
            LookLeft();

        bool isWallDetected = IsWallDetected();

        // if the detected wall is on the right side, we should not move to the right
        if (_moveValue.x > 0 && isWallDetected && IsLookingRight())
            _moveValue = new Vector2(0, _moveValue.y);

        if(_moveValue.x < 0 && isWallDetected && !IsLookingRight())
            _moveValue = new Vector2(0, _moveValue.y);

        
        bool isGrounded = IsGrounded();

        if (jumpAction.IsPressed() && CanJump() && isGrounded)
        {
            rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
            //rb.linearVelocity =  new Vector3(rb.linearVelocity.x, _jumpForce * Time.fixedDeltaTime, rb.linearVelocity.z);

            if (_jumpCoroutine != null)
                StopCoroutine(_jumpCoroutine);
            _jumpCoroutine = StartCoroutine(StopJumpingFor(_stopJumpingTime));
        }

        if(!isGrounded)
        {
            // simulate gravity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y - (_gravityMultiplier * Time.deltaTime), rb.linearVelocity.z);
        }

        _animator.SetInteger("Speed", Mathf.Abs((int)_moveValue.x));
        _animator.SetBool("IsGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        if (!EnableCompletely)
            return;

        // Go to left or right
        rb.linearVelocity = new Vector3(_speed * _moveValue.x, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    public void LookRight()
    {
        _artObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void LookLeft()
    {
        _artObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private bool IsLookingRight()
    {
        return _artObject.transform.rotation.y == 0;
    }

    private IEnumerator StopJumpingFor(float delay)
    {
        _canJump = false;
        yield return new WaitForSeconds(delay);
        _canJump = true;
    }

    private bool CanJump()
    {
        return _canJump && _isEnabled && !_isSlowMotion;
    }

    private bool IsWallDetected()
    {
        // overlap 0.25f to the right of the player and make it less than the half of the collider size
        if (IsLookingRight())
        {
            var cols = Physics.OverlapBox(new Vector3(col.bounds.max.x + 0.25f, col.bounds.center.y, col.bounds.center.z), new Vector3(0.01f, col.bounds.size.y / 2 - 0.1f, col.bounds.size.z / 2 - 0.1f));
            foreach (var c in cols)
            {
                if (c.gameObject != gameObject && !c.gameObject.tag.Equals("Dialog") && !c.gameObject.tag.Equals("EndLevel"))
                    return true;
            }
        }
        else
        {
            var cols = Physics.OverlapBox(new Vector3(col.bounds.min.x - 0.25f, col.bounds.center.y, col.bounds.center.z), new Vector3(0.01f, col.bounds.size.y / 2 - 0.1f, col.bounds.size.z / 2 - 0.1f));
            foreach (var c in cols)
            {
                if (c.gameObject != gameObject && !c.gameObject.tag.Equals("Dialog"))
                    return true;
            }
        }
        return false;
    }

    private bool IsGrounded()
    {
        var cols = Physics.OverlapSphere(new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius / 2);

        foreach (var c in cols)
        {
            if (c.gameObject != gameObject && !c.gameObject.tag.Equals("Dialog") && !c.gameObject.tag.Equals("EndLevel"))
                return true;
        }

        return false;
    }

    public void SetEnabled(bool enabled)
    {
        _isEnabled = enabled;
    }

    public void SetSlowMotion(bool slowMotion)
    {
        _isSlowMotion = slowMotion;
        if (slowMotion)
        {
            rb.isKinematic = true;
            _speed = _slowMotionSpeed;
            _gravityMultiplier = _slowMotionGravityMultiplier;
        }
        else
        {
            rb.isKinematic = false;
            _speed = _originalSpeed;
            _gravityMultiplier = _originalGravityMultiplier;
        }
    }
}
