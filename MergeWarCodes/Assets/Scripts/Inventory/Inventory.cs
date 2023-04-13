using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public bool win = false;
    float tmpCoin;
    float tmpGem;
    [HideInInspector] public float LevelsCoinIncome;
    [HideInInspector] public float LevelsGemIncome;
    private float Coin;
    private string CoinID = "Coin";

    private float Gem;
    private string GemID = "Gem";

    TextMeshProUGUI CoinText;
    TextMeshProUGUI GemText;

    public float StartCoinPerSecond = 1;
    float t = 1;
    float startCoinValue;
    private void Start()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            PlayerPrefs.SetFloat(CoinID, 400);
            Coin = PlayerPrefs.GetFloat(CoinID, 400);
        }
        else if (PlayerPrefs.GetInt("Tutorial", 0) == 1)
        {
            PlayerPrefs.SetFloat(CoinID, 200);
            Coin = PlayerPrefs.GetFloat(CoinID, 200);
        }

        Gem = PlayerPrefs.GetFloat(GemID, 0);

        CoinText = GameObject.FindGameObjectWithTag("CoinPosUI").GetComponent<TextMeshProUGUI>();
        CoinText.text = Coin.ToString();
        GemText = GameObject.FindGameObjectWithTag("GemPosUI").GetComponent<TextMeshProUGUI>();
        GemText.text = Gem.ToString();

        tmpCoin = Coin;
        tmpGem = Gem;
        LevelsCoinIncome = 0;
        LevelsGemIncome = 0;
    }

    private void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {

            return;
        }
        t -= Time.deltaTime;
        if (t < 0) { ChangeCoin(PlayerPrefs.GetFloat("CoinPerSecond", StartCoinPerSecond)); t = 1; }

    }

    public void ChangeCoin(float amount)
    {
        Coin += amount;

        PlayerPrefs.SetFloat(CoinID, Coin);

        CoinText.text = ((int)Coin).ToString();
    }

    public void ChangeGem(float amount)
    {
        Gem += amount;

        PlayerPrefs.SetFloat(GemID, Gem);
        GemText.text = ((int)Gem).ToString();
    }

    public float GetCoin()
    {
        return Coin;
    }

    public float GetGem()
    {
        return Gem;
    }
}
