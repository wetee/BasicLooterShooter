using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {


    private Enemy targetEnemy;
    private float checkTargetTimer;
    private float checkTargetTimerMax;

    private Transform projectileSpawnPosition;
    [SerializeField] private float shootTimerMax;
    private float shootTimer;
    private void Awake() {
        projectileSpawnPosition = transform.Find("projectileSpawnPosition");
    }
    private void Update() {
        HandleTargeting();
        HandleShooting();
    }
    private void CheckAvailableTargets() {

        float checkRadius = 30f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, checkRadius);

        foreach (Collider2D collider2D in collider2DArray) {
            Enemy enemy = collider2D.GetComponent<Enemy>();

            if (enemy != null) {
                if (this.targetEnemy == null) {
                    this.targetEnemy = enemy;
                }
                else {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, this.targetEnemy.transform.position)) {
                        this.targetEnemy = enemy;

                    }
                }
            }
        }

    }
    private void HandleTargeting() {
        checkTargetTimer -= Time.deltaTime;
        if (checkTargetTimer <= 0f) {
            checkTargetTimer += checkTargetTimerMax;
            CheckAvailableTargets();
        }
    }
    private void HandleShooting() {
        shootTimer -= Time.deltaTime;

        if(targetEnemy != null) {
            if(shootTimer <= 0f) {
                ArrowProjectile.Create(projectileSpawnPosition.position, targetEnemy);
                shootTimer += shootTimerMax;
            }
        }
    }
    
}
