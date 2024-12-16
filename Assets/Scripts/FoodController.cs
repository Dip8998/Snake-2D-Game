using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D gridArea;
    [SerializeField] private GameObject massGainerPrefab;
    [SerializeField] private GameObject massBurnerPrefab;
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private float spawnInterval = 3.0f;
    private void Start()
    {
        StartCoroutine(AutoFoodGeneration());
    }

    private IEnumerator AutoFoodGeneration()
    {
        while (true)
        {
            SpawnFood();
            float foodInterval = Random.Range(1.5f,spawnInterval);
            yield return new WaitForSeconds(foodInterval);
        }
    }

    public void SpawnFood()
    {
        Bounds bounds = gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        Vector2 randomPosition = new Vector2(Mathf.Round(x), Mathf.Round(y));

        GameObject foodToSpawn;

        if(snakeController.GetSnakeSize() > 1)
        {
            foodToSpawn = Random.Range(0f, 1f) > 0.5f ? massGainerPrefab : massBurnerPrefab;
            GameObject spawnFood = Instantiate(foodToSpawn, randomPosition, Quaternion.identity);
            Destroy(spawnFood, 10);
        }
        else
        {
            foodToSpawn = massGainerPrefab;
            GameObject spawnFood = Instantiate(foodToSpawn, randomPosition, Quaternion.identity);
            Destroy(spawnFood, 10);
        }
    }
}
