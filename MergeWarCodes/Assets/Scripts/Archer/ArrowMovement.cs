using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    ArcherAttack archerAttack;
    [SerializeField] float bulletForce = 5f;
    [HideInInspector] public int damage;
    EnemyHealth enemyHealth;
    EnemyListOnField enemyListOnField;
    TowerHealth towerHealth;
    [HideInInspector] public EnemyHealth target = null;
    GameObject targetObject = null;


    void Start()
    {
        enemyListOnField = FindObjectOfType<EnemyListOnField>();

        targetObject = GameObject.FindGameObjectWithTag("EnemyTower");
        if (targetObject != null)
        {
            towerHealth = targetObject.GetComponent<TowerHealth>();

        }
    }

    void Update()
    {

        if (!GameManager.gameStarted || GameManager.gameFinished || Tutorial.isTutorialOn)
        {
            Destroy(this.gameObject);
        }
        if (target != null)
        {

            transform.LookAt(target.transform);


            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, bulletForce * Time.deltaTime);

        }
        else if (towerHealth != null)
        {
            transform.LookAt(towerHealth.transform);


            transform.position = Vector3.MoveTowards(transform.position, towerHealth.transform.position, bulletForce * Time.deltaTime);


        }
        else
        {
            Destroy(this.gameObject);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger != true && other.CompareTag("Enemy"))
        {
            enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.GetAttacked(damage);
            StartCoroutine(WaitAndDestroy());


        }
        else if (other.isTrigger != true && other.CompareTag("EnemyTower"))
        {
            other.GetComponent<TowerHealth>().GetAttacked(damage);
            StartCoroutine(WaitAndDestroy());

        }

    }
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForFixedUpdate();
        Destroy(this.gameObject);

    }
}