using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    Transform gameOverPanel;

    public static GameMode Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        gameOverPanel = canvas.transform.Find("GameOverPanel");

        gameOverPanel.gameObject.SetActive(false);
    }
    public void OnPlayerDie()
    {
        gameOverPanel.gameObject.SetActive(true);
    }
    public void OnRestart()
    {
        SceneManager.LoadScene("Game");
    }
}
