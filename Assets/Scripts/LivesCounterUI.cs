using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LivesCounterUI : MonoBehaviour
{
    
    private Text LivesText;

    private void Awake()
    {
        LivesText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LivesText.text = "LIVES: " + GameMaster.RemainingLives;
    }
}
