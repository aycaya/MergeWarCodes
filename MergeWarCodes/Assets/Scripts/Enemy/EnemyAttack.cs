using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    HeroListOnField heroListOnField;
    GameObject Tower;
    GameObject selectedCharacter;
    EnemyAI enemyAI;

    [SerializeField] int enemyAttackNormalVal = 5;
    [SerializeField] int enemyAttackBuffPerLevel = 1;
    void Start()
    {
        enemyAttackNormalVal += enemyAttackBuffPerLevel * PlayerPrefs.GetInt("Level", 1);
        heroListOnField = FindObjectOfType<HeroListOnField>();
        Tower = GameObject.FindGameObjectWithTag("Tower");
        enemyAI = gameObject.GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished)
        {
            return;
        }


    }
    public void EnemyAttackNormal()
    {
        if (heroListOnField.GetClosestHero(transform.position, out float distance) != null)
        {
            selectedCharacter = heroListOnField.GetClosestHero(transform.position, out float distance2).gameObject;
        }
        else
        {
            distance = Mathf.Infinity;
        }

        if (Tower)
        {
            float distanceToTower = Vector3.Distance(transform.position, Tower.transform.position);

            if (distanceToTower < distance && enemyAI.hitRange > distanceToTower)
            {
                Tower.GetComponent<TowerHealth>().GetAttacked(enemyAttackNormalVal);
                transform.LookAt(Tower.transform);
            }
            else if (selectedCharacter && enemyAI.hitRange > distance)
            {
                selectedCharacter.GetComponent<CharacterHealth>().GetAttacked(enemyAttackNormalVal);
                transform.LookAt(selectedCharacter.transform);
            }
        }
    }
}
