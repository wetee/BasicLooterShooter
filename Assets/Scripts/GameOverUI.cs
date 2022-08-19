using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour{

    public static GameOverUI Instance { get; private set; }
    private void Awake() {

        Instance = this;

        transform.Find("retryButton").GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });
        transform.Find("mainMenuButton").GetComponent<Button>().onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        HideGameOverScreen();
    }

    public void ShowGameOverScreen() {
        gameObject.SetActive(true);

        transform.Find("waveInfoText").GetComponent<TextMeshProUGUI>().SetText("You survived " + EnemyWaveManager.Instance.GetWaveNumber() +" waves!");
        transform.Find("gameOverText").GetComponent<TextMeshProUGUI>();
    }
    public void HideGameOverScreen() {
        gameObject.SetActive(false);
    }
}
