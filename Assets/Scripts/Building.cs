using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour{

    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;
    private Transform buildingDemolishButton;
    private Transform buildingRepairButton;
    

    private void Awake() {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        healthSystem = GetComponent<HealthSystem>();

        buildingDemolishButton = transform.Find("pfBuildingDemolishButton");
        if (buildingDemolishButton != null) HideBuildingDemolishButton();

        buildingRepairButton = transform.Find("pfBuildingRepairButton");
        if (buildingRepairButton != null) HideBuildingRepairButton();

        healthSystem.SetMaxHP(buildingType.HitPointMax, true);

        healthSystem.OnDamageTaken += HealthSystem_OnDamageTaken;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDied += HealthSystem_OnDied;


    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        if (!healthSystem.IsFullHealth()) ShowBuildingRepairButton();
        else HideBuildingRepairButton();
    }

    private void HealthSystem_OnDamageTaken(object sender, System.EventArgs e) {
        ShowBuildingRepairButton();
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
    }
    private void OnMouseEnter() {
        ShowBuildingDemolishButton();
    }
    private void OnMouseExit() {
        HideBuildingDemolishButton();
    }
    private void ShowBuildingDemolishButton() {
        if(buildingDemolishButton != null) buildingDemolishButton.gameObject.SetActive(true);
    }
    private void HideBuildingDemolishButton() {
        if (buildingDemolishButton != null) buildingDemolishButton.gameObject.SetActive(false);
    }
    private void ShowBuildingRepairButton() {
        if (buildingRepairButton != null) buildingRepairButton.gameObject.SetActive(true);
    }
    private void HideBuildingRepairButton() {
        if (buildingRepairButton != null) buildingRepairButton.gameObject.SetActive(false);
    }
}
