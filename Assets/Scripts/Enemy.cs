using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

    public static Enemy Create(Vector3 position) {
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyTransform =  Instantiate(pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private Transform targetTransform;
    private Rigidbody2D _rigidbody2D;
    private HealthSystem healthSystem;

    private float checkTargetTimer; 
    private float checkTargetTimerMax = .2f;
    

    private void Start() {
        healthSystem = GetComponent<HealthSystem>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (BuildingManager.Instance.GetHQBuilding() != null) {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }

        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamageTaken += HealthSystem_OnDamageTaken;

        checkTargetTimer = Random.Range(0f, checkTargetTimerMax);
    }

    private void HealthSystem_OnDamageTaken(object sender, EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        Destroy(gameObject); 
    }

    private void Update() {
        HandleMovement();
        HandleTargeting();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building != null) {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            Destroy(gameObject);
        }
    }

    private void CheckAvailableTargets() {

        float checkRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        
        foreach(Collider2D  collider2D in collider2DArray) {
            Building building = collider2D.GetComponent<Building>();

            if(building != null) {
                if (targetTransform == null) { 
                    targetTransform = building.transform; 
                }
                else {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, targetTransform.position)) {
                        targetTransform = building.transform;

                    }
                }
            }
        }

        if (targetTransform == null) {
            if (BuildingManager.Instance.GetHQBuilding() != null) {
                targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
            }
        }
    }
    private void HandleMovement() {
        if (targetTransform != null) {
            Vector3 moveDirection = (targetTransform.position - transform.position).normalized;

            float moveSpeed = 6f;
            _rigidbody2D.velocity = moveDirection * moveSpeed;
        }   
        else {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void HandleTargeting() {
        checkTargetTimer -= Time.deltaTime;
        if (checkTargetTimer <= 0f) {
            checkTargetTimer += checkTargetTimerMax;
            CheckAvailableTargets();
        }
    }
}
