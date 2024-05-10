using UnityEngine;

public class PlatformerPlayerController : BasePlayerMover
{
   
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float fallMultiplier = 2.5f;
    
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    
    [Header("Rotation Settings")]
    [SerializeField] private float rotationClampValue = 15f;
    
    [Header("References")]
    [SerializeField] private ParticleSystem movementEffect;
    [SerializeField] private ParticleSystem jumpEffect;
    [SerializeField] private SwingingController swingingController;
    [SerializeField] private GroundCheck groundCheck;

    
    private Rigidbody2D rigidbody2D;

    private bool executeJump;
    private bool executeDash;
    private bool isDashing;
    
    private bool isGrounded;
    
    private float dashTimer;
    
    private int currentJumpCount;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
        swingingController = GetComponent<SwingingController>();
        dashTimer = dashDuration;
        
        currentJumpCount = maxJumpCount;
    }

    private void Update()
    {
        if(swingingController.IsSwinging) return;
        isGrounded = groundCheck.IsGrounded;
     
        if (MoveVector.x != 0 && isGrounded)
        {
            if(!movementEffect.isPlaying)
                movementEffect.Play();
            
            var linearVelocity = movementEffect.velocityOverLifetime;
            linearVelocity.x = 3f * -MoveVector.x;
        }
        else if(!isGrounded)
        {
            movementEffect.Stop();
        }
        
        ExecuteJump();
        HandleDashing();
        Rotate();
    }

    private void FixedUpdate()
    {
        if(swingingController.IsSwinging) return;
        HandleJump();
        Move();
    }


    private void ExecuteJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            currentJumpCount = maxJumpCount;
            executeJump = true;
        
            currentJumpCount--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && currentJumpCount > 0)
        {
            executeJump = true;
            currentJumpCount--;
        }
    }

    private void HandleJump()
    {
        if (executeDash && !isDashing)
        {
            isDashing = true;
            rigidbody2D.AddForce(Mathf.Sign(MoveVector.x) * dashForce * Vector2.right, ForceMode2D.Impulse);
        }

        if (executeJump)
        {
            executeJump = false;
            
            jumpEffect.Play();
            var direction = rigidbody2D.velocity.normalized;
            
            var linearVelocity = jumpEffect.velocityOverLifetime;
            linearVelocity.x = 5 * direction.x;
            linearVelocity.y = 5 * direction.y;
            
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
            rigidbody2D.velocity = Mathf.Sqrt( 2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y) ) * Vector2.up;
        }
        
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime * Vector2.up;
        } 
        else if (rigidbody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigidbody2D.velocity += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * Vector2.up;
        }
    }

    public override void Move()
    {
        if(isDashing || swingingController.IsSwinging) return;
        _currentSpeed = HandleSpeedAcceleration(_currentSpeed, maxSpeed);
        rigidbody2D.velocity = new Vector2(MoveVector.x * _currentSpeed, rigidbody2D.velocity.y);
    }

    public override void Rotate()
    {
        Quaternion targetRot = Quaternion.Euler(0f, 0f, Mathf.Clamp(MoveVector.x * -rotationClampValue, -rotationClampValue, rotationClampValue));
        transform.GetChild(0).rotation = targetRot;
    }
    
    private void HandleDashing()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            executeDash = true;
        }

        if (executeDash)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
                executeDash = false;
                dashTimer = dashDuration;
                rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
            }
        }
    }
}