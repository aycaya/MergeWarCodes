using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Healer : MonoBehaviour
{
    EnemyListOnField enemyListOnField;
    HeroListOnField heroListOnField;

    [SerializeField] float healerAttackStopDistance = 1f;
    [SerializeField] float healStopDistance = 3f;

    float coolDownTime = 5f;
    [SerializeField] float healcoolDownTime = 5f;
    [SerializeField] float attackcoolDownTime = 5f;

    //float swordCDT = 5f;
    float healCDT = 5f;
    float attackCDT = 5f;

    float timer = 0f;
    bool attacked = false;
    bool coolingdown = false;
    bool attackcoolingdown = false;


    [SerializeField] float healRangeVal = 1f;
    float rangeVal = 5f;

    Animator animator;
    GameObject Target;
    bool timerStart = false;
    EnemyHealth selectedEnemy = null;
    GameObject selectedCharacter;
    [SerializeField] int healUpVal;
    [SerializeField] int healerDamage;
    AnimatorHandler animatorHandler;
    CharacterSkills characterSkills;
    NavMeshAgent navmeshAgent;
    private NavMeshPath path;

    void Start()
    {
        UpdateSkills();
        enemyListOnField = FindObjectOfType<EnemyListOnField>();
        heroListOnField = FindObjectOfType<HeroListOnField>();

        navmeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

        animatorHandler = GetComponent<AnimatorHandler>();

        animator = GetComponent<Animator>();
        Target = GameObject.FindGameObjectWithTag("EnemyTower");

    }


    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }
        if (!characterSkills.isItOnStage)
        {
            return;
        }
        if (navmeshAgent.velocity.magnitude > 0.15f)
        {
            animatorHandler.AnimatorWalk();
        }
        else if (navmeshAgent.velocity.magnitude <= 0.15f)
        {
            animatorHandler.AnimatorIdle();

        }
        if (heroListOnField.GetClosestHero(transform.position, out float distance, true) != null && navmeshAgent.enabled)
        {
            selectedCharacter = heroListOnField.GetClosestHero(transform.position, out float distance2, true).gameObject;
            transform.LookAt(selectedCharacter.transform);
            if (distance2 > healStopDistance)
            {
                navmeshAgent.isStopped = false;

                NavMesh.CalculatePath(navmeshAgent.gameObject.transform.position, selectedCharacter.transform.position, NavMesh.AllAreas, path);
                navmeshAgent.SetPath(path);
            }
            else
            {
                navmeshAgent.isStopped = true;
                navmeshAgent.velocity = Vector3.zero;
                navmeshAgent.ResetPath();

            }

        }
        else if (enemyListOnField.GetClosestEnemy(transform.position, out float distance3) != null)
        {
            if (distance3 < healerAttackStopDistance)
            {
                selectedEnemy = enemyListOnField.GetClosestEnemy(transform.position, out float distance7);

                if (!attackcoolingdown && Vector3.Distance(transform.position, selectedEnemy.transform.position) < healerAttackStopDistance && navmeshAgent.enabled)
                {
                    transform.LookAt(selectedEnemy.transform);
                    navmeshAgent.isStopped = true;
                    navmeshAgent.velocity = Vector3.zero;
                    navmeshAgent.ResetPath();
                    animatorHandler.AnimatorAttack();
                    attackcoolingdown = true;

                }
                else if (attackcoolingdown && Vector3.Distance(transform.position, selectedEnemy.transform.position) < healerAttackStopDistance && navmeshAgent.enabled)
                {
                    transform.LookAt(selectedEnemy.transform);
                    navmeshAgent.isStopped = false;

                    animatorHandler.AnimatorIdle();

                }
            }
            else
            {
                return;
            }


        }



        if (!coolingdown && selectedCharacter != null && heroListOnField.GetClosestHero(transform.position, out float distance4, true) != null)
        {
            if (Vector3.Distance(transform.position, selectedCharacter.transform.position) < healStopDistance && navmeshAgent.enabled)
            {
                navmeshAgent.isStopped = true;
                navmeshAgent.velocity = Vector3.zero;
                navmeshAgent.ResetPath();

                animatorHandler.AnimatorHeal();


                coolingdown = true;
            }
            else if (navmeshAgent.enabled)
            {
                transform.LookAt(selectedCharacter.transform);
                navmeshAgent.isStopped = false;

                NavMesh.CalculatePath(navmeshAgent.gameObject.transform.position, selectedCharacter.transform.position, NavMesh.AllAreas, path);
                navmeshAgent.SetPath(path);


            }
        }
        else
        {

            healCDT -= Time.deltaTime;

            if (healCDT <= 0)
            {
                coolingdown = false;
                healCDT = healcoolDownTime;
            }

        }

        if (attackcoolingdown)
        {
            attackCDT -= Time.deltaTime;

            if (attackCDT <= 0)
            {
                attackcoolingdown = false;
                attackCDT = attackcoolDownTime;
            }
        }

    }
    void MoveTowardsTargetFunc(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {

        if (selectedCharacter != null && Vector3.Distance(thisTransform.position, selectedCharacter.transform.position) < healStopDistance)
        {
            animatorHandler.AnimatorIdle();

        }
        else if (Vector3.Distance(thisTransform.position, Target.transform.position) < healerAttackStopDistance)
        {
            animatorHandler.AnimatorIdle();

        }
        else
        {

            animatorHandler.AnimatorWalk();


            thisTransform.position = Vector3.MoveTowards(thisTransform.position, endPos, Time.deltaTime * .5f);



        }
    }
    public void HealerHealUp()
    {
        transform.LookAt(selectedCharacter.transform);


        selectedCharacter.GetComponent<CharacterHealth>().HealUp((int)characterSkills.DamageDontTouch);

    }
    public void HealerAnimationAttack()
    {
        if (selectedEnemy != null && Vector3.Distance(transform.position, selectedEnemy.transform.position) < healerAttackStopDistance)
        {
            transform.LookAt(selectedEnemy.transform);

            selectedEnemy.GetComponent<EnemyHealth>().GetAttacked((int)characterSkills.DamageDontTouch);

        }
        else if (Target.GetComponent<TowerHealth>() != null && Vector3.Distance(transform.position, Target.transform.position) < healerAttackStopDistance)
        {
            transform.LookAt(Target.transform);

            Target.GetComponent<TowerHealth>().GetAttacked((int)characterSkills.DamageDontTouch);

        }

    }

    public void UpdateSkills()
    {
        characterSkills = GetComponent<CharacterSkills>();
        attackcoolDownTime = attackcoolDownTime / characterSkills.AttackSpeedDontTouch;
        healcoolDownTime = attackcoolDownTime;
        attackCDT = attackcoolDownTime;
        healCDT = attackcoolDownTime;
    }


}
