using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherAttack : MonoBehaviour
{


    bool enemyFound = false;
    EnemyListOnField enemyListOnField;
    NavMeshAgent navmeshAgent;
    [SerializeField] float archerStopDistance = 4f;


    bool goToTarget = true;
    Transform closestEnemy;
    public bool enemyContact;
    float coolDownTime = 5f;
    [SerializeField] float arrowcoolDownTime = 5f;

    //float swordCDT = 5f;
    float arrowCDT = 5f;

    float timer = 0f;
    bool attacked = false;
    bool coolingdown = false;

    [SerializeField] float arrowForce = 20f;
    [SerializeField] Transform arrowPos;
    List<GameObject> enemyList = new List<GameObject>();
    float rangeVal = 5f;
    [SerializeField] GameObject ArrowPrefab;
    Animator animator;
    GameObject Target;
    bool timerStart = false;
    [HideInInspector] public EnemyHealth selectedEnemy = null;
    AnimatorHandler animatorHandler;
    CharacterSkills characterSkills;
    private NavMeshPath path;
    Rigidbody rb;
    void Start()
    {
        UpdateSkills();
        enemyListOnField = FindObjectOfType<EnemyListOnField>();
        animator = GetComponent<Animator>();
        animatorHandler = GetComponent<AnimatorHandler>();
        navmeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        rb = GetComponent<Rigidbody>();
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
            //animatorHandler.AnimatorWalk();
            if (Vector3.Distance(transform.position, Target.transform.position) > archerStopDistance)
            {

                navmeshAgent.isStopped = false;
                NavMesh.CalculatePath(navmeshAgent.gameObject.transform.position, Target.transform.position, NavMesh.AllAreas, path);
                navmeshAgent.SetPath(path);

            }
            else
            {
                navmeshAgent.isStopped = true;
                navmeshAgent.velocity = Vector3.zero;
                navmeshAgent.ResetPath();
                navmeshAgent.SetDestination(navmeshAgent.transform.position);
                animatorHandler.AnimatorIdle();


            }



            if (!coolingdown && Vector3.Distance(transform.position, Target.transform.position) < archerStopDistance && navmeshAgent.enabled)
            {

                navmeshAgent.isStopped = true;
                navmeshAgent.velocity = Vector3.zero;
                navmeshAgent.ResetPath();
                navmeshAgent.SetDestination(navmeshAgent.transform.position);

                animatorHandler.AnimatorAttack();

                coolingdown = true;

            }
        }
        else if (enemyListOnField.GetClosestEnemy(transform.position, out float distance8) != null && !enemyFound && navmeshAgent.enabled)
        {

            selectedEnemy = enemyListOnField.GetClosestEnemy(transform.position, out float distance2);

            if (selectedEnemy)
            {
                transform.LookAt(selectedEnemy.transform);

                navmeshAgent.isStopped = false;
                NavMesh.CalculatePath(navmeshAgent.gameObject.transform.position, selectedEnemy.transform.position, NavMesh.AllAreas, path);
                navmeshAgent.SetPath(path);

                enemyFound = true;


            }


        }
        if (!coolingdown && selectedEnemy != null)
        {
            if (Vector3.Distance(transform.position, selectedEnemy.transform.position) < archerStopDistance && navmeshAgent.enabled)
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

            arrowCDT -= Time.deltaTime;

            if (arrowCDT <= 0)
            {
                coolingdown = false;
                arrowCDT = arrowcoolDownTime;
            }

        }
        if (selectedEnemy != null && selectedEnemy.IsDead) { enemyFound = false; }

    }

    void MoveTowardsTargetFunc(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {

        if (Vector3.Distance(thisTransform.position, endPos) < archerStopDistance)
        {
            animatorHandler.AnimatorIdle();

        }
        else
        {

            animatorHandler.AnimatorWalk();


            thisTransform.position = Vector3.MoveTowards(thisTransform.position, endPos, Time.deltaTime * .5f);



        }
    }
    IEnumerator MoveTowardsTarget(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            if (Vector3.Distance(transform.position, endPos) < archerStopDistance)
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
    public void AttackAnimationStart()
    {

        if (selectedEnemy != null)
        {

            transform.LookAt(selectedEnemy.transform);

            GameObject arrow = Instantiate(ArrowPrefab, arrowPos.position, arrowPos.rotation);
            arrow.GetComponent<ArrowMovement>().target = selectedEnemy;
            arrow.GetComponent<ArrowMovement>().damage = (int)characterSkills.DamageDontTouch;
        }
        else if (Target != null)
        {
            GameObject arrow = Instantiate(ArrowPrefab, arrowPos.position, arrowPos.rotation);
            arrow.GetComponent<ArrowMovement>().damage = (int)characterSkills.DamageDontTouch;


        }

    }
    public Transform GetClosestEnemy()
    {

        float closestDist = Mathf.Infinity;
        Transform trans = null;
        foreach (GameObject go in enemyList)
        {
            float currentDist;
            if (go == null)
            {
                continue;
            }
            currentDist = Vector3.Distance(transform.position, go.transform.position);
            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                trans = go.transform;
            }
        }
        if (rangeVal > closestDist)
        {
            return trans;

        }
        else
        {
            goToTarget = true;

            return null;

        }
    }

    public void UpdateSkills()
    {
        characterSkills = GetComponent<CharacterSkills>();
        arrowcoolDownTime = arrowcoolDownTime / characterSkills.AttackSpeedDontTouch;
        arrowCDT = arrowcoolDownTime;
    }
}
