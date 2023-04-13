using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    private List<GameObject> Levels;
    private void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
    }
    private void Awake()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1));
        Levels = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Levels.Add(transform.GetChild(i).gameObject);
        }

        if (Levels.Count > PlayerPrefs.GetInt("Level", 1))
        {
            Levels[PlayerPrefs.GetInt("Level", 1) - 1].SetActive(true);
        }
        else
        {
            Levels[Levels.Count - 1].SetActive(true);
        }

    }
}
