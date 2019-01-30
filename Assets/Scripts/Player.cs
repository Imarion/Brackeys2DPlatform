using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats {
        public int Health = 100;
    }

    public int fallBoundary = -20;

    public PlayerStats playerStats = new PlayerStats();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(9999999);
        }
    }

    public void DamagePlayer(int damage) {
        playerStats.Health -= damage;

        if (playerStats.Health <= 0) {
            GameMaster.KillPlayer(this);
        }
    }
}
