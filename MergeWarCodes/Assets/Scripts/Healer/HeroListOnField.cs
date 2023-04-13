using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroListOnField : MonoBehaviour
{
    public List<CharacterHealth> characterHealthsOnField = new List<CharacterHealth>();

    public void AddNewCharacter(CharacterHealth incomingCharacterHealth)
    {
        characterHealthsOnField.Add(incomingCharacterHealth);
    }
    private void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
    }
    public CharacterHealth GetClosestHero(Vector3 sourcePosition, out float distance, bool excludeFullHealth = false)
    {
        distance = 99999f;
        CharacterHealth heroToBeReturned = null;
        if (characterHealthsOnField.Count < 1)
        {
            return null;
        }
        for (int i = 0; i < characterHealthsOnField.Count; i++)
        {
            if (characterHealthsOnField[i] == null)
            {
                continue;
            }
            if (!characterHealthsOnField[i].gameObject.activeInHierarchy)
            {
                continue;
            }
            if (characterHealthsOnField[i].IsDead)
            {
                continue;
            }
            if (excludeFullHealth)
            {
                if (characterHealthsOnField[i].MaximumHealth <= characterHealthsOnField[i].CurrentHealth)
                {
                    continue;
                }
                if (characterHealthsOnField[i].isHealer)
                {
                    continue;
                }
            }
            float currentDistance = Vector3.Distance(sourcePosition, characterHealthsOnField[i].transform.position);
            if (distance > currentDistance)
            {
                distance = currentDistance;
                heroToBeReturned = characterHealthsOnField[i];
            }
        }
        return heroToBeReturned;
    }
}
