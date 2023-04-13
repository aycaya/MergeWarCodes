using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimationEvent : MonoBehaviour
{
    Wizard wizardScript;
    void Start()
    {
        wizardScript = GetComponentInParent<Wizard>();

    }

    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
    }

    public void WizardAttackEvent()
    {
        wizardScript.WizardAttackAnimation();
    }
}
