using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int score;
    private bool isScoreBoosterActive = false;
    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        ResetUI();
    }

    public void IncreaseScore(int increment)
    {
        if (isScoreBoosterActive)
        {
            increment *= 2;  
        }
        score += increment;
        ResetUI();
    }

    public void DecreaseScore(int decrement)
    {
        score -= decrement;
        ResetUI();
    }

    private void ResetUI()
    {
        scoreText.text = "Score: " + score;
    }

    public void ActivateScoreBooster(float duration)
    {
        isScoreBoosterActive = true;
        StartCoroutine(DeactivateScoreBooster(duration));
    }

    private IEnumerator DeactivateScoreBooster(float duration)
    {
        yield return new WaitForSeconds(duration);
        isScoreBoosterActive = false;
    }
}
