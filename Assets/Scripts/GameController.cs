using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static event Action OnReset;
    public static event Action OnLevelChanged;

    int progressAmount;
    public Slider progressSlider;

    public GameObject player;
    public GameObject loadCanvas;
    public GameObject[] levels;
    private int currentLevelIndex = 0;

    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    public int survivedLevelsCount;

    private void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        Coin.OnCoinCollect += IncreaseProgressAmount;
        HoldToLoad.OnHoldComplet += LoadNextLevel;
        PlayerHealth.OnPlayedDied += GameOverScreen;

        loadCanvas.SetActive(false);
        gameOverScreen.SetActive(false);

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(i == currentLevelIndex);
        }
    }

    private void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        if (progressAmount >= 100)
        {
            loadCanvas.SetActive(true);
            Debug.Log("Level Complete");
        }
    }

    private void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex + 1) % levels.Length;
        LoadLevel(nextLevelIndex, true);
    }

    private void LoadLevel(int level, bool increaseSurvivedCount)
    {
        loadCanvas.SetActive(false);

        levels[currentLevelIndex].SetActive(false);
        levels[level].SetActive(true);

        player.transform.position = Vector3.zero;

        currentLevelIndex = level;
        progressAmount = 0;
        progressSlider.value = 0;

        if (increaseSurvivedCount)
            survivedLevelsCount++;

        OnLevelChanged?.Invoke();
    }

    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        survivedText.text = "You Survived " + survivedLevelsCount + " Level" + (survivedLevelsCount != 1 ? "s" : "");
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        survivedLevelsCount = 0;
        LoadLevel(0, false);
        OnReset?.Invoke();
        Time.timeScale = 1;
    }
}