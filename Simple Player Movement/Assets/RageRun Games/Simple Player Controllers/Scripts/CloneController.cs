using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CloneController : MonoBehaviour
{
    public static CloneController Instance;
    
    [Header("Clone Settings")] 
    [SerializeField] private CloneObject clonePrefab;

    [SerializeField] private int maxClones = 5;
    [SerializeField] private float durationBetweenClones = 0.5f;
    [SerializeField] private float cloneAlpha = 0.9f;
    [SerializeField] private float cloneFadeSpeed = 0.5f;
    
    [SerializeField] private AnimationCurve cloneScaleCurve;
    [SerializeField] private Gradient cloneColorGradient;

    private float startCloneAlpha = 0.9f;
    
    public ObjectPool<CloneObject> clonePool;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startCloneAlpha = cloneAlpha;
        
        clonePool = new ObjectPool<CloneObject>(() =>
        {
            var clone = Instantiate(clonePrefab);
            clone.gameObject.SetActive(false);
            return clone;
        }, clone => clone.gameObject.SetActive(true), clone => clone.gameObject.SetActive(false));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(SpawnClonesRoutine());
        }
    }
    
   
    private IEnumerator SpawnClonesRoutine()
    {
        cloneFadeSpeed = 1f / maxClones;

        for (int i = 0; i < maxClones; i++)
        {
            CloneObject clone = clonePool.Get();
            clone.gameObject.SetActive(true);
            clone.CopyMainBodyTransform(transform);
            clone.SetSprite(clone.Sprite);
            cloneAlpha -= cloneFadeSpeed;
            clone.SetColorWithAlpha(cloneColorGradient.Evaluate(cloneAlpha), cloneAlpha);
            clone.ShrinkScaleOverTime(cloneScaleCurve.Evaluate(cloneAlpha));

            yield return new WaitForSeconds(durationBetweenClones);
        }

        cloneAlpha = startCloneAlpha;
    }
}