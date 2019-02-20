using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    AudioManager audioManager;

    [SerializeField]
    string hoverOverSound = "ButtonHover";
    [SerializeField]
    string pressButtonSound = "ButtonPress";


    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("No Audiomanager found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        audioManager.PlaySound(pressButtonSound);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        audioManager.PlaySound(pressButtonSound);

        Debug.Log("We quit the game.");
        Application.Quit();
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }
}
