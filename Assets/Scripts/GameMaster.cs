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

    public static GameMaster gm;
    

    // Start is called before the first frame update
    void Start()
    {        
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        gm.StartCoroutine(gm.RespawnPlayer());
    }
}
