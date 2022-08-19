using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour{
    
    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;

    private void Awake() {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private void Start() {

        int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position);

        if (nearbyResourceAmount == 0) enabled = false;
        else {
            timerMax = (resourceGeneratorData.timerMax / 2f) +
                resourceGeneratorData.timerMax *
                (1 - (float)(nearbyResourceAmount / resourceGeneratorData.maxResoureAmount));
        }
    }

    private void Update() {
        timer -= Time.deltaTime;
        if(timer <= 0f){
            timer += timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData ,Vector3 position) {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceAmount = 0;

        foreach (Collider2D collider in collider2Ds) {
            ResourceNode resourceNode = collider.GetComponent<ResourceNode>();
            if (resourceNode != null) {
                if (resourceNode.resourceType == resourceGeneratorData.resourceType) {
                    nearbyResourceAmount++;
                }
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResoureAmount);

        return nearbyResourceAmount;
    }

    public ResourceGeneratorData GetResourceGeneratorData() {
        return resourceGeneratorData;
    }
    public float GetTimerNormalized() {
        return timer / timerMax;
    }
    public float GetAmountGeneratedPerSecond() {
        return 1f / timerMax;
    }
}
