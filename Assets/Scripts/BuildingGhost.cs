using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour{

    private GameObject spriteGameObj;
    private ResourceNearbyOverlay resourceNearbyOverlay;
    private void Awake() {
        resourceNearbyOverlay = transform.Find("pfResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();
        spriteGameObj = transform.Find("sprite").gameObject;

        Hide();
    }

    private void Start() {
        BuildingManager.Instance.OnActiveBuildTypeChanged += BuildingManager_OnActiveBuildTypeChanged;  
    }

    private void BuildingManager_OnActiveBuildTypeChanged(object sender, BuildingManager.OnActiveBuildTypeChangedEventArgs e) {
        if (e.activeBuildingType == null) {
            Hide();
            resourceNearbyOverlay.Hide();
        }
        else {
            Show(e.activeBuildingType.sprite);
            if (e.activeBuildingType.hasResourceGeneratorData) resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            else resourceNearbyOverlay.Hide();

        }

    }

        private void Update() {
        transform.position = UtilsClass.GetMouseWorldPosition();

    }
    private void Show(Sprite ghostSprite) {
        spriteGameObj.SetActive(true);
        spriteGameObj.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }
    private void Hide() {
        spriteGameObj.SetActive(false);
    }

}
