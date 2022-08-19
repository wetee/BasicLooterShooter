using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour{

    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType) {
        Transform pfBuildingConstruction = Resources.Load<Transform>("pfBuildingConstruction");
        Transform buildingConstTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);

        return buildingConstruction;
    }
    private BuildingTypeSO buildingType;
    private float constructionTimer;
    private float constructionTimerMax;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionProgressShaderMaterial;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        constructionProgressShaderMaterial = spriteRenderer.material;

    }
    private void Update() {
        constructionTimer -= Time.deltaTime;

        constructionProgressShaderMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());

        if(constructionTimer <= 0) {
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);

            Destroy(gameObject);

        }
    }

    private void SetBuildingType(BuildingTypeSO buildingType) {
        this.buildingType = buildingType;
        constructionTimerMax = buildingType.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        spriteRenderer.sprite = buildingType.sprite;

        buildingTypeHolder.buildingType = buildingType;

        boxCollider.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;
    }

    public float GetConstructionTimerNormalized() {
        return 1 - constructionTimer / constructionTimerMax;
    }


}
