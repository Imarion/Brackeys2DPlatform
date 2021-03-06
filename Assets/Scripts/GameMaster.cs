﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2.0f;
    public Transform spawnPrefab;
    public string respawnCountdownSoundName = "RespawnCountDown";
    public string spawnSoundName = "Spawn";

    public string gameOverSoundName = "GameOver";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeUI;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives { get { return _remainingLives; } }

    [SerializeField]
    private int startingMoney;
    public static int Money;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;

    private AudioManager audioManager;

    private void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (cameraShake == null) {
            Debug.LogError("No cameraShake referenced in Gamemaster");
        }
        _remainingLives = maxLives;

        Money = startingMoney;

        // caching
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu() {
        upgradeUI.SetActive(!upgradeUI.activeSelf);
        waveSpawner.enabled = !upgradeUI.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeUI.activeSelf);
    }

    public void EndGame() {

        audioManager.PlaySound(gameOverSoundName);

        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer() {
        audioManager.PlaySound(respawnCountdownSoundName);
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
    }

    public static void KillPlayer(Player player) {
        Destroy(player.gameObject);
        _remainingLives--;
        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm.RespawnPlayer());
        }

    }

    public static void KillEnemy(Enemy _enemy) {
        gm._KillEnemy(_enemy);
    }

    public void _KillEnemy(Enemy _enemy) {

        // Let's play some sound
        audioManager.PlaySound(_enemy.deathSoundName);

        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");

        // Add particles
        Transform clone = Instantiate( _enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(clone.gameObject, 3f);

        // Go camera shake
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
