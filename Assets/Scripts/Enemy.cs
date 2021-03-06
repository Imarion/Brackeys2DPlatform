﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        public int damage = 40;

        private int _curHealth;
        public int curHealth {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init() {
            curHealth = maxHealth;
        }
    }

    public EnemyStats enemyStats = new EnemyStats();

    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    public Transform deathParticles;

    public string deathSoundName = "Explosion";

    public int moneyDrop = 10;

    [Header("Optional")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    // Start is called before the first frame update
    void Start()
    {
        enemyStats.Init();

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggled;

        if (statusIndicator != null) {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }

        if (deathParticles == null) {
            Debug.LogError("No deathparticles referenced on Enemy");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageEnemy(int damage)
    {
        enemyStats.curHealth -= damage;

        if (enemyStats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }
    }

    void OnUpgradeMenuToggled(bool active)
    {
        // handle what happens when the upgrade menu is toggled
        GetComponent<EnemyAI>().enabled = !active; 
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player _player = collision.gameObject.GetComponent<Player>();

        if (_player != null) {
            _player.DamagePlayer(enemyStats.damage);
            DamageEnemy(9999999);
        }
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggled;
    }
}
