using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour{
    
    public static ResourceManager Instance {get; private set; }
    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private List<ResourceAmount> starterResourceAmountList;

    private Dictionary<ResourceTypeSO, int> resourceAmountDict;

    private void Awake() {
        Instance = this;
        resourceAmountDict = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name); 

        foreach(ResourceTypeSO resourceType in resourceTypeListSO.resourceTypes){
            resourceAmountDict.Add(resourceType, 0);
        }

        foreach(ResourceAmount resourceAmount in starterResourceAmountList) {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }

    }

    private void TestResourceAmountDict(){
        foreach(ResourceTypeSO resource in resourceAmountDict.Keys){
            Debug.Log(resource.nameString + ": " + resourceAmountDict[resource]);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount){
        resourceAmountDict[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetResourceAmount(ResourceTypeSO resourceType){
        return resourceAmountDict[resourceType];
    }
    public bool CanAffordable(ResourceAmount[] resourceAmounts) {

        foreach(ResourceAmount resourceAmount in resourceAmounts) {
            if(GetResourceAmount(resourceAmount.resourceType) < resourceAmount.amount) {
                return false;
            }
        }
        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmounts) {
        foreach(ResourceAmount resourceAmount in resourceAmounts) {
            resourceAmountDict[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }
}
