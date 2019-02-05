using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int Health = 100;
    }

    public EnemyStats enemyStats = new EnemyStats();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageEnemy(int damage)
    {
        enemyStats.Health -= damage;

        if (enemyStats.Health <= 0)
        {
            GameMaster.KillEnemy(this);
        }
    }
}
