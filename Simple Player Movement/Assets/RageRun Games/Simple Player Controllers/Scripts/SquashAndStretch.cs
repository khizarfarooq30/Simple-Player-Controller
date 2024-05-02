using UnityEngine;

public class SquishAndStretch : MonoBehaviour
{
    public Transform Sprite;
    public float Stretch = 0.1f;
    [SerializeField] private Transform squashParent;
 
    private Rigidbody2D _rigidbody;
    private Vector3 _originalScale;
 
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _originalScale = Sprite.transform.localScale;
    }
 
    private void Update()
    {
        Sprite.parent = transform;
        Sprite.localPosition = Vector3.zero;
        Sprite.localScale = _originalScale;
      
        squashParent.localScale = Vector3.one;
        squashParent.position = transform.position;
 
        Vector3 velocity = _rigidbody.velocity;
 
        var scaleX = 1.0f + (velocity.magnitude * Stretch);
        var scaleY = 1.0f / scaleX;
        Sprite.parent = squashParent;
        squashParent.localScale = new Vector3(scaleX, scaleY, 1.0f);
    }
}
