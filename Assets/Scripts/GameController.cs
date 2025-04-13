using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public Slider progressSlider;

    public GameObject player;
    public GameObject loadCanvas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    public int survivedLevelsCount;

    public static event Action OnReset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressAmount = 0;
        progressSlider. value = 0;
        Coin.OnCoinCollect += IncreaseProgressAmount;
        HoldToLoad.OnHoldComplet += LoadNextLevel;
        PlayerHealth.OnPlayedDied += GameOverScreen;
        loadCanvas.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        survivedText.text = "You Survived " + survivedLevelsCount + " Level";
        if (survivedLevelsCount != 1) survivedText.text += "s";
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        survivedLevelsCount = 0;
        LoadLevel(0, false);
        OnReset.Invoke();
        Time.timeScale = 1;
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        if(progressAmount >= 100)
        {
            loadCanvas.SetActive(true);
            Debug.Log("Level Complete");
        }
    }

    void LoadLevel(int level, bool wantSurvivedIncrease)
    {
        loadCanvas.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[level].gameObject.SetActive(true);

        player.transform.position = new Vector3(0, 0, 0);

        currentLevelIndex = level;
        progressAmount = 0;
        progressSlider.value = 0;
        if(wantSurvivedIncrease) survivedLevelsCount++;
    }
    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
