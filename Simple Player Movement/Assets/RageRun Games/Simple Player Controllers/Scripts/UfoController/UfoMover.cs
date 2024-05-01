using UnityEngine;

public class UfoMover : BasePlayerMover
{
    [Header("References")] [SerializeField]
    private Transform ufoBody;

    [SerializeField] private GameObject laser;

    [Header("UFO Movement Settings")] 
    [SerializeField] private float dragForce = 8f;
    [SerializeField] private float ufoRotationLimit = 20f;

    [SerializeField] private float raycastDistance = 5f;

    private Rigidbody2D _playerRB;

    void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            laser.SetActive(true);

            _playerRB.velocity = Vector2.zero;
            _playerRB.angularVelocity = 0;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            laser.SetActive(false);
        }

        Rotate();
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    public override void Rotate()
    {
        if (MoveVector.x != 0)
        {
            ufoBody.rotation = Quaternion.Slerp(ufoBody.rotation,
                Quaternion.Euler(0, 0, -MoveVector.x * ufoRotationLimit), Time.deltaTime * 5f);
        }
        else
        {
            ufoBody.rotation =
                Quaternion.Slerp(ufoBody.rotation, Quaternion.Euler(0, 0, 0f), Time.deltaTime * 5f);
        }
    }

    public override void Move()
    {
        if (MoveVector.magnitude <= 0.25)
        {
            _playerRB.drag = HandleSpeedAcceleration( _playerRB.drag, dragForce * 0.5f);
            _currentSpeed = HandleSpeedAcceleration(_currentSpeed, 0);
        }
        else
        {
            _playerRB.drag = 0.5f;
            _currentSpeed = HandleSpeedAcceleration(_currentSpeed, maxSpeed);
        }

        Vector2 engineForce = _currentSpeed * MoveVector.y * _playerRB.transform.up +
                              _currentSpeed * MoveVector.x * _playerRB.transform.right;
        _playerRB.AddForce(engineForce, ForceMode2D.Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * raycastDistance);
    }
}