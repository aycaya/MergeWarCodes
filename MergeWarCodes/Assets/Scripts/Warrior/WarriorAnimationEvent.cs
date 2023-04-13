using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAnimationEvent : MonoBehaviour
{
    WarriorAttack warriorAttack;
    // Start is called before the first frame update
    void Start()
    {
        warriorAttack = GetComponentInParent<WarriorAttack>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
    }
    public void WarriorAttackEvent()
    {
        warriorAttack.WarriorAnimationAttack();
    }
}
