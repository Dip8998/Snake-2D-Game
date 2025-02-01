using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameObject gameOverPanelforSnake1;
    [SerializeField] private GameObject gameOverPanelforSnake2;
    [SerializeField] private GameOverController gameOverController;
    [SerializeField] private PowerUpController powerUpController;
    [SerializeField] private GameObject gameOverPanelforTie;
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

    private float baseMoveInterval = 0.2f; 
    private float moveInterval;
    private float nextMoveTime;
    private void Start()
    {
        tails = new List<Transform> { transform };
        moveInterval = baseMoveInterval;
        nextMoveTime = Time.time;
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
        if (Time.time >= nextMoveTime)
        {
            MoveSnake();
            HandleWrapping();
            nextMoveTime = Time.time + moveInterval; 
            canChangeDir = true;
        }
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
        int scoreMultiplier = powerUpController.HasScoreBoost(this) ? 2 : 1;
        int n = 10;
        scoreController.IncreaseScore(isSnake1, n * scoreMultiplier);
    }


    private void HandlePowerUpCollision(Collider2D collision)
    {
        ItemController item = collision.GetComponent<ItemController>();
        if (item == null) return;

        switch (item.GetName())
        {
            case "MassGainer":
                ModifyTail(1, true);
                Scored();
                break;

            case "MassBurner":
                if (powerUpController.HasShield(this)) return;
                ModifyTail(1, false);
                scoreController.DecreaseScore(isSnake1, 10);
                break;

            case "Shield":
                powerUpController.ActivateShield(this, 5f);
                break;

            case "ScoreBoost":
                powerUpController.ActivateScoreBoost(this, 5f);
                break;

            case "SpeedBoost":
                powerUpController.ActivateSpeedBoost(this, 5f, moveInterval);
                break;
        }

        SoundController.Instance.Play(Sounds.EatsSound);
        Destroy(collision.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPowerUp = true;

        if (!powerUpController.HasShield(this))
        {
            if (collision.tag == "Tail")
            {
                if (isSnake1)
                {
                    gameOverPanelforSnake1.SetActive(true);
                }
                else
                {
                    gameOverPanelforSnake2.SetActive(true);
                }
                isPowerUp = false;
            }
            else if ((collision.tag == "Player2" && isSnake1) || (collision.tag == "Player" && !isSnake1))
            {
                gameOverPanelforTie.SetActive(true);
                isPowerUp = false;
            }
        }

        if (!isPowerUp)
        {
            SoundController.Instance.Play(Sounds.GameOver);
            Time.timeScale = 0f;
        }
        else
        {
            HandlePowerUpCollision(collision);
        }
    }


}
