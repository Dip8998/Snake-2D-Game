using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private GameObject upperWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private Transform tailPreFab;

    private Vector2 snakeDir = Vector2.right;
    private List<Transform> tails;
    private bool isWrapping = false;

    private void Start()
    {
        tails = new List<Transform>();

        if (this.transform != null)
        {
            tails.Add(this.transform);
        }
    }
    private void Update()
    {
        if (!isWrapping)
        {
            if (Input.GetKey(KeyCode.W) && snakeDir != Vector2.down)
            {
                snakeDir = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S) && snakeDir != Vector2.up)
            {
                snakeDir = Vector2.down;
            }
            else if (Input.GetKey(KeyCode.A) && snakeDir != Vector2.right)
            {
                snakeDir = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.D) && snakeDir != Vector2.left)
            {
                snakeDir = Vector2.right;
            }
        }
    }

    private void FixedUpdate()
    {   
        for(int i = tails.Count - 1; i > 0; i--)
        {
            tails[i].position = tails[i-1].position;
        }


        this.transform.position = new Vector3(
        Mathf.Round(this.transform.position.x) + snakeDir.x,
        Mathf.Round(this.transform.position.y) + snakeDir.y,
        0.0f
            );

        if (!isWrapping)
        {
            if (transform.position.x > rightWall.transform.position.x - 1f)
            {
                transform.position = new Vector2(leftWall.transform.position.x + 1f, transform.position.y);
                isWrapping = true;
            }
            else if (transform.position.x < leftWall.transform.position.x + 1f)
            {
                transform.position = new Vector2(rightWall.transform.position.x - 1f, transform.position.y);
                isWrapping = true;
            }
            else if (transform.position.y > upperWall.transform.position.y - 0.6f)
            {
                transform.position = new Vector2(transform.position.x, bottomWall.transform.position.y + 1f);
                isWrapping = true;
            }
            else if (transform.position.y < bottomWall.transform.position.y + 0.6f)
            {
                transform.position = new Vector2(transform.position.x, upperWall.transform.position.y - 1f);
                isWrapping = true;
            }
        }
        if (isWrapping)
        {
            isWrapping = false;
        }
    }

    private void InstantiateTail(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Transform tail = Instantiate(this.tailPreFab);
            tail.position = tails[tails.Count - 1].position;
            tails.Add(tail);
        }
    }
    private void DecreaseTail(int count = 1)
    {
        for (int i = 0; i < count && tails.Count > 1; i++)
        {
            if(tails.Count > 2)
            {
                Transform lastTail = tails[tails.Count - 1];
                tails.RemoveAt(tails.Count - 1);
                Destroy(lastTail.gameObject);
            }
        }
    }
    public int GetSnakeSize()
    {
        if (tails == null)
        {
            return 0; 
        }
        return tails.Count;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "MassGainer")
        {
            Destroy(collision.gameObject);
            InstantiateTail();   
        }
        if(collision.tag == "MassBurner")
        {
            Destroy(collision.gameObject);
            DecreaseTail();
        }
        if(collision.tag == "Tail")
        {
            SceneManager.LoadScene(0);
        }
    }
}
