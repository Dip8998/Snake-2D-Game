using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        mainMenuButton.onClick.AddListener(MainMenu);
    }
    private void Pause()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }
    private void Resume()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        Time.timeScale = 1f;
        pausePanel?.SetActive(false);
    }
    private void Restart()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        Time.timeScale = 1f;
    }
    private void MainMenu()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(0);
    }
}
