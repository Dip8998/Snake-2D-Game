using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameObject upperWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject shield;
    [SerializeField] private Transform tailPrefab;
    [SerializeField] private bool isSnake1;

    private Vector2 snakeDir = Vector2.right;
    private List<Transform> tails;
    private bool isWrapping = false;
    private bool canChangeDir = true;
    private bool hasShield = false;
    private bool hasScoreBooster = false;
    private float speedBoostDuration = 5f;
    
    private void Start()
    {
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

    private IEnumerator ActivateShield(float delay)
    {
        hasShield = true;
        shield.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        hasShield = false;
        shield.gameObject.SetActive(false);
    }

    private IEnumerator ActivateSpeedBoost(float duration)
    {
        Time.fixedDeltaTime = 0.03f;
        yield return new WaitForSeconds(duration);
        Time.fixedDeltaTime = 0.07f; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "MassGainer":
                if (hasScoreBooster == false)
                {
                    Destroy(collision.gameObject);
                    ModifyTail(1, true);
                    scoreController.IncreaseScore(10);
                }
                break;
            case "MassBurner":
                if (hasScoreBooster == false)
                {
                    Destroy(collision.gameObject);
                    ModifyTail(1, false);
                    scoreController.DecreaseScore(10);
                }
                break;
            case "Tail":
                if (hasShield == false)
                {
                    SceneManager.LoadScene(0);
                    Time.fixedDeltaTime = 0.07f;
                }
                break;
            case "Shield":
                Destroy(collision.gameObject);
                StartCoroutine(ActivateShield(5f));
                break;
            case "ScoreBoost":
                Destroy(collision.gameObject);
                scoreController.ActivateScoreBooster(5f);
                break;
            case "SpeedBoost":
                Destroy(collision.gameObject);
                StartCoroutine(ActivateSpeedBoost(speedBoostDuration));
                break;
            case "Player2":
                if(hasShield == false)
                {
                    SceneManager.LoadScene(1);
                }
                break;
        }
    }
}
