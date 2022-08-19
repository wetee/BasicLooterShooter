using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    [SerializeField] private HealthSystem healthSystem;

    [SerializeField] private Transform seperatorContainer;

    private Transform barTransform;

    private void Awake() {
        barTransform = transform.Find("bar");
        seperatorContainer = transform.Find("seperatorContainer");
    }

    private void Start() {
        healthSystem.OnDamageTaken += HealthSystem_OnDamageTaken;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        UpdateBar();
        UpdateBarVisibility();

        Transform seperatorTemplate = seperatorContainer.Find("seperatorTemplate");
        seperatorTemplate.gameObject.SetActive(false);

        float barSize = 4f;
        int healthSeperatorCount = Mathf.FloorToInt(healthSystem.GetMaxHP() / 10);
        float barOneHealthAmountSize = barSize / healthSeperatorCount;

        for (int i = 0; i < healthSeperatorCount; i++) {
            Transform tempSeperatorTransform = Instantiate(seperatorTemplate, seperatorContainer);
            tempSeperatorTransform.gameObject.SetActive(true);
            tempSeperatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i, seperatorContainer.position.y, 0f);   
        }
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        UpdateBar();
        UpdateBarVisibility();
    }

    private void HealthSystem_OnDamageTaken(object sender, System.EventArgs e) {
        UpdateBar();
        UpdateBarVisibility();
    }

    private void UpdateBar() {
        barTransform.localScale = new Vector3(healthSystem.GetHPNormalized(), 1f, 1f);
    }

    private void UpdateBarVisibility() {
        if (healthSystem.IsFullHealth()) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }


}
