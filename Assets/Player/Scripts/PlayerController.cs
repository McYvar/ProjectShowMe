using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public bool hasSpawned;

    [SerializeField] private float movementBuildupStrenght;
    [SerializeField] private float maxMovementVelocityThreshold;
    [SerializeField] private float movementCounterStrength;

    [SerializeField, Range(0, 10)] private float jumpStrenght;
    [SerializeField] private float inAirMovementStrenghtReduction;
    private bool isGrounded;

    private Rigidbody rb;
    [SerializeField] private Vector3 myGravityDirection;
    [SerializeField, Range(0, 1)] private float myGravityRotationSpeed;
    [SerializeField, Range(-20, 20)] private float myGravityStrength;

    private CapsuleCollider capsuleCollider;
    private PlayerInput playerInput;

    private Vector2 movementInput;

    private void Awake()
    {
        FindObjectOfType<CameraController>().ConnectPlayerToInstances(transform);

        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        hasSpawned = false;
        rb.useGravity = false;
        myGravityDirection = myGravityDirection.normalized;
    }

    private void FixedUpdate()
    {
        // Always adding force to the gravity direction
        rb.AddForce(myGravityDirection * myGravityStrength);

        ForceMovement();
    }

    private void Update()
    {
        isGrounded = GroundCheck();

        GravityBehaviour();
    }

    private void ForceMovement()
    {
        float inAirMovementReduction = 1;
        if (!isGrounded)
        {
            inAirMovementReduction = inAirMovementStrenghtReduction;
        }

        Quaternion targetRotation;
        if (myGravityDirection == Vector3.down)
        {
            targetRotation = Quaternion.FromToRotation(myGravityDirection, Vector3.down);
        }
        else
        {
            targetRotation = Quaternion.FromToRotation(myGravityDirection, Vector3.up);
        }

        Vector3 movementInput = targetRotation * new Vector3(this.movementInput.x, 0, this.movementInput.y);
        Vector3 movement = movementInput * movementBuildupStrenght * inAirMovementReduction;
        rb.AddForce(movement, ForceMode.VelocityChange);

        // counter velocity if too fast
        Vector3 horizontalMovement = relativeToGravityHorizontalDirectionVector(Vector3.zero, -rb.velocity, myGravityDirection);
        if (horizontalMovement.magnitude > maxMovementVelocityThreshold)
        {
            horizontalMovement *= movementCounterStrength * inAirMovementReduction;
            rb.AddForce(horizontalMovement);
        }
        //Debug.DrawLine(transform.position, transform.position + relativeToGravityHorizontalDirectionVector(Vector3.zero, -rb.velocity, myGravityDirection));
        //Debug.DrawLine(transform.position, transform.position + movementInput);
    }

    private Vector3 relativeToGravityHorizontalDirectionVector(Vector3 mainPosition, Vector3 positionTowards, Vector3 gravity)
    {
        float angle = Vector3.Angle(-gravity, positionTowards - mainPosition);
        float distanceCloseToAngleVector = Mathf.Cos(angle * Mathf.Deg2Rad) * Vector3.Distance(mainPosition, positionTowards);
        Vector3 heightVector = gravity.normalized * distanceCloseToAngleVector;
        return (positionTowards - mainPosition) + heightVector;
    }

    private Vector3 GetColliderBottom()
    {
        return transform.position + (transform.rotation * (Vector3.down * capsuleCollider.height / 4));
    }

    private RaycastHit GetBottomHitPosition()
    {
        RaycastHit hit;
        float maxRayDistance = 0.2f;
        Physics.SphereCast(GetColliderBottom() - myGravityDirection * 0.1f, capsuleCollider.radius, myGravityDirection, out hit, maxRayDistance);
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

    private void GravityBehaviour()
    {
        Quaternion targetRotation = Quaternion.LookRotation(myGravityDirection, -myGravityDirection) * Quaternion.Euler(-90, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, myGravityRotationSpeed);
    }

    public void ChangeGravityDirection(Vector3 direction)
    {
        myGravityDirection = direction.normalized;
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
            rb.AddForce(-myGravityDirection * jumpStrenght, ForceMode.VelocityChange);
        }
    }

    public void PickUp(InputAction.CallbackContext context)
    {
        //if (context.started)
    }
    #endregion
}
