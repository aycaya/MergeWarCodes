using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeControl : MonoBehaviour
{
    List<GameObject> multipleEnemies = new List<GameObject>();
    Transform closestEnemy;
    public bool enemyContact;
    float coolDownTime = 5f;
    [SerializeField] float arrowcoolDownTime = 5f;

    float arrowCDT = 5f;

    float timer = 0f;
    bool attacked = false;
    bool coolingdown = false;

    [SerializeField] float arrowForce = 20f;
    [SerializeField] Transform arrowPos;
    List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] float rangeValue = 5f;
    [SerializeField] float swordRangeVal = 1f;
    float rangeVal = 1f;
    [SerializeField] GameObject ArrowPrefab;
    Animator animator;


    void Start()
    {

        closestEnemy = null;
        enemyContact = false;
        arrowCDT = arrowcoolDownTime;
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (closestEnemy != null && enemyContact)
        {
            transform.LookAt(closestEnemy);
        }

        if (!coolingdown)
        {
            closestEnemy = GetClosestEnemy();



            if (enemyContact)
            {
                coolingdown = true;
                animator.SetTrigger("FireTrigger");


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

    }
    void EnemyContactCheck()
    {
        if (enemyList.Count == 0)
        {
            enemyContact = false;
        }
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
            }
            else if (Vector3.Distance(transform.position, enemyList[i].transform.position) > rangeVal)
            {
                enemyList.RemoveAt(i);
            }

        }

    }

    private void ArrangeRangeValues()
    {

        rangeVal = rangeValue;

    }


    public Transform PlayerHandPos()
    {
        return arrowPos;
    }
    public void TriggerEnterFunc(Collider other)
    {

        bool hasThisEnemy = false;
        foreach (GameObject el in enemyList)
        {
            if (el != null)
            {
                if (other.gameObject == el)
                {
                    hasThisEnemy = true;
                }
            }
        }
        if (!hasThisEnemy)
        {
            enemyList.Add(other.gameObject);

        }
        closestEnemy = GetClosestEnemy();


    }
    public void TriggerStayFunc(Collider other)
    {
        bool hasThisEnemy = false;
        foreach (GameObject el in enemyList)
        {
            if (el != null)
            {
                if (other.gameObject == el)
                {
                    hasThisEnemy = true;
                }
            }
        }
        if (!hasThisEnemy)
        {
            enemyList.Add(other.gameObject);

        }
        closestEnemy = GetClosestEnemy();

    }





    public Transform GetClosestEnemy()
    {
        foreach (GameObject enem in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyList.Add(enem);

        }
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
            enemyContact = true;
        }
        else
        {
            enemyContact = false;

        }
        return trans;
    }
}
