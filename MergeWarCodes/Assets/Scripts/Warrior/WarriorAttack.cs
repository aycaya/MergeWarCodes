using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarriorAttack : MonoBehaviour
{
    bool enemyFound = false;
    EnemyListOnField enemyListOnField;
    AudioPlayer audioPlayer;

    [SerializeField] float warriorStopDistance = 2f;

    NavMeshAgent navmeshAgent;
    Transform closestEnemy;

    bool enemyContact;
    float coolDownTime = 5f;
    [SerializeField] float swordcoolDownTime = 5f;

    float swordCDT = 5f;
    List<GameObject> enemyList = new List<GameObject>();
    float rangeVal = 2f;

    float timer = 0f;
    bool attacked = false;
    bool coolingdown = false;

    [SerializeField] float swordForce = 20f;
    GameObject Target;
    bool goToTarget = true;
    EnemyHealth selectedEnemy = null;
    Animator animator;
    AnimatorHandler animatorHandler;
    CharacterSkills characterSkills;
    private NavMeshPath path;

    void Start()
    {
        UpdateSkills();
        enemyListOnField = FindObjectOfType<EnemyListOnField>();

        animatorHandler = GetComponent<AnimatorHandler>();
        navmeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

        animator = GetComponent<Animator>();
        closestEnemy = null;
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


        if (enemyListOnField.GetClosestEnemy(transform.position, out float distance) == null && Target && navmeshAgent.enabled)
        {
            transform.LookAt(Target.transform);


            if (!coolingdown && Vector3.Distance(transform.position, Target.transform.position) < warriorStopDistance && navmeshAgent.enabled)
            {
                navmeshAgent.isStopped = true;
                navmeshAgent.velocity = Vector3.zero;
                navmeshAgent.ResetPath();
                navmeshAgent.SetDestination(navmeshAgent.transform.position);

                animatorHandler.AnimatorAttack();

                coolingdown = true;

            }
            else if (Vector3.Distance(transform.position, Target.transform.position) < warriorStopDistance && navmeshAgent.enabled)
            {
                navmeshAgent.isStopped = true;
                navmeshAgent.velocity = Vector3.zero;
                navmeshAgent.ResetPath();
                navmeshAgent.SetDestination(navmeshAgent.transform.position);
                animatorHandler.AnimatorIdle();

            }
            else if (navmeshAgent.enabled)
            {
                navmeshAgent.isStopped = false;

                NavMesh.CalculatePath(navmeshAgent.gameObject.transform.position, Target.transform.position, NavMesh.AllAreas, path);
                navmeshAgent.SetPath(path);
            }
        }
        else if (!enemyFound && navmeshAgent.enabled)
        {
            if (enemyListOnField.GetClosestEnemy(transform.position, out float distance3) && navmeshAgent.enabled)
            {
                selectedEnemy = enemyListOnField.GetClosestEnemy(transform.position, out float distance2);
                transform.LookAt(selectedEnemy.transform);
                navmeshAgent.isStopped = false;

                NavMesh.CalculatePath(navmeshAgent.gameObject.transform.position, selectedEnemy.transform.position, NavMesh.AllAreas, path);
                navmeshAgent.SetPath(path);

                enemyFound = true;
            }

        }


        if (!coolingdown && selectedEnemy != null)
        {
            if (Vector3.Distance(transform.position, selectedEnemy.transform.position) < warriorStopDistance && navmeshAgent.enabled)
            {
                navmeshAgent.isStopped = true;

                navmeshAgent.velocity = Vector3.zero;
                navmeshAgent.ResetPath();
                navmeshAgent.SetDestination(navmeshAgent.transform.position);

                animatorHandler.AnimatorAttack();

                coolingdown = true;
            }
            else if (navmeshAgent.enabled)
            {
                transform.LookAt(selectedEnemy.transform);
                navmeshAgent.isStopped = false;

                NavMesh.CalculatePath(navmeshAgent.gameObject.transform.position, selectedEnemy.transform.position, NavMesh.AllAreas, path);
                navmeshAgent.SetPath(path);


            }
        }
        else
        {

            swordCDT -= Time.deltaTime;

            if (swordCDT <= 0)
            {
                coolingdown = false;
                swordCDT = swordcoolDownTime;
            }

        }
        if (selectedEnemy != null && selectedEnemy.IsDead) { enemyFound = false; }

    }
    void MoveTowardsTargetFunc(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {

        if (Vector3.Distance(thisTransform.position, endPos) < warriorStopDistance)
        {
            animatorHandler.AnimatorIdle();

        }
        else
        {

            animatorHandler.AnimatorWalk();


            thisTransform.position = Vector3.MoveTowards(thisTransform.position, endPos, Time.deltaTime * .5f);



        }
    }
    public void WarriorAnimationAttack()
    {
        if (selectedEnemy != null)
        {
            transform.LookAt(selectedEnemy.transform);

            selectedEnemy.GetComponent<EnemyHealth>().GetAttacked((int)characterSkills.DamageDontTouch);
            if (audioPlayer == null)
            {
                audioPlayer = FindObjectOfType<AudioPlayer>();
            }
            audioPlayer.PlayTowerSound();
        }
        else if (Target && Target.GetComponent<TowerHealth>().enabled)
        {
            transform.LookAt(Target.transform);

            Target.GetComponent<TowerHealth>().GetAttacked((int)characterSkills.DamageDontTouch);
            if (audioPlayer == null)
            {
                audioPlayer = FindObjectOfType<AudioPlayer>();
            }
            audioPlayer.PlayTowerSound();
        }
    }

    IEnumerator MoveTowardsTarget(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            if (Vector3.Distance(transform.position, endPos) < 1.5f)
            {
                animatorHandler.AnimatorIdle();

                break;


            }
            else
            {
                animatorHandler.AnimatorWalk();

                yield return new WaitForEndOfFrame();
                i = i + (Time.deltaTime * rate);
                thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            }

        }
        yield return null;
    }

    public void UpdateSkills()
    {
        characterSkills = GetComponent<CharacterSkills>();
        swordcoolDownTime = swordcoolDownTime / characterSkills.AttackSpeedDontTouch;
        swordCDT = swordcoolDownTime;
    }

}
