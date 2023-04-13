using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wizard : MonoBehaviour
{

    bool enemyFound = false;
    EnemyListOnField enemyListOnField;
    NavMeshAgent navmeshAgent;
    [SerializeField] float wizardStopDistance = 4f;


    bool goToTarget = true;
    Transform closestEnemy;
    public bool enemyContact;
    float coolDownTime = 5f;
    [SerializeField] float wizardCoolDownTime = 5f;

    float wizardCDT = 5f;

    float timer = 0f;
    bool attacked = false;
    bool coolingdown = false;

    [SerializeField] float wizardForce = 20f;
    [SerializeField] Transform wizardThrowPos;
    float rangeVal = 5f;
    [SerializeField] GameObject wizardProjectilePrefab;
    Animator animator;
    GameObject Target;
    bool timerStart = false;
    [HideInInspector] public EnemyHealth selectedEnemy = null;
    AnimatorHandler animatorHandler;
    CharacterSkills characterSkills;
    private NavMeshPath path;

    void Start()
    {
        UpdateSkills();
        enemyListOnField = FindObjectOfType<EnemyListOnField>();
        animator = GetComponent<Animator>();

        animatorHandler = GetComponent<AnimatorHandler>();
        navmeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

        closestEnemy = null;
        Target = GameObject.FindGameObjectWithTag("EnemyTower");

    }

    // Update is called once per frame
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

            if (!coolingdown && Vector3.Distance(transform.position, Target.transform.position) < wizardStopDistance && navmeshAgent.enabled)
            {
                navmeshAgent.isStopped = true;
                navmeshAgent.velocity = Vector3.zero;
                navmeshAgent.ResetPath();
                navmeshAgent.SetDestination(navmeshAgent.transform.position);
                animatorHandler.AnimatorAttack();

                coolingdown = true;

            }
            else if (Vector3.Distance(transform.position, Target.transform.position) < wizardStopDistance && navmeshAgent.enabled)
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
            selectedEnemy = enemyListOnField.GetClosestEnemy(transform.position, out float distance2);
            if (selectedEnemy)
                transform.LookAt(selectedEnemy.transform);
            navmeshAgent.isStopped = false;

            NavMesh.CalculatePath(navmeshAgent.gameObject.transform.position, selectedEnemy.transform.position, NavMesh.AllAreas, path);
            navmeshAgent.SetPath(path);


            enemyFound = true;
        }
        if (!coolingdown && selectedEnemy != null)
        {
            if (Vector3.Distance(transform.position, selectedEnemy.transform.position) < wizardStopDistance && navmeshAgent.enabled)
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

            wizardCDT -= Time.deltaTime;

            if (wizardCDT <= 0)
            {
                coolingdown = false;
                wizardCDT = wizardCoolDownTime;
            }

        }
        if (selectedEnemy != null && selectedEnemy.IsDead) { enemyFound = false; }


    }
    void MoveTowardsTargetFunc(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {

        if (Vector3.Distance(thisTransform.position, endPos) < wizardStopDistance)
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
            if (Vector3.Distance(transform.position, endPos) < wizardStopDistance)
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
    public void WizardAttackAnimation()
    {

        if (selectedEnemy != null)
        {

            transform.LookAt(selectedEnemy.transform);

            GameObject projectile = Instantiate(wizardProjectilePrefab, wizardThrowPos.position, wizardThrowPos.rotation);
            projectile.GetComponent<WizardProjectileMovement>().target = selectedEnemy;
            projectile.GetComponent<WizardProjectileMovement>().damage = (int)characterSkills.DamageDontTouch;
        }
        else if (Target != null)
        {
            GameObject projectile = Instantiate(wizardProjectilePrefab, wizardThrowPos.position, wizardThrowPos.rotation);
            projectile.GetComponent<WizardProjectileMovement>().damage = (int)characterSkills.DamageDontTouch;
        }
    }

    public void UpdateSkills()
    {
        characterSkills = GetComponent<CharacterSkills>();
        wizardCoolDownTime = wizardCoolDownTime / characterSkills.AttackSpeedDontTouch;
        wizardCDT = wizardCoolDownTime;
    }
}
