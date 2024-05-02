using UnityEngine;

public abstract class BasePlayerMover : MonoBehaviour
{
    [SerializeField] protected float maxSpeed = 25f;
    [SerializeField] protected float lerpSpeed = 8f;
    [SerializeField] private bool rawAxis;

    protected float _currentSpeed;
    
    public Vector2 MoveVector
    {
        get
        {
            if (rawAxis)
            {
                return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
            
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }

    protected virtual float HandleSpeedAcceleration(float current, float final)
    {
        current = Mathf.Lerp(current, final, lerpSpeed * Time.deltaTime);
        return current;
    }
    
    public abstract void Move();

    public abstract void Rotate();
    
    
}