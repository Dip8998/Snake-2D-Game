using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Start()
    {
        StartCoroutine(SpawnPowerUps());

        foreach (SnakeController snakeController in Snakes)
        {
            snakeStates.Add(snakeController, new PowerUpState());
        }

        SetIconTransparency(speedIconSnake1, true);
        SetIconTransparency(shieldIconSnake1, true);
        SetIconTransparency(scoreBoostIconSnake1, true);
        SetIconTransparency(speedIconSnake2, true);
        SetIconTransparency(shieldIconSnake2, true);
        SetIconTransparency(scoreBoostIconSnake2, true);
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

        GameObject generatedPowerUp = Instantiate(randomPowerUp, randomPosition, Quaternion.identity);
        Destroy(generatedPowerUp, 10f);
    }

    public void ActivateShield(SnakeController snake, float duration)
    {
        PowerUpState state = snakeStates[snake];
        if (state.HasShield) return;

        state.HasShield = true;
        SetIconTransparency(snake.isSnake1 ? shieldIconSnake1 : shieldIconSnake2, false);
        snake.shield.gameObject.SetActive(true);

        StartCoroutine(DeactivateShield(snake, duration));
    }

    private IEnumerator DeactivateShield(SnakeController snake, float delay)
    {

        yield return new WaitForSeconds(delay);

        PowerUpState state = snakeStates[snake];
        state.HasShield = false;

        SetIconTransparency(snake.isSnake1 ? shieldIconSnake1 : shieldIconSnake2, true);
        snake.shield.gameObject.SetActive(false);
    }

    public void ActivateSpeedBoost(SnakeController snake, float duration, float moveInterval)
    {
        PowerUpState state = snakeStates[snake];
        if (state.HasSpeedBoost) return;
        float orgMove = moveInterval;
        moveInterval = 0.1f;
        state.HasSpeedBoost = true;
        SetIconTransparency(snake.isSnake1 ? speedIconSnake1 : speedIconSnake2, false);
        StartCoroutine(DeactivateSpeedBoost(snake, duration,orgMove,moveInterval));
    }

    private IEnumerator DeactivateSpeedBoost(SnakeController snake, float delay, float orgMove, float moveInterval)
    {
        yield return new WaitForSeconds(delay);
        moveInterval = orgMove;
        PowerUpState state = snakeStates[snake];
        state.HasSpeedBoost = false;
        SetIconTransparency(snake.isSnake1 ? speedIconSnake1 : speedIconSnake2, true);
    }

    public void ActivateScoreBoost(SnakeController snake, float duration)
    {
        PowerUpState state = snakeStates[snake];
        if (state.HasScoreBooster) return;

        state.HasScoreBooster = true;
        SetIconTransparency(snake.isSnake1 ? scoreBoostIconSnake1 : scoreBoostIconSnake2, false);

        StartCoroutine(DeactivateScoreBoost(snake, duration));
    }

    private IEnumerator DeactivateScoreBoost(SnakeController snake, float delay)
    {
        yield return new WaitForSeconds(delay);

        PowerUpState state = snakeStates[snake];
        state.HasScoreBooster = false;

        SetIconTransparency(snake.isSnake1 ? scoreBoostIconSnake1 : scoreBoostIconSnake2, true);
    }

    private void SetIconTransparency(Image icon, bool transparent)
    {
        Color color = icon.color;
        color.a = transparent ? 0.1f : 1f;
        icon.color = color;
    }

    public bool HasShield(SnakeController snake) => snakeStates[snake].HasShield;
    public bool HasSpeedBoost(SnakeController snake) => snakeStates[snake].HasSpeedBoost;
    public bool HasScoreBoost(SnakeController snake) => snakeStates[snake].HasScoreBooster;
}

