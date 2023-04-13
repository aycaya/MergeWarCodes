using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAnimationEvent : MonoBehaviour
{
    Healer healerScript;
    void Start()
    {
        healerScript = GetComponentInParent<Healer>();
    }

    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
    }
    public void HealerHealUpEvent()
    {
        healerScript.HealerHealUp();
    }
    public void HealerAttackEvent()
    {
        healerScript.HealerAnimationAttack();
    }
}
