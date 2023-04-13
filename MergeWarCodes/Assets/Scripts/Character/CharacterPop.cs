using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPop : MonoBehaviour
{
    public bool FirstPhase = true;
    public bool SecondPhase = false;
    public bool ThirtPhase = false;

    public Vector3 FirstPhaseVector = new Vector3(1.65f, 0.075f, 1.65f);
    public Vector3 SecondPhaseVector = new Vector3(0.6f, 1.7f, 0.6f);
    public Vector3 ThirtPhaseVector = new Vector3(1, 1, 1);

    private float SpeedAnim = 10f;

    bool Pop = true;

    void Awake()
    {
        gameObject.transform.localScale = Vector3.one;
        PopUp();
    }

    private void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished)
        {
            return;
        }
        if (FirstPhase && Pop)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, FirstPhaseVector, Time.deltaTime * SpeedAnim * 1.5f);
            if (Vector3.Distance(gameObject.transform.localScale, FirstPhaseVector) < 0.1f)
            {
                FirstPhase = false;
                SecondPhase = true;
                ThirtPhase = false;
            }
        }

        if (SecondPhase && Pop)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, SecondPhaseVector, Time.deltaTime * SpeedAnim * 1.4f);
            if (Vector3.Distance(gameObject.transform.localScale, SecondPhaseVector) < 0.1f)
            {
                FirstPhase = false;
                SecondPhase = false;
                ThirtPhase = true;
            }
        }

        if (ThirtPhase && Pop)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, ThirtPhaseVector, Time.deltaTime * SpeedAnim * 1.25f);
            if (Vector3.Distance(gameObject.transform.localScale, ThirtPhaseVector) < 0.02f)
            {
                FirstPhase = false;
                SecondPhase = false;
                ThirtPhase = false;
                Pop = false;
            }
        }

    }

    public void PopUp()
    {
        FirstPhase = true;
        SecondPhase = false;
        ThirtPhase = false;
        Pop = true;
    }

}
