using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private float healthMultiplier = 1.3f;

    [SerializeField]
    private float movementSpeedMultiplier = 1.2f;

    [SerializeField]
    private int UpgradeCost = 50;

    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues() {
        healthText.text = "HEALTH: " + playerStats.maxHealth;
        speedText.text = "SPEED: " + playerStats.movementSpeed;
    }

    public void UpgradeHealth() {

        if (GameMaster.Money < UpgradeCost) {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        playerStats.maxHealth = (int)(playerStats.maxHealth * healthMultiplier);
        GameMaster.Money -= UpgradeCost;
        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }

    public void UpgradeSpeed()
    {
        if (GameMaster.Money < UpgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        playerStats.movementSpeed = Mathf.Round(playerStats.movementSpeed * movementSpeedMultiplier);
        GameMaster.Money -= UpgradeCost;
        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }
}
