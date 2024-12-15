using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2 snakeDir = Vector2.right;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            snakeDir = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            snakeDir = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            snakeDir = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            snakeDir = Vector2.right;
        }

    }

    private void FixedUpdate()
    { 
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x)+snakeDir.x,
            Mathf.Round(this.transform.position.y) + snakeDir.y,
            0.0f
            );
    }
}
