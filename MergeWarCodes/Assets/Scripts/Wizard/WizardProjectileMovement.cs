using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardProjectileMovement : MonoBehaviour
{
    Wizard wizardScript;
    [SerializeField] float bulletForce = 5f;
    [SerializeField] float AreaSize = 3f;
    public int damage;
    EnemyHealth enemyHealth;
    EnemyListOnField enemyListOnField;
    TowerHealth towerHealth;
    public ParticleSystem AreaDamageParticle;
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
            Instantiate(AreaDamageParticle, transform.position, Quaternion.identity);
            Collider[] hit = Physics.OverlapSphere(transform.position, AreaSize);
            foreach (var VARIABLE in hit)
            {
                if (VARIABLE.gameObject.CompareTag("Enemy"))
                {
                    enemyHealth = VARIABLE.GetComponent<EnemyHealth>();
                    if (enemyHealth.enabled)
                    {
                        enemyHealth.GetAttacked(damage);
                    }
                }

                if (VARIABLE.gameObject.CompareTag("EnemyTower"))
                {
                    var enemyHealth1 = VARIABLE.GetComponent<TowerHealth>();
                    if (enemyHealth1.enabled)
                    {
                        enemyHealth1.GetAttacked(damage);
                    }
                }
            }

            StartCoroutine(WaitAndDestroy());
        }
        else if (other.isTrigger != true && other.CompareTag("EnemyTower"))
        {
            Instantiate(AreaDamageParticle, transform.position, Quaternion.identity);
            Collider[] hit = Physics.OverlapSphere(transform.position, AreaSize);
            foreach (var VARIABLE in hit)
            {
                if (VARIABLE.gameObject.CompareTag("Enemy"))
                {
                    enemyHealth = VARIABLE.GetComponent<EnemyHealth>();
                    if (enemyHealth.enabled)
                    {
                        enemyHealth.GetAttacked(damage);
                    }
                }

                if (VARIABLE.gameObject.CompareTag("EnemyTower"))
                {
                    var enemyHealth1 = VARIABLE.GetComponent<TowerHealth>();
                    if (enemyHealth1.enabled)
                    {
                        enemyHealth1.GetAttacked(damage);
                    }
                }
            }

            StartCoroutine(WaitAndDestroy());

        }
    }
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForFixedUpdate();
        Destroy(this.gameObject);

    }
}