using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationEvent : MonoBehaviour
{
    ArcherAttack archerAttack;

    void Start()
    {
        archerAttack = GetComponentInParent<ArcherAttack>();

    }

    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
    }
    public void ArcherAttackEvent()
    {
        archerAttack.AttackAnimationStart();
    }
}
