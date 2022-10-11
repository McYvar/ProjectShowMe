using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementBuildupStrenght;
    [SerializeField] private float maxMovementVelocityThreshold;
    [SerializeField] private float movementCounterStrength;

    [SerializeField, Range(0, 10)] private float jumpStrenght;
    [SerializeField] private float inAirMovementStrenghtReduction;
    private bool isGrounded;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private PlayerInput playerInput;

    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        ForceMovement();
    }

    private void Update()
    {
        isGrounded = GroundCheck();
    }

    private void ForceMovement()
    {
        float inAirMovementReduction = 1;
        if (!isGrounded)
        {
            inAirMovementReduction = inAirMovementStrenghtReduction;
        }
        rb.AddForce(new Vector3(movementInput.x * movementBuildupStrenght * inAirMovementReduction, 0, movementInput.y * movementBuildupStrenght * inAirMovementReduction), ForceMode.VelocityChange);

        Vector2 horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);
        if (horizontalMovement.magnitude > maxMovementVelocityThreshold)
        {
            horizontalMovement *= -movementCounterStrength * inAirMovementReduction;
            rb.AddForce(horizontalMovement.x, 0, horizontalMovement.y);
        }
    }

    private Vector3 GetColliderBottom()
    {
        return transform.position + (Vector3.down * capsuleCollider.height / 4);
    }

    private RaycastHit GetBottomHitPosition()
    {
        RaycastHit hit;
        float maxRayDistance = 0.2f;
        Physics.SphereCast(GetColliderBottom() + Vector3.up * 0.1f, capsuleCollider.radius, Vector3.down, out hit, maxRayDistance);
        return hit;
    }

    private bool GroundCheck()
    {
        RaycastHit hit = GetBottomHitPosition();
        if (hit.collider == null) return false;
        
        PlayerController controller = hit.collider.GetComponent<PlayerController>();
        if (controller == null) return true;
        
        return false;
    }

    #region InputActions
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpStrenght, ForceMode.VelocityChange);
        }
    }
    #endregion
}
