using System;
using UnityEngine;

public class SwingingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GroundCheck groundCheck;
    
    [Header("Swing Settings")]
    [SerializeField] private float angleDifference = 20f;
    
    [Header("Swing Hook Settings")]
    [SerializeField] private float swingHookCheckRadius = 1f;
    [SerializeField] private LayerMask grappleLayer;

    [SerializeField] private float rotationClampValue = 45f;
    
    [Header("Swing Speed Settings")]
    [SerializeField] private float maxCurrentSpeed = 10f;
    [SerializeField] private float minCurrentSpeed = 5f;
    
    float currentSpeed = 0f;
    private float speedMultiplier = 1f;
    
    private float maxSwingAngle;
    private float minSwingAngle;

    private float horizontalInput;
    private bool isHorizontalInput;

    private bool isSwinging = false;
    public bool IsSwinging => isSwinging;
  
    bool isFacingDirectionReversed = false;
    int lastFacingDirection = 1;
    
    public Transform hookTransform;
    private Hook currentHook;
    
    public int FacingDirection { get; private set; }
    
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        minSwingAngle = angleDifference;
        maxSwingAngle = 180f - angleDifference;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        isHorizontalInput = Mathf.Abs(horizontalInput) > 0.1f;
        
        HandleSwinging();
        HandleSwingRotation();
    }

    private void HandleSwinging()
    {
        if (!groundCheck.IsGrounded && Input.GetKey(KeyCode.E) && !isSwinging)
        {
            Collider2D hookCollider = Physics2D.OverlapCircle(transform.position, swingHookCheckRadius, grappleLayer);
            
            if (hookCollider)
            {
                isSwinging = true;
                FacingDirection = isHorizontalInput ? 1 : -1;
                
                if(hookCollider.TryGetComponent(out currentHook))
                {
                   currentHook.SetSpriteColor(isSwinging);
                }
                
                hookTransform = currentHook.transform;
            }
        }

        if (isSwinging && currentHook)
        {
            Vector3 hookDir = (currentHook.transform.position - transform.position).normalized;
            Vector3 crossDir = Vector3.Cross(hookDir, Vector3.forward).normalized;
            
            float angle = Vector2.SignedAngle(transform.right, hookDir);
            
            if (angle > maxSwingAngle && !isFacingDirectionReversed)
            {
                lastFacingDirection *= -1;
                isFacingDirectionReversed = true;
            }

            if (angle < minSwingAngle && isFacingDirectionReversed)
            {
                lastFacingDirection *= -1;
                isFacingDirectionReversed = false;
            }

            float pendulumFactor = Mathf.Sin(angle * Mathf.Deg2Rad);
            speedMultiplier = isHorizontalInput ? pendulumFactor * Mathf.Sign(horizontalInput) : pendulumFactor * lastFacingDirection; 
            
            currentSpeed = Mathf.Lerp(minCurrentSpeed, maxCurrentSpeed, Mathf.Abs(pendulumFactor)); 
            rb.velocity = speedMultiplier * currentSpeed * crossDir;
        }

        if (!Input.GetKeyUp(KeyCode.E) && !groundCheck.IsGrounded) return;
        
        RemoveSwinging();
    }

    private void RemoveSwinging()
    {
        if (isSwinging)
        {
            isSwinging = false;
        }
      
        if (!currentHook) return;
        
        rb.AddForce(rb.velocity.normalized * 7f, ForceMode2D.Impulse);
        currentHook.SetSpriteColor(isSwinging);
        currentHook = null;
        hookTransform = null;
    }

    private void HandleSwingRotation()
    {
        if (!isSwinging) return;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, Mathf.Clamp(isHorizontalInput ? horizontalInput * rotationClampValue : lastFacingDirection * rotationClampValue, -rotationClampValue, rotationClampValue));
        transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, targetRot, Time.deltaTime * 2f);
    }
}