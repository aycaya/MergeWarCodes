using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public bool isHealer = false;
    [HideInInspector] public int characterHealthVal = 100;
    [SerializeField] int characterHv = 100;
    int healthLevel = 0;
    int healthConstant;
    [SerializeField] int healthUpgradeIncrement = 25;
    bool isDead = false;
    [SerializeField] Slider characterHealthBar;
    AnimatorHandler animatorHandler;
    [SerializeField] GameObject characterCanvas;
    Camera _cam;
    private CharacterSkills characerSkills;
    public ParticleSystem HealParticlePrefab;
    GameObject DamageInfoPrefab;
    public int MaximumHealth
    {
        get { return healthConstant; }
    }
    public int CurrentHealth
    {
        get
        {
            return characterHealthVal;
        }
    }
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }
    void Start()
    {
        DamageInfoPrefab = Resources.Load<GameObject>("DamageInfo");
        animatorHandler = GetComponent<AnimatorHandler>();

        _cam = Camera.main;
        characerSkills = GetComponent<CharacterSkills>();
        characterHealthVal = (int)characerSkills.HealthDontTouch;
        healthConstant = characterHealthVal;

    }

    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished)
        {
            return;
        }
        if (characterHealthVal <= 0 && !isDead)
        {
            PlayerDeath();


        }
        characterHealthBar.value = ((float)characterHealthVal / (float)healthConstant);
        characterCanvas.transform.rotation = Quaternion.LookRotation(characterCanvas.transform.position - _cam.transform.position);


    }
    public void UpgradeHealth(float newHealth)
    {
        characterHealthVal = (int)newHealth;
        healthConstant = characterHealthVal;

    }
    public void GetAttacked(int param)
    {
        Instantiate(DamageInfoPrefab, transform.position + (Vector3.right * 1f) + (Vector3.up * 0.4f), quaternion.identity).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = param.ToString();
        characterHealthVal -= param;
        GetComponent<FlashEffect>().Flash();
    }
    public void HealUp(int param)
    {
        Instantiate(DamageInfoPrefab, transform.position + (Vector3.right * 1f) + (Vector3.up * 0.4f), quaternion.identity).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = param.ToString();

        if (characterHealthVal + param <= healthConstant)
        {
            characterHealthVal += param;
        }

        HealParticlePrefab.Clear();
        HealParticlePrefab.Play();
    }

    void PlayerDeath()
    {

        animatorHandler.AnimatorDeath();

        isDead = true;
        StartCoroutine(WaitAndDestroy());
    }
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);

    }
}
