using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private BoxCollider2D gridArea;

    private void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            float randInterval = Random.Range(spawnInterval, 10f);
            yield return new WaitForSeconds(randInterval);
            SpawnPowerUp();
        }
    }

    private void SpawnPowerUp()
    {
        if (powerUps.Length == 0 || gridArea == null) return;

        GameObject randomPowerUp = powerUps[Random.Range(0, powerUps.Length)];

        Bounds bounds = gridArea.bounds;

        Vector2 randomPosition = new Vector2(
            Mathf.Round(Random.Range(bounds.min.x, bounds.max.x)),
            Mathf.Round(Random.Range(bounds.min.y, bounds.max.y))
        );

        GameObject generatedPowerUps  = Instantiate(randomPowerUp, randomPosition, Quaternion.identity);
        Destroy(generatedPowerUps, 10f);
    }
}
