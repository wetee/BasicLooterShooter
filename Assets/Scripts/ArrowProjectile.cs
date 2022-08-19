using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour{

    public static ArrowProjectile Create(Vector3 position, Enemy enemy) {
        Transform pfArrowProjectile = Resources.Load<Transform>("pfArrowProjectile");
        Transform arrowTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }

    private Enemy targetEnemy;
    private Vector3 lastMoveDirection;
    private float arrowLifeTime = 2f;

    private void Update() {
        Vector3 moveDirection;

        if (targetEnemy != null) {
            moveDirection = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDirection = moveDirection;
        }
        else {
            moveDirection = lastMoveDirection;
        }

        float moveSpeed = 20f;
        transform.position += moveDirection * Time.deltaTime * moveSpeed;

        transform.eulerAngles = new Vector3(0f, 0f, UtilsClass.GetAngleFromVector(moveDirection));

        arrowLifeTime -= Time.deltaTime;
        if(arrowLifeTime <= 0f) {
            Destroy(gameObject);
        }

    }

    private void SetTarget(Enemy enemy) {
        this.targetEnemy = enemy;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if( enemy != null) {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            int damageAmount = 10;
            healthSystem.Damage(damageAmount);
            Destroy(gameObject);
        }
    }


}
