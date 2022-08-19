using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectionUI : MonoBehaviour {

    [SerializeField] private Sprite arrowSprite;

    [SerializeField] List<BuildingTypeSO> ignoredBuildingTypeList;

    private Dictionary<BuildingTypeSO, Transform> buttonTransformDict;

    private Transform arrowButton;
    private void Awake() {
        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypeListSO");

        buttonTransformDict = new Dictionary<BuildingTypeSO, Transform>();

        Transform buttonTemplate = transform.Find("ButtonTemplate");
        buttonTemplate.gameObject.SetActive(false);

        float index = 0;

        float offsetAmount = 125f;

        arrowButton = Instantiate(buttonTemplate, transform);
        arrowButton.gameObject.SetActive(true);
        
        arrowButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, offsetAmount * index);

        arrowButton.Find("image").GetComponent<Image>().sprite = arrowSprite;

        arrowButton.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -20f);

        arrowButton.GetComponent<Button>().onClick.AddListener(() => {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });

        MouseEnterExitEvents mouseEnterExitEvents = arrowButton.GetComponent<MouseEnterExitEvents>();

        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) => {
            TooltipUI.Instance.Show("Arrow Button");
        };

        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) => {
            TooltipUI.Instance.Hide();
        };

        index++; 

        foreach (BuildingTypeSO buildingType in buildingTypeList.buildings) {
            if (ignoredBuildingTypeList.Contains(buildingType)) continue;

            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            buttonTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, offsetAmount * index);

            buttonTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() => {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            mouseEnterExitEvents = buttonTransform.GetComponent<MouseEnterExitEvents>();

            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) => {
                TooltipUI.Instance.Show(buildingType.nameString + "\n" + buildingType.GetResourceCostString());
            };

            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) => {
                TooltipUI.Instance.Hide();
            };

            buttonTransformDict.Add(buildingType, buttonTransform);

            index++;
        }
    }
    private void Start() {
        BuildingManager.Instance.OnActiveBuildTypeChanged += BuildingManager_OnActiveBuildTypeChanged;
    }

    private void BuildingManager_OnActiveBuildTypeChanged(object sender, BuildingManager.OnActiveBuildTypeChangedEventArgs e) {
        UpdateActiveBuildingTypeButton();

    }

    private void UpdateActiveBuildingTypeButton() {
        arrowButton.Find("selected").gameObject.SetActive(false);
        foreach (BuildingTypeSO buildingType in buttonTransformDict.Keys) {
            Transform tempButtonTransform = buttonTransformDict[buildingType];
            tempButtonTransform.Find("selected").gameObject.SetActive(false);
        }

        BuildingTypeSO tempBuildingType = BuildingManager.Instance.GetActiveBuildingType();

        if (tempBuildingType == null) 
            arrowButton.Find("selected").gameObject.SetActive(true);
        else 
            buttonTransformDict[tempBuildingType].Find("selected").gameObject.SetActive(true);
        
    }
}
