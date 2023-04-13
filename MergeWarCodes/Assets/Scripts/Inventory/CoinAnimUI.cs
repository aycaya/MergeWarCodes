using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnimUI : MonoBehaviour
{
    Vector3 StartPos;

    bool FirstPhase = false;

    public bool isGem = false;

    public Vector3 FirstPhaseVector;

    GameObject CoinPos;
    RectTransform selfRect;
    Inventory inventory;
    Image image;

    private float SpeedAnim = 8f;

    private void Awake()
    {
        if (!isGem)
            CoinPos = GameObject.FindGameObjectWithTag("CoinPosUI");
        else
            CoinPos = GameObject.FindGameObjectWithTag("GemPosUI");

        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        FirstPhaseVector = CoinPos.GetComponent<RectTransform>().position;
        selfRect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        StartAnim();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
        if (FirstPhase)
        {
            selfRect.position = Vector3.Lerp(selfRect.position, FirstPhaseVector, Time.deltaTime * SpeedAnim);
        }
        float distance = Vector3.Distance(selfRect.position, FirstPhaseVector);
        if (distance < 60f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, (distance / 200f));
        }
        if (image.color.a < 0.1f)
        {
            if (!isGem)
            {
                inventory.ChangeCoin(5);
                inventory.LevelsCoinIncome += 5;
            }
            else
            {
                inventory.ChangeGem(5);
                inventory.LevelsGemIncome += 5;
            }

            Destroy(gameObject);
        }
    }

    public void StartAnim()
    {
        FirstPhase = true;
    }
}
