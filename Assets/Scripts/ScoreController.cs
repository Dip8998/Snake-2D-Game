using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI snake1ScoreText; 
    [SerializeField] private TextMeshProUGUI snake2ScoreText; 

    private int snake1Score = 0;
    private int snake2Score = 0;

    private void Start()
    {
        ResetUI();
    }

    public void IncreaseScore(bool isSnake1, int increment)
    {
        if (isSnake1)
        {
            snake1Score += increment;
        }
        else
        {
            snake2Score += increment;
        }
        ResetUI();
    }

    public void DecreaseScore(bool isSnake1, int decrement)
    {
        if (isSnake1)
        {
            if(snake1Score > 0)
            {
                snake1Score -= decrement;
            }
        }
        else
        {
            if(snake2Score > 0)
            {
                snake2Score -= decrement;
            }
        }
        ResetUI();
    }

    private void ResetUI()
    {
        snake1ScoreText.text = "Score: " + snake1Score;
        snake2ScoreText.text = "Score: " + snake2Score;
    }

    public int GetScore(bool isSnake1)
    {
        return isSnake1 ? snake1Score : snake2Score;
    }
}
