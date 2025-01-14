using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D gridArea;
    [SerializeField] private ItemController massGainerPrefab;
    [SerializeField] private ItemController massBurnerPrefab;
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private float spawnInterval = 3.0f;
    [SerializeField] private float minDistanceBetweenFoods = 2.0f;

    private List<ItemController> activeFoods = new List<ItemController>();
    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnFood();
            spawnTimer = Random.Range(1.5f, spawnInterval);
        }
    }

    private void SpawnFood()
    {
        Vector2 randomPosition = GetValidPosition();
        ItemController prefab = snakeController.GetSnakeSize() > 1 && Random.Range(0f, 1f) > 0.7f ? massBurnerPrefab : massGainerPrefab;
        ItemController food = Instantiate(prefab, randomPosition, Quaternion.identity);
        activeFoods.Add(food);

        Destroy(food.gameObject, 10f);
        Invoke(nameof(RemoveOldFood), 10f);
    }

    private Vector2 GetValidPosition()
    {
        Bounds bounds = gridArea.bounds;
        int maxAttempts = 20;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
            Vector2 position = new Vector2(x, y);

            if (IsPositionValid(position))
                return position;
        }

        return new Vector2(
            Mathf.Round(Random.Range(bounds.min.x, bounds.max.x)),
            Mathf.Round(Random.Range(bounds.min.y, bounds.max.y))
        );
    }

    private bool IsPositionValid(Vector2 position)
    {
        foreach (ItemController food in activeFoods)
        {
            if (food != null && Vector2.Distance(food.transform.position, position) < minDistanceBetweenFoods)
                return false;
        }
        return true;
    }

    private void RemoveOldFood()
    {
        activeFoods.RemoveAll(food => food == null);
    }
}
