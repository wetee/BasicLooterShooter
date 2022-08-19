using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour{

    public static SoundManager Instance { get; private set; }

    public enum Sound {
        BuildingDamaged,
        BuildingDestroyed,
        BuildingPlaced,
        EnemyDie,
        EnemyHit,
        EnemyWaveStarting,
        GameOver,

    }

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDict;
    private float volume = 0.5f;

    private void Awake() {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        soundAudioClipDict = new Dictionary<Sound, AudioClip>();

        foreach(Sound sound in System.Enum.GetValues(typeof(Sound))) {
            soundAudioClipDict[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound) {
        audioSource.PlayOneShot(soundAudioClipDict[sound], volume);
    }

    public void IncreaseVolume() {
        volume += 0.1f;
        volume = Mathf.Clamp01(volume);
    }
    public void DecreaseVolume() {
        volume -= 0.1f;
        volume = Mathf.Clamp01(volume);
    }
    public float GetVolume() {
        return volume;
    }


}
