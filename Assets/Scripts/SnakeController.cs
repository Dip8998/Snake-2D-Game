using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameObject gameOverPanelSnake1;
    [SerializeField] private GameObject gameOverPanelSnake2;
    [SerializeField] private GameOverController gameOverController;
    [SerializeField] private PowerUpController powerUpController;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject upperWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private Transform tailPrefab;
    public GameObject shield;
    public bool isSnake1;

    private Vector2 snakeDir = Vector2.right;
    private List<Transform> tails;
    private bool isWrapping = false;
    private bool canChangeDir = true;
   

    private void Start()
    {
        Time.fixedDeltaTime = 0.08f;
        tails = new List<Transform> { transform };
      
    }

    private void Update()
    {
        if (canChangeDir && !isWrapping)
        {
            Vector2 newDir = GetInputDirection();
            if (newDir != Vector2.zero && newDir != -snakeDir)
            {
                snakeDir = newDir;
                canChangeDir = false;
            }
        }
    }

    private Vector2 GetInputDirection()
    {
        if (isSnake1)
        {
            if (Input.GetKey(KeyCode.W)) return Vector2.up;
            if (Input.GetKey(KeyCode.S)) return Vector2.down;
            if (Input.GetKey(KeyCode.A)) return Vector2.left;
            if (Input.GetKey(KeyCode.D)) return Vector2.right;
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow)) return Vector2.up;
            if (Input.GetKey(KeyCode.DownArrow)) return Vector2.down;
            if (Input.GetKey(KeyCode.LeftArrow)) return Vector2.left;
            if (Input.GetKey(KeyCode.RightArrow)) return Vector2.right;
        }
        return Vector2.zero;
    }

    private void FixedUpdate()
    {
        MoveSnake();
        HandleWrapping();
        canChangeDir = true;
    }

    private void MoveSnake()
    {
        for (int i = tails.Count - 1; i > 0; i--)
        {
            tails[i].position = tails[i - 1].position;
        }

        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + snakeDir.x,
            Mathf.Round(transform.position.y) + snakeDir.y,
            0.0f
        );
    }

    private void HandleWrapping()
    {
        float rightLimit = rightWall.transform.position.x - 0.5f;
        float leftLimit = leftWall.transform.position.x + 0.5f;
        float topLimit = upperWall.transform.position.y - 0.5f;
        float bottomLimit = bottomWall.transform.position.y + 0.5f;

        if (!isWrapping)
        {
            if (transform.position.x > rightLimit)
            {
                transform.position = new Vector2(leftLimit, transform.position.y);
                isWrapping = true;
            }
            else if (transform.position.x < leftLimit)
            {
                transform.position = new Vector2(rightLimit, transform.position.y);
                isWrapping = true;
            }
            else if (transform.position.y > topLimit)
            {
                transform.position = new Vector2(transform.position.x, bottomLimit);
                isWrapping = true;
            }
            else if (transform.position.y < bottomLimit)
            {
                transform.position = new Vector2(transform.position.x, topLimit);
                isWrapping = true;
            }
        }
        if (isWrapping)
        {
            isWrapping = false;
        }
    }

    private void ModifyTail(int count, bool add)
    {
        for (int i = 0; i < count && (add || tails.Count > 1); i++)
        {
            if (add)
            {
                Transform tail = Instantiate(tailPrefab);
                tail.position = tails[tails.Count - 1].position;
                tails.Add(tail);
            }
            else if (tails.Count > 1)
            {
                Transform lastTail = tails[tails.Count - 1];
                tails.RemoveAt(tails.Count - 1);
                Destroy(lastTail.gameObject);
            }
        }
    }

    public int GetSnakeSize()
    {
        return tails != null ? tails.Count : 0;
    }

    private void Scored()
    {
        int n = 10;
        if (!powerUpController.HasScoreBoost(this) && isSnake1)
        {
            scoreController.IncreaseScore(isSnake1,n);
        }
        if (powerUpController.HasScoreBoost(this) && isSnake1)
        {

            scoreController.IncreaseScore(isSnake1,n * 2);
        }
        if (!powerUpController.HasScoreBoost(this) && !isSnake1)
        {
            scoreController.IncreaseScore(isSnake1, n);
        }
        if (powerUpController.HasScoreBoost(this) && !isSnake1)
        {

            scoreController.IncreaseScore(isSnake1, n * 2);
        }

    }

    private void HandlePowerUpCollision(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "MassGainer":
                SoundController.Instance.Play(Sounds.MassGainerEats);
                Destroy(collision.gameObject);
                ModifyTail(1, true);
                Scored();
                break;

            case "MassBurner":
                if (!powerUpController.HasShield(this))
                {
                    SoundController.Instance.Play(Sounds.MassBurnerEats);
                    Destroy(collision.gameObject);
                    ModifyTail(1, false);
                    scoreController.DecreaseScore(isSnake1, 10);
                }
                break;

            case "Shield":
                SoundController.Instance.Play(Sounds.PowerUpsCollect);
                Destroy(collision.gameObject);
                powerUpController.ActivateShield(this, 5f);
                break;

            case "ScoreBoost":
                SoundController.Instance.Play(Sounds.PowerUpsCollect);
                Destroy(collision.gameObject);
                powerUpController.ActivateScoreBoost(this, 5f);
                break;

            case "SpeedBoost":
                SoundController.Instance.Play(Sounds.PowerUpsCollect);
                Destroy(collision.gameObject);
                powerUpController.ActivateSpeedBoost(this, 5f);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSnake1)
        {
            if (collision.tag == "Tail" && !powerUpController.HasShield(this))
            {
               gameOverPanelSnake1.SetActive(true);
               SoundController.Instance.Play(Sounds.GameOver);
                Time.timeScale = 0f;
            }
            else if (collision.tag == "Player2")
            {
                if (!powerUpController.HasShield(this))
                {
                    gameOverPanel.SetActive(true);
                    SoundController.Instance.Play(Sounds.GameOver);
                    Time.timeScale = 0f;
                }
            }
            else
            {
                HandlePowerUpCollision(collision);
            }
        }
        else
        {
            if (collision.tag == "Tail" && !powerUpController.HasShield(this))
            {
                gameOverPanelSnake2.SetActive(true);
                SoundController.Instance.Play(Sounds.GameOver);
                Time.timeScale = 0f;
            }
            else if (collision.tag == "Player" && !powerUpController.HasShield(this))
            {
                gameOverPanel.SetActive(true);
                SoundController.Instance.Play(Sounds.GameOver);
                Time.timeScale = 0f;
            }
            else
            {
                HandlePowerUpCollision(collision);
            }
        }
    }


}
