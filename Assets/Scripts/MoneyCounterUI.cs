using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MoneyCounterUI : MonoBehaviour
{
    private Text MoneyText;

    // Start is called before the first frame update
    void Start()
    {
        MoneyText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        MoneyText.text = "MONEY: " + GameMaster.Money;
    }
}

