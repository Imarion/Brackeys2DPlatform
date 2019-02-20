using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Player : MonoBehaviour
{
    public int fallBoundary = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "DamageVoice";

    [SerializeField]
    private StatusIndicator statusIndicator;

    private PlayerStats playerStats;

    AudioManager audioManager;    

    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.instance;

        playerStats.curHealth = playerStats.maxHealth;

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audiomanager found in scene");
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggled;

        if (statusIndicator == null)
        {
            Debug.LogError("Player: no status indicatior referenced");
        }
        else {
            statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }

        InvokeRepeating("RegenHealth", 1f / playerStats.healthRegenRate, 1f / playerStats.healthRegenRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= fallBoundary) {
            DamagePlayer(9999999);
        }
    }

    void RegenHealth() {
        playerStats.curHealth += 1;
        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

    void OnUpgradeMenuToggled(bool active) {
        // handle what happens when the upgrade menu is toggled
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null) {
            _weapon.enabled = !active;
        }
    }

    public void DamagePlayer(int damage) {
        playerStats.curHealth -= damage;

        if (playerStats.curHealth <= 0)
        {
            audioManager.PlaySound(deathSoundName);
            GameMaster.KillPlayer(this);
        }
        else {
            audioManager.PlaySound(damageSoundName);
        }

        statusIndicator.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggled;
    }
}
