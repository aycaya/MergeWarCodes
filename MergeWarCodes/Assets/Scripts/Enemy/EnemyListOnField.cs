using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListOnField : MonoBehaviour
{
    public List<EnemyHealth> enemyHealthsOnField = new List<EnemyHealth>();

    public void AddNewEnemy(EnemyHealth incomingEnemyHealth)
    {
        enemyHealthsOnField.Add(incomingEnemyHealth);
    }
    private void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished)
        {
            return;
        }
    }
    public EnemyHealth GetClosestEnemy(Vector3 sourcePosition, out float distance, bool excludeFullHealth = false)
    {
        distance = 999999999999999999;
        EnemyHealth enemyToBeReturned = null;
        if (enemyHealthsOnField.Count < 1)
        {
            return null;
        }
        for (int i = 0; i < enemyHealthsOnField.Count; i++)
        {
            if (enemyHealthsOnField[i] == null)
            {
                continue;
            }
            if (!enemyHealthsOnField[i].gameObject.activeInHierarchy)
            {
                continue;
            }
            if (enemyHealthsOnField[i].IsDead)
            {
                continue;
            }
            if (excludeFullHealth)
            {
                if (enemyHealthsOnField[i].MaximumHealth == enemyHealthsOnField[i].CurrentHealth)
                {
                    continue;
                }
            }
            float currentDistance = Vector3.Distance(sourcePosition, enemyHealthsOnField[i].transform.position);
            if (distance > currentDistance)
            {
                distance = currentDistance;
                enemyToBeReturned = enemyHealthsOnField[i];
            }
        }
        return enemyToBeReturned;
    }
}
