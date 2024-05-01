using UnityEngine;

public class PlatformerPlayerController : BasePlayerMover
{
    [SerializeField] private float dashMultiplier = 2f;

    [SerializeField] private float rotationClampValue = 15f;
    
    private void Update()
    {
        Move();
        Rotate();
    }

    public override void Move()
    {
        _currentSpeed = HandleSpeedAcceleration(_currentSpeed, maxSpeed);
        // move the player with the input

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _currentSpeed *= dashMultiplier;
        }
 
        
        transform.position += _currentSpeed * Time.deltaTime * new Vector3(MoveVector.x, 0f, 0f);
    }

    public override void Rotate()
    {
        // rotate child body on x axis
        Quaternion targetRot = Quaternion.Euler(0f, 0f, Mathf.Clamp(MoveVector.x * -rotationClampValue, -rotationClampValue, rotationClampValue));
        transform.GetChild(0).rotation = targetRot;
    }
}