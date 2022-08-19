using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class OptionsUI : MonoBehaviour {

    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;

    private TextMeshProUGUI currentSoundLevelText;
    private TextMeshProUGUI currentMusicLevelText;

    private void Awake() {

        currentSoundLevelText = transform.Find("currentSoundText").GetComponent<TextMeshProUGUI>();
        currentMusicLevelText = transform.Find("currentMusicText").GetComponent<TextMeshProUGUI>();

        transform.Find("soundIncreaseButton").GetComponent<Button>().onClick.AddListener(() => {
            SoundManager.Instance.IncreaseVolume();
            UpdateSoundLevelText();
        });
        transform.Find("soundDecreaseButton").GetComponent<Button>().onClick.AddListener(() => {
            SoundManager.Instance.DecreaseVolume();
            UpdateSoundLevelText();
        });
        transform.Find("musicIncreaseButton").GetComponent<Button>().onClick.AddListener(() => {
            musicManager.IncreaseVolume();
            UpdateMusicLevelText();
        });
        transform.Find("musicDecreaseButton").GetComponent<Button>().onClick.AddListener(() => {
            musicManager.DecreaseVolume();
            UpdateMusicLevelText();
        });
        transform.Find("mainMenuButton").GetComponent<Button>().onClick.AddListener(() => {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });
    }
    private void Start() {
        UpdateSoundLevelText();
        UpdateMusicLevelText();
        gameObject.SetActive(false);
    }
    private void UpdateSoundLevelText() {
        currentSoundLevelText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 10).ToString());
    }
    private void UpdateMusicLevelText() {
        currentMusicLevelText.SetText(Mathf.RoundToInt(musicManager.GetVolume() * 10).ToString());

    }
    public void ToggleVisible() {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

}
