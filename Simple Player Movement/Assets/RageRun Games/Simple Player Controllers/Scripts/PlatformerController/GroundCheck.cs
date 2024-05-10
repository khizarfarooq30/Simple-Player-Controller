using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private ParticleSystem landingEffect;
    
    [SerializeField] private float groundCheckDistance = 0.1f; 
    [SerializeField] private LayerMask groundLayer;
    
    private bool lastGroundedState;
    
    public bool IsGrounded { get; private set; }


    void Update()
    {
        CheckForGround();
    }

    private void CheckForGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        IsGrounded = hit.collider != null;

        if (IsGrounded && !lastGroundedState)
        {
            landingEffect.Play();
        }
        
        lastGroundedState = IsGrounded;
    }
}