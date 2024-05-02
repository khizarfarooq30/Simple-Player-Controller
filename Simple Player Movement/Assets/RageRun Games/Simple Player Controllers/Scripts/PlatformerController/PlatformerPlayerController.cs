using UnityEngine;

public class PlatformerPlayerController : BasePlayerMover
{
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float rotationClampValue = 15f;
    
    [SerializeField] private float dashDuration = 0.5f;

    private Rigidbody2D rigidbody2D;

    private bool executeJump;
    private bool executeDash;
    private bool isDashing;
    
    private float dashTimer;
    
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        dashTimer = dashDuration;
    }

    private void Update()
    {
        Rotate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            executeJump = true;
        }
        
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

    private void FixedUpdate()
    {
        if (executeDash && !isDashing)
        {
            isDashing = true;
            rigidbody2D.AddForce(Mathf.Sign(MoveVector.x) * dashForce * Vector2.right, ForceMode2D.Impulse);
        }

        if (executeJump)
        {
            executeJump = false;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (1.5f - 1) * Time.deltaTime;
        } 
        else if (rigidbody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (2 - 1) * Time.deltaTime;
        }
        
        Move();
    }

    public override void Move()
    {
        if(isDashing) return;
        _currentSpeed = HandleSpeedAcceleration(_currentSpeed, maxSpeed);
        rigidbody2D.velocity = new Vector2(MoveVector.x * _currentSpeed, rigidbody2D.velocity.y);
    }

    public override void Rotate()
    {
        // rotate child body on x axis
        Quaternion targetRot = Quaternion.Euler(0f, 0f, Mathf.Clamp(MoveVector.x * -rotationClampValue, -rotationClampValue, rotationClampValue));
        transform.GetChild(0).rotation = targetRot;
    }
}