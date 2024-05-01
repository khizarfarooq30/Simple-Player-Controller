using UnityEngine;

public class CloneObject : MonoBehaviour
{
    enum ParticlePlayMode
    {
       PlayOnSpawn,
       PlayOnRelease
    }
    
    [SerializeField] ParticlePlayMode particlePlayMode;
    
    [SerializeField] private float shrinkSpeed = 2f;
    [SerializeField] float shrinkDuration = 0.5f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem particleSystem;

    public Sprite Sprite => spriteRenderer.sprite;

    private float shrinkTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shrinkTimer = shrinkDuration;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        particleSystem.transform.parent = null;
        
       
    }

    private void Update()
    {
        shrinkTimer -= Time.deltaTime;
     
        if (shrinkTimer <= 0)
        {
            Shrink(shrinkSpeed);
        }
      
        if (transform.localScale.x <= 0.1f)
        {
            shrinkTimer = shrinkDuration;
            
            if(particlePlayMode == ParticlePlayMode.PlayOnRelease)
            {
                particleSystem.transform.position = transform.position;
                particleSystem.Play();
            }
         
            CloneController.Instance.clonePool.Release(this);
            gameObject.SetActive(false);
        }
    }

    public void CopyMainBodyTransform(Transform mainBodyToCloneFrom)
    {
        transform.position = mainBodyToCloneFrom.position;
        transform.rotation = mainBodyToCloneFrom.rotation;
        
        if (particlePlayMode == ParticlePlayMode.PlayOnSpawn)
        {
            particleSystem.transform.position = transform.position;
            particleSystem.Play();
        }
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetColorWithAlpha(Color newColor, float alpha)
    {
        var color = spriteRenderer.color;
        color = new Color(newColor.r, newColor.g, newColor.b, 1 - alpha);
        spriteRenderer.color = color;

        var main = particleSystem.main;
        main.startColor = new Color(newColor.r, newColor.g, newColor.b);
    }

    public void Shrink(float shrinkSpeed)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * shrinkSpeed);
    }
}