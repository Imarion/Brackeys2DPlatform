using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

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

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "DamageVoice";


    [SerializeField]
    private StatusIndicator statusIndicator;

    AudioManager audioManager;

    public PlayerStats playerStats = new PlayerStats();

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audiomanager found in scene");
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggled;

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
