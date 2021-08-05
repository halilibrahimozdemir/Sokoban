using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Box[] _boxes;
    private Area[] _areas;
    public bool isLevelFinished = false;
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject gameOverScreen;
    public GameObject nextLevelScreen;
    public Text levelCountText;
    public int levelCount;

    public static GameManager instance;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    private void Start()
    {
        _boxes = FindObjectsOfType<Box>();
        _areas = FindObjectsOfType<Area>();
        mainMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!isLevelFinished)
        {
            nextLevelScreen.SetActive(false);
            CheckIfFinished();
        }

        if (isLevelFinished)
        {
            nextLevelScreen.SetActive(true);
            Time.timeScale = 0f;
            isLevelFinished = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Pause();
        }
    }

    private void CheckIfFinished()
    {
        foreach (var area in _areas)
        {
            if(!area.amIDone) return;
        }
        
        isLevelFinished = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        mainMenu.SetActive(false);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        nextLevelScreen.SetActive(false);
        levelCount++;
        SceneManager.LoadScene(levelCount);
    }

    public void ReLoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
