using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int enemyHealthVal = 100;
    [SerializeField] private int HealthBuffPerLevel = 10;
    int enemyHv = 100;
    bool isEnemyDead = false;
    float deathTimer = 2f;
    [SerializeField] private GameObject CoinPrefab;
    [SerializeField] private GameObject GemPrefab;
    [SerializeField] Slider enemyHealthBar;
    AnimatorHandler animatorHandler;
    [SerializeField] GameObject enemyCanvas;
    GameObject DamageInfoPrefab;
    Camera _cam;
    public int MaximumHealth
    {
        get { return enemyHv; }
    }
    public int CurrentHealth
    {
        get
        {
            return enemyHealthVal;
        }
    }
    public bool IsDead
    {
        get
        {
            return isEnemyDead;
        }
    }
    void Start()
    {
        DamageInfoPrefab = Resources.Load<GameObject>("DamageInfo");
        animatorHandler = GetComponent<AnimatorHandler>();
        enemyHealthVal += HealthBuffPerLevel * PlayerPrefs.GetInt("Level", 1);
        enemyHv = enemyHealthVal;
        _cam = Camera.main;

    }

    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
        if (enemyHealthVal <= 0 && !isEnemyDead)
        {
            isEnemyDead = true;
            GameManager.score++;
            animatorHandler.AnimatorDeath();
            StartCoroutine(WaitAndDestroy());

        }
        enemyHealthBar.value = ((float)enemyHealthVal / (float)enemyHv);
        enemyCanvas.transform.rotation = Quaternion.LookRotation(enemyCanvas.transform.position - _cam.transform.position);
    }
    public void GetAttacked(int param)
    {
        enemyHealthVal -= param;
        Instantiate(DamageInfoPrefab, transform.position + (Vector3.right * 1f) + (Vector3.up * 0.4f), quaternion.identity).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = param.ToString();
        GetComponent<FlashEffect>().Flash();
        animatorHandler.AnimatorHit();
    }

    IEnumerator WaitAndDestroy()
    {
        for (int i = 0; (int)(PlayerPrefs.GetFloat("CoinPerEnemy") / 5f) > i; i++)
        {
            Instantiate(CoinPrefab, transform.position, quaternion.identity);
        }
        for (int i = 0; (int)(PlayerPrefs.GetFloat("GemPerEnemy") / 5f) > i; i++)
        {
            Instantiate(GemPrefab, transform.position, quaternion.identity);
        }
        yield return new WaitForSeconds(deathTimer);

        Destroy(this.gameObject);

    }

}
