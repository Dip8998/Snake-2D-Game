using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiPlayerButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        singlePlayerButton.onClick.AddListener(SinglePlayer);
        multiPlayerButton.onClick.AddListener(MultiPlayer);
        quitButton.onClick.AddListener(Quit);
    }

    private void SinglePlayer()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    private void MultiPlayer()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(2);
        Time.timeScale = 1.0f;
    }

    private void Quit()
    {
        SoundController.Instance.Play(Sounds.ButtonClick);
        Application.Quit();
    }
}
