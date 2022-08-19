using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour{
    private enum State {
        WaitingToSpawnNextWave,
        SpawningWave
    }

    public static EnemyWaveManager Instance { get; private set; } 

    private State state;

    public event EventHandler OnWaveNumberChanged;


    [SerializeField] private List<Transform> spawnPointTransformList;

    [SerializeField] private Transform nextWaveSpawnPosition;

    [SerializeField] private float nextWaveTimerMax;
    private float nextWaveTimer;

    private float nextEnemySpawnTimer;
    private int remaniningEnemySpawnAmount;
    private Vector3 spawnPosition;

    private int waveNumber;
       
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPointTransformList[Random.Range(0, spawnPointTransformList.Count)].position;
        nextWaveSpawnPosition.position = spawnPosition;

        nextWaveTimer = 4f;
    }
    private void Update() {
        switch (state) {
            case State.WaitingToSpawnNextWave:
                nextWaveTimer -= Time.deltaTime;
                if (nextWaveTimer <= 0f) {
                    nextWaveTimer += nextWaveTimerMax;
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remaniningEnemySpawnAmount > 0) {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer <= 0) {
                        nextEnemySpawnTimer = Random.Range(0f, 0.2f);
                        Enemy.Create(spawnPosition + UtilsClass.GetRandomDirection() * Random.Range(0f, 10f));
                        remaniningEnemySpawnAmount--;
                            
                        if (remaniningEnemySpawnAmount <= 0) {
                            spawnPosition = spawnPointTransformList[Random.Range(0, spawnPointTransformList.Count)].position;
                            nextWaveSpawnPosition.position = spawnPosition;
                            state = State.WaitingToSpawnNextWave;
                            nextWaveTimer = nextWaveTimerMax;

                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave() {
        remaniningEnemySpawnAmount = 5 + 3 * waveNumber;

        state = State.SpawningWave;

        waveNumber++;

        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber() {
        return waveNumber;
    }

    public float GetTimeToNextWave() {
        return nextWaveTimer;
    }
    public Vector3 GetNextSpawnPosition() {
        return spawnPosition;
    }

}
