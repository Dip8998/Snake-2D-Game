using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(Restart);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(Quit);
    }
    private void Restart()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        Time.timeScale = 1.0f;
    }
    private void MainMenu()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(0);
    }
    private void Quit()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        Application.Quit();
    }

   

}
