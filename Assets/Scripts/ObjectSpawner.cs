using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType { Coin, Enemy }

    public Tilemap tilemap;
    public GameObject[] objectPrefabs; // Expect index 0 = Coin prefab, index 1 = Enemy prefab

    public int maxCoins = 10;
    public int maxEnemies = 5;
    public float spawnInterval = 0.1f;

    private List<Vector3> validSpawnPositions = new List<Vector3>();
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        GameController.OnLevelChanged += OnLevelChanged;
        InitializeSpawner();
    }

    private void OnDestroy()
    {
        GameController.OnLevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged()
    {
        InitializeSpawner();
    }

    private void InitializeSpawner()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        GatherValidPositions();
        DestroyAllSpawnedObjects();
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        int coinsSpawned = 0;
        int enemiesSpawned = 0;
        

        while (coinsSpawned < maxCoins || enemiesSpawned < maxEnemies)
        {
            if (validSpawnPositions.Count == 0)
                yield break;

            Vector3 spawnPos = GetValidSpawnPosition();

            if (spawnPos == Vector3.zero)
                yield break;

            if (coinsSpawned < maxCoins)
            {
                SpawnObject(ObjectType.Coin, spawnPos);
                coinsSpawned++;
                yield return new WaitForSeconds(spawnInterval);
            }

            if (enemiesSpawned < maxEnemies)
            {
                spawnPos = GetValidSpawnPosition();
                if (spawnPos == Vector3.zero)
                    yield break;

                SpawnObject(ObjectType.Enemy, spawnPos);
                enemiesSpawned++;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        for (int attempts = 0; attempts < 20; attempts++)
        {
            if (validSpawnPositions.Count == 0)
                break;

            int index = Random.Range(0, validSpawnPositions.Count);
            Vector3 pos = validSpawnPositions[index];

            // Check if position is free (no objects too close)
            if (!PositionHasObjectNearby(pos, 1f))
            {
                validSpawnPositions.RemoveAt(index);
                return pos;
            }
            else
            {
                // Remove invalid pos to avoid repeated checks
                validSpawnPositions.RemoveAt(index);
            }
        }
        return Vector3.zero; // No valid position found
    }

    private bool PositionHasObjectNearby(Vector3 pos, float minDistance)
    {
        return spawnedObjects.Any(obj => obj != null && Vector3.Distance(obj.transform.position, pos) < minDistance);
    }

    private void SpawnObject(ObjectType type, Vector3 position)
    {
        GameObject prefab = objectPrefabs[(int)type];
        if (prefab == null)
            return;

        GameObject spawned = Instantiate(prefab, position, Quaternion.identity);
        spawnedObjects.Add(spawned);
    }

    private void DestroyAllSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObjects.Clear();
    }

    private void GatherValidPositions()
    {
        validSpawnPositions.Clear();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Vector3 worldStart = tilemap.CellToWorld(new Vector3Int(bounds.xMin, bounds.yMin, 0));

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3 pos = worldStart + new Vector3(x + 0.5f, y + 1.5f, 0);
                    validSpawnPositions.Add(pos);
                }
            }
        }
    }
}