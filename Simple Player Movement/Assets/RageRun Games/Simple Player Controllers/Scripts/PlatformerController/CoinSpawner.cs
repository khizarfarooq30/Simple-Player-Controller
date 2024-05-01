using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner Instance;
    
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float spawnRate = 1f;
    
    [SerializeField] float minX = -10f;
    [SerializeField] float maxX = 10f;
    
    public ObjectPool<GameObject> coinPool;

    private float _spawnTimer;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        coinPool = new ObjectPool<GameObject>(() =>
        {
            var coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            return coin;
        }, coin => coin.SetActive(true));
        
        _spawnTimer = spawnRate;
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0)
        {
            SpawnCoin();
            _spawnTimer = spawnRate;
        }
    }

    private void SpawnCoin()
    {
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(minX, maxX), 0f, 0f);
        GameObject newCoin = coinPool.Get();
        newCoin.transform.position = spawnPosition;
    }
}
