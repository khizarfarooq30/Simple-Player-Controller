using UnityEngine;

public class SquishAndStretch : MonoBehaviour
{
    public Transform characterSpriteTransform;
    [SerializeField] private Transform anchor;
    
    public float stretch = 0.1f;
   
    private Rigidbody2D rigidbody2d;
    private Vector3 initialScale;
 
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        initialScale = characterSpriteTransform.transform.localScale;
    }
 
    private void Update()
    {
        characterSpriteTransform.parent = transform;
        characterSpriteTransform.localPosition = Vector3.zero;
        characterSpriteTransform.localScale = initialScale;
      
        anchor.localScale = Vector3.one;
        anchor.position = transform.position;
 
        Vector3 velocity = rigidbody2d.velocity;
 
        var scaleX = 1.0f + (velocity.magnitude * stretch);
        var scaleY = 1.0f / scaleX;
        characterSpriteTransform.parent = anchor;
        anchor.localScale = new Vector3(scaleX, scaleY, 1.0f);
    }
}
