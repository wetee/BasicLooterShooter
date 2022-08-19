using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour{

    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildTypeChangedEventArgs> OnActiveBuildTypeChanged;

    public class OnActiveBuildTypeChangedEventArgs : EventArgs {
        public BuildingTypeSO activeBuildingType;
    }
    [SerializeField] private Building hqBuilding;

    private Camera mainCamera;
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private void Awake() {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }
    
    private void Start() {
        mainCamera = Camera.main;

        hqBuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;

    }

    private void HQ_OnDied(object sender, EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.ShowGameOverScreen();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            if (activeBuildingType != null) {
                if(IsValidSpawnPoint(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage)) {
                    if (ResourceManager.Instance.CanAffordable(activeBuildingType.constructionResourceCostArray)) {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);

                    }
                    else {
                        TooltipUI.Instance.Show("Cannot Afford " + activeBuildingType.GetResourceCostString(),
                            new TooltipUI.TooltipTimer { timer = 2f });
                    }
                }
                else {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f }); ;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            Vector3 enemySpawnPosition = UtilsClass.GetMouseWorldPosition() + UtilsClass.GetRandomDirection() * 5f;
            Enemy.Create(enemySpawnPosition);
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType) {
        activeBuildingType = buildingType;

        OnActiveBuildTypeChanged?.Invoke(this, 
            new OnActiveBuildTypeChangedEventArgs { activeBuildingType = activeBuildingType}
        );
    }
    public BuildingTypeSO GetActiveBuildingType() {
        return activeBuildingType;
    }

    public bool IsValidSpawnPoint(BuildingTypeSO buildingType, Vector3 position, out string errorMessage) {
        BoxCollider2D boxCollider = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3) boxCollider.offset, boxCollider.size, 0f);

        bool isClear = collider2DArray.Length == 0;

        if (!isClear) {
            errorMessage = "Area is not clear!";
            return false; 
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach(Collider2D collider in collider2DArray) {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();

            if(buildingTypeHolder != null) {
                
                if(buildingTypeHolder.buildingType == buildingType) {
                    errorMessage = "there is building here with the same type!";
                    return false;
                }
            }
        }

        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D collider in collider2DArray) {

            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null) {
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "Too far away from town!";
        return false;
    }
    
    public Building GetHQBuilding() {
        return hqBuilding;
    }


}
