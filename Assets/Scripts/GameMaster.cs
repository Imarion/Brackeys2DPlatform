using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2.0f;
    public Transform spawnPrefab;
    public AudioSource respawnAudioCountdown;

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives { get { return _remainingLives; } }

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame() {
        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer() {
        respawnAudioCountdown.Play();
        yield return new WaitForSeconds(spawnDelay);
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
        Transform clone = Instantiate( _enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(clone.gameObject, 3f);
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
