using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWaveUI : MonoBehaviour{

    [SerializeField] private EnemyWaveManager enemyWaveManager;

    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI nextWaveInfoText;
    private RectTransform indicatorRectTransform;
    private RectTransform closetEnemyIndicator;


    private Camera mainCamera;

    private void Awake() {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        nextWaveInfoText = transform.Find("waveInfoText").GetComponent<TextMeshProUGUI>();
        indicatorRectTransform = transform.Find("enemySpawnPointIndicator").GetComponent<RectTransform>();
        closetEnemyIndicator = transform.Find("closetEnemyIndicator").GetComponent<RectTransform>();
    }

    private void Start() {
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;

        SetWaveNumberText("WAVE: " + enemyWaveManager.GetWaveNumber().ToString());

        mainCamera = Camera.main;
    }


    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e) {
        SetWaveNumberText("WAVE: " + enemyWaveManager.GetWaveNumber().ToString());
    }

    private void Update() {
        HandleNextWaveMessage();
        HandleNextSpawnPointIndicator();
        HandleClosestEnemyIndicator();
    }
    private void HandleNextWaveMessage() {
        if (enemyWaveManager.GetTimeToNextWave() <= 0) {
            SetWaveInfoText("");
        }
        else {
            SetWaveInfoText("Next wave in " + enemyWaveManager.GetTimeToNextWave().ToString("F1") + "s");
        }
    }
    private void HandleNextSpawnPointIndicator() {
       

        Vector3 dirToNextSpawnPoint = (enemyWaveManager.GetNextSpawnPosition() - mainCamera.transform.position).normalized;

        indicatorRectTransform.anchoredPosition = dirToNextSpawnPoint * 300f;
        indicatorRectTransform.eulerAngles = new Vector3(0f, 0f, UtilsClass.GetAngleFromVector(dirToNextSpawnPoint));

        float distanceToNextSpawnPoint = Vector3.Distance(enemyWaveManager.GetNextSpawnPosition(), mainCamera.transform.position);

        indicatorRectTransform.gameObject.SetActive(distanceToNextSpawnPoint > mainCamera.orthographicSize * 1.5f);
    }
    private void HandleClosestEnemyIndicator() {
        float checkRadius = 555f;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, checkRadius);

        Enemy targetEnemy = null;

        foreach (Collider2D collider2D in collider2DArray) {
            Enemy enemy = collider2D.GetComponent<Enemy>();

            if (enemy != null) {
                if (targetEnemy == null) {
                    targetEnemy = enemy;
                }
                else {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position)) {
                        targetEnemy = enemy;

                    }
                }
            }
        }

        if (targetEnemy != null) {
            Vector3 dirToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;

            closetEnemyIndicator.anchoredPosition = dirToClosestEnemy * 250f;
            closetEnemyIndicator.eulerAngles = new Vector3(0f, 0f, UtilsClass.GetAngleFromVector(dirToClosestEnemy));

            float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);

            closetEnemyIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f);
        }
        else {
            closetEnemyIndicator.gameObject.SetActive(false);
        }
    }
    private void SetWaveNumberText(string text) {
        waveNumberText.SetText(text);
    }
    private void SetWaveInfoText(string text) {
        nextWaveInfoText.SetText(text);
    }
}
