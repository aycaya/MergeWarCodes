using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatorHandler : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    private bool AttackIsStarted = false;
    private float tmpSpeed = 0.2f;
    [SerializeField] private bool Idle;
    [SerializeField] private bool Walk;
    [SerializeField] public bool Death;
    private NavMeshAgent navmesh;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (gameObject.CompareTag("Character"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.CompareTag("AnimatorPrefab"))
                {
                    animator = transform.GetChild(i).gameObject.GetComponent<Animator>();
                    break;
                }
            }
        }
        else
        {
            animator = GetComponent<Animator>();
        }

        navmesh = GetComponent<NavMeshAgent>();

        Idle = true;
        Walk = false;
        Death = false;

        tmpSpeed = navmesh.speed;
    }

    private void Update()
    {
        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            return;
        }


        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            navmesh.speed = 0;
            AttackIsStarted = true;
        }
        else if (AttackIsStarted)
        {
            navmesh.speed = tmpSpeed;
            AttackIsStarted = false;
        }

        var vel = rb.velocity;
        if (vel.magnitude < 0.2f)
        {
            if (!animator.GetBool("Win"))
            {
                AnimatorIdle();
            }
        }
    }

    public void AnimatorIdle()
    {
        if (!Death)
        {
            Idle = true;
            Walk = false;
            animator.SetBool("Idle", Idle);
            animator.SetBool("Walk", Walk);
        }
    }
    public void AnimatorWalk()
    {
        if (!Death)
        {
            Idle = false;
            Walk = true;
            animator.SetBool("Idle", Idle);
            animator.SetBool("Walk", Walk);
        }
    }
    public void AnimatorHit()
    {
        if (!Death)
        {
            animator.SetTrigger("Hit");
        }
    }
    public void AnimatorWin()
    {
        if (!Death)
        {
            animator.SetBool("Win", true);
        }
    }
    public void AnimatorAttack()
    {
        if (!Death)
        {
            animator.SetTrigger("Attack");
        }
    }
    public void AnimatorHeal()
    {
        if (!Death)
        {
            animator.SetTrigger("Heal");
        }
    }
    public void AnimatorDeath()
    {
        if (!Death)
        {
            animator.SetTrigger("Death");
            Death = true;
            navmesh.enabled = false;
            gameObject.GetComponent<NavMeshObstacle>().enabled = false;

        }
    }
}
