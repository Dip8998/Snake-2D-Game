using System.Collections.Generic;
using System.Collections;
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

    private void Start()
    {
        StartCoroutine(AutoFoodGeneration());
    }

    private IEnumerator AutoFoodGeneration()
    {
        SpawnFood();
        yield return new WaitForSeconds(Random.Range(1.5f, spawnInterval));
        StartCoroutine(AutoFoodGeneration());
    }

    private void SpawnFood()
    {
        Vector2 randomPosition = GetValidPosition();
        ItemController prefab = snakeController.GetSnakeSize() > 1 && Random.Range(0f, 1f) > 0.7f ? massBurnerPrefab : massGainerPrefab;
        ItemController food = Instantiate(prefab, randomPosition, Quaternion.identity);
        activeFoods.Add(food);
        Destroy(food, 10);
        StartCoroutine(RemoveFoodAfterDelay(food, 10));
    }

    private Vector2 GetValidPosition()
    {
        Bounds bounds = gridArea.bounds;
        int maxAttempts = 20;
        Vector2 position = Vector2.zero;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
            position = new Vector2(x, y);

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
        foreach (var food in activeFoods)
        {
            if (food != null && Vector2.Distance(food.transform.position, position) < minDistanceBetweenFoods)
                return false;
        }
        return true;
    }

    private IEnumerator RemoveFoodAfterDelay(ItemController food, float delay)
    {
        yield return new WaitForSeconds(delay);
        activeFoods.Remove(food);
    }
}