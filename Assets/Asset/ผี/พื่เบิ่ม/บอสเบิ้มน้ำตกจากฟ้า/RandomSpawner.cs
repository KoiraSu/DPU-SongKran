using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject fallingObjectPrefab;

    public float minX = -70f;
    public float maxX = 70f;
    public float spawnY = 90f;

    public float spawnDelay = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 1f, spawnDelay);
    }

    void SpawnObject()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(minX, maxX),
            spawnY
        );

        Instantiate(fallingObjectPrefab, spawnPos, Quaternion.identity);
    }
}