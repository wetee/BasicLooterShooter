using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject {
    
    public string nameString;
    public Transform prefab;
    public ResourceGeneratorData resourceGeneratorData;
    public bool hasResourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;
    public int HitPointMax;
    public float constructionTimerMax;
    

    public string GetResourceCostString() {
        string info = "";
        foreach (ResourceAmount resourceAmount in constructionResourceCostArray) {
            info += "<color=#" + resourceAmount.resourceType.colorHex + ">" +
                resourceAmount.resourceType.nameShort + 
                resourceAmount.amount + "</color> ";
        }
        return info;
    }
}
 