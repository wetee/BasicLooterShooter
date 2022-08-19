using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResourcesUI : MonoBehaviour{

    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDict;

    private void Awake() {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        resourceTypeTransformDict = new Dictionary<ResourceTypeSO, Transform>();

        Transform resourceTemplate = transform.Find("ResourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        int index = 0;
        foreach(ResourceTypeSO resourceType in resourceTypeList.resourceTypes){
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);

            float offsetAmount = -95f;
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, offsetAmount * index);

            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;

            resourceTypeTransformDict[resourceType] = resourceTransform;
            index++;
        }
    }
    private void Start() {
        UpdateResourceAmount();
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, EventArgs e){
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount(){
        foreach(ResourceTypeSO resourceType in resourceTypeList.resourceTypes){
            Transform resourceTransform = resourceTypeTransformDict[resourceType];

            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
