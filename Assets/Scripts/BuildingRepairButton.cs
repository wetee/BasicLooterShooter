using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairButton : MonoBehaviour {

    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO resourceType;
    private void Awake() {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() => {
            int missingHealth = healthSystem.GetMaxHP() - healthSystem.GetHP();
            int repairCost = missingHealth / 2;

            ResourceAmount resourceAmount = new ResourceAmount { resourceType = this.resourceType, amount = repairCost };
            ResourceAmount[] resourceAmountArray = new ResourceAmount[] { resourceAmount };

            if (ResourceManager.Instance.CanAffordable(resourceAmountArray)) {
                healthSystem.HealFully();
                ResourceManager.Instance.SpendResources(resourceAmountArray);
            }
            else {
                TooltipUI.Instance.Show("Not enough resources to repair", new TooltipUI.TooltipTimer { timer = 2f });
            }

        });
    }


}
