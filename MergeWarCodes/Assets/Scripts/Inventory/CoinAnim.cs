using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnim : MonoBehaviour
{
    Vector3 StartPos;

    bool FirstPhase = true;

    public Vector3 FirstPhaseVector;
    public Vector3 SecondPhaseVector;

    GameObject OurCanvas;

    private float SpeedAnim = 6f;

    public GameObject CoinUI;

    private void Awake()
    {
        StartPos = transform.position;
        Vector2 rnd = (Random.insideUnitCircle * 2f);
        FirstPhaseVector = transform.position + new Vector3(rnd.x, 1, rnd.y);
        OurCanvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
        gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, FirstPhaseVector, Time.deltaTime * SpeedAnim);
        if (Vector3.Distance(gameObject.transform.position, FirstPhaseVector) < 0.01f)
        {
            SecondPhaseVector = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            GameObject tmp = Instantiate(CoinUI, OurCanvas.transform);
            tmp.GetComponent<RectTransform>().position = SecondPhaseVector;
            Destroy(gameObject);
        }
    }
}
