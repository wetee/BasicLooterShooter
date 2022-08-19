using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    public event EventHandler OnDamageTaken;
    public event EventHandler OnHealed;
    public event EventHandler OnDied;

    [SerializeField] private int hitPointMax;
    private int hitPoint;

    private void Awake() {

        hitPoint = hitPointMax;
    }
    public void Damage(int damageAmount) {
        hitPoint -= damageAmount;
        hitPoint = Mathf.Clamp(hitPoint, 0, hitPointMax);

        OnDamageTaken?.Invoke(this, EventArgs.Empty);

        if (IsDead()) OnDied?.Invoke(this, EventArgs.Empty);
    }
    public bool IsDead() {
        return hitPoint == 0;
    }
    public int GetHP() {
        return hitPoint;
    }
    public int GetMaxHP() {
        return hitPointMax;
    }
    public float GetHPNormalized() {
        return (float) hitPoint / hitPointMax;
    }
    public bool IsFullHealth() {
        return hitPoint == hitPointMax;
    }
    public void SetMaxHP(int maxHP, bool updateHealthAmount) {
        hitPointMax = maxHP;

        if(updateHealthAmount) {
            hitPoint = hitPointMax;
        }
    }

    public void Heal(int HPAmount) {
        hitPoint += HPAmount;
        hitPoint = Mathf.Clamp(hitPoint, 0, hitPointMax);

        OnHealed?.Invoke(this, EventArgs.Empty);

    }
    public void HealFully() {
        hitPoint = hitPointMax;

        OnHealed?.Invoke(this, EventArgs.Empty);
    }


}
