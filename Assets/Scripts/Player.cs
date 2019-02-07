using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats {
        public int maxHealth = 100;

        private int _curHealth;

        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    public int fallBoundary = -20;

    [SerializeField]
    private StatusIndicator statusIndicator;

    public PlayerStats playerStats = new PlayerStats();

    // Start is called before the first frame update
    void Start()
    {
        playerStats.Init();

        if (statusIndicator == null)
        {
            Debug.LogError("Player: no status indicatior referenced");
        }
        else {
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(9999999);
        }
    }

    public void DamagePlayer(int damage) {
        playerStats.curHealth -= damage;

        if (playerStats.curHealth <= 0) {
            GameMaster.KillPlayer(this);
        }

        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }
}
