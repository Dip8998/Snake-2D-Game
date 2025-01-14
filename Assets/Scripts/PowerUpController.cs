using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpState
{
    public bool HasShield { get; set; }
    public bool HasSpeedBoost { get; set; }
    public bool HasScoreBooster { get; set; }
}

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private BoxCollider2D gridArea;
    [SerializeField] private Image speedIconSnake1;
    [SerializeField] private Image shieldIconSnake1;
    [SerializeField] private Image scoreBoostIconSnake1;
    [SerializeField] private Image speedIconSnake2;
    [SerializeField] private Image shieldIconSnake2;
    [SerializeField] private Image scoreBoostIconSnake2;

    private Dictionary<SnakeController, PowerUpState> snakeStates = new();
    public List<SnakeController> Snakes;

    private float spawnTimer;

    private void Start()
    {
        foreach (SnakeController snake in Snakes)
        {
            snakeStates.Add(snake,new PowerUpState());
        }

        ResetIcons();
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnPowerUp();
            spawnTimer = Random.Range(spawnInterval, 10f);
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

        GameObject generatedPowerUp = Instantiate(randomPowerUp, randomPosition, Quaternion.identity);
        Destroy(generatedPowerUp, 10f);
    }

    public void ActivateShield(SnakeController snake, float duration)
    {
        if (snakeStates.ContainsKey(snake) && !snakeStates[snake].HasShield)
        {
            snakeStates[snake].HasShield = true;
            SetIconTransparency(snake.isSnake1 ? shieldIconSnake1 : shieldIconSnake2, false);
            snake.shield.gameObject.SetActive(true);

            StartCoroutine(DeactivateShieldAfterDelay(snake, duration));
        }
    }

    private IEnumerator DeactivateShieldAfterDelay(SnakeController snake, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (snakeStates.ContainsKey(snake))
        {
            snakeStates[snake].HasShield = false;
            SetIconTransparency(snake.isSnake1 ? shieldIconSnake1 : shieldIconSnake2, true);
            snake.shield.gameObject.SetActive(false);
        }
    }

    public void ActivateSpeedBoost(SnakeController snake, float duration, float moveInterval)
    {
        if (snakeStates.ContainsKey(snake) && !snakeStates[snake].HasSpeedBoost)
        {
            float originalInterval = moveInterval;
            moveInterval = 0.1f;

            snakeStates[snake].HasSpeedBoost = true;
            SetIconTransparency(snake.isSnake1 ? speedIconSnake1 : speedIconSnake2, false);

            StartCoroutine(DeactivateSpeedBoostAfterDelay(snake, duration, moveInterval, originalInterval));
        }
    }

    private IEnumerator DeactivateSpeedBoostAfterDelay(SnakeController snake, float delay, float moveInterval, float originalInterval)
    {
        yield return new WaitForSeconds(delay);

        if (snakeStates.ContainsKey(snake)) 
        {
            moveInterval = originalInterval;
            snakeStates[snake].HasSpeedBoost = false;
            SetIconTransparency(snake.isSnake1 ? speedIconSnake1 : speedIconSnake2, true);
        }
    }

    public void ActivateScoreBoost(SnakeController snake, float duration)
    {
        if (snakeStates.ContainsKey(snake) && !snakeStates[snake].HasScoreBooster)
        {
            snakeStates[snake].HasScoreBooster = true;
            SetIconTransparency(snake.isSnake1 ? scoreBoostIconSnake1 : scoreBoostIconSnake2, false);

            StartCoroutine(DeactivateScoreBoostAfterDelay(snake, duration));
        }
    }

    private IEnumerator DeactivateScoreBoostAfterDelay(SnakeController snake, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (snakeStates.ContainsKey(snake)) 
        {
            snakeStates[snake].HasScoreBooster = false;
            SetIconTransparency(snake.isSnake1 ? scoreBoostIconSnake1 : scoreBoostIconSnake2, true);
        }
    }

    private void ResetIcons()
    {
        SetIconTransparency(speedIconSnake1, true);
        SetIconTransparency(shieldIconSnake1, true);
        SetIconTransparency(scoreBoostIconSnake1, true);
        SetIconTransparency(speedIconSnake2, true);
        SetIconTransparency(shieldIconSnake2, true);
        SetIconTransparency(scoreBoostIconSnake2, true);
    }

    private void SetIconTransparency(Image icon, bool transparent)
    {
        Color color = icon.color;
        color.a = transparent ? 0.1f : 1f;
        icon.color = color;
    }

    public bool HasShield(SnakeController snake)
    {
        return snakeStates.ContainsKey(snake) && snakeStates[snake].HasShield;
    }

    public bool HasSpeedBoost(SnakeController snake)
    {
        return snakeStates.ContainsKey(snake) && snakeStates[snake].HasSpeedBoost;
    }

    public bool HasScoreBoost(SnakeController snake)
    {
        return snakeStates.ContainsKey(snake) && snakeStates[snake].HasScoreBooster;
    }
}