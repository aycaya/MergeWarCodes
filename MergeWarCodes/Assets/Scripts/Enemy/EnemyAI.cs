using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform target;
    Transform closestTarget;
    GameObject Tower;
    HeroListOnField heroListOnField;
    [SerializeField] public float chaseRange = 4f;
    [SerializeField] public float hitRange = 1f;

    NavMeshAgent navmeshAgent;
    float distanceToTarget = Mathf.Infinity;
    float distanceToTower = Mathf.Infinity;
    public bool isChasing = false;
    public bool isTargetLocked = false;
    NavMeshAgent agent;
    AnimatorHandler animator;
    private NavMeshPath path;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Tower = GameObject.FindGameObjectWithTag("Tower");
        path = new NavMeshPath();
        navmeshAgent = GetComponent<NavMeshAgent>();
        heroListOnField = GameObject.FindObjectOfType<HeroListOnField>();
        if (Tower)
            target = Tower.transform;

        animator = GetComponent<AnimatorHandler>();
    }

    void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished)
        {
            return;
        }
        if (target == null)
        {
            isTargetLocked = false;
            target = Tower.transform;
        }

        if (heroListOnField.GetClosestHero(transform.position, out float distance) != null)
        {
            closestTarget = heroListOnField.GetClosestHero(transform.position, out distanceToTarget).transform;
        }
        else
        {
            distanceToTarget = Mathf.Infinity;
            if (Tower)
                target = Tower.transform;
        }

        if (Tower)
            distanceToTower = Vector3.Distance(Tower.transform.position, transform.position);

        if (distanceToTower > distanceToTarget)
        {
            if (distanceToTarget <= chaseRange && !isTargetLocked && closestTarget)
            {
                target = closestTarget;
                isChasing = true;
                isTargetLocked = true;
            }
        }

        else
        {
            if (distanceToTower <= chaseRange && !isTargetLocked && Tower)
            {
                target = Tower.transform;
                isChasing = true;
                isTargetLocked = true;
            }
        }

        if (chaseRange < distanceToTower && chaseRange < distanceToTarget) { isChasing = false; isTargetLocked = false; }

        if (isChasing && target)
        {
            Vector3 targetDir = target.position - agent.gameObject.transform.position;
            targetDir.y = 0;
            float step = 5 * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(agent.gameObject.transform.forward, targetDir, step, 0.0F);

            transform.rotation = Quaternion.LookRotation(newDir);
        }

        if ((hitRange > distanceToTower || hitRange > distanceToTarget) && target && isTargetLocked)
        {
            animator.AnimatorAttack();
        }

        if (target && agent.enabled)
        {
            NavMesh.CalculatePath(agent.gameObject.transform.position, target.transform.position, NavMesh.AllAreas, path);
            agent.SetPath(path);
        }
    }
}