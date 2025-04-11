using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public Slider progressSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       progressAmount = 0;
        progressSlider. value = 0;
        Coin.OnCoinCollect += IncreaseProgressAmount;
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        if(progressAmount >= 100)
        {
            Debug.Log("Level Complete");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
