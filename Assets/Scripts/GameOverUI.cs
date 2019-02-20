using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{

    private AudioManager audioManager;

    [SerializeField]
    string mouseHoverSound = "ButtonHover";

    [SerializeField]
    string buttonPressSound = "ButtonPress";

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit() {

        audioManager.PlaySound(buttonPressSound);

        Debug.Log("Application.Quit");
        Application.Quit();
    }

    public void Retry()
    {
        audioManager.PlaySound(buttonPressSound);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }

}
