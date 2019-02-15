using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{

    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text WaveCountDownText;

    [SerializeField]
    Text WaveCountText;

    private WaveSpawner.SpawnState previousSate;

    // Start is called before the first frame update
    void Start()
    {
        if (spawner == null) {
            Debug.LogError("No spawner referenced");
            this.enabled = false;
        }

        if (waveAnimator == null)
        {
            Debug.LogError("No waveAnimator referenced");
            this.enabled = false;
        }

        if (WaveCountDownText == null)
        {
            Debug.LogError("No WaveCountDownText referenced");
            this.enabled = false;
        }

        if (WaveCountText == null)
        {
            Debug.LogError("No WaveCountText referenced");
            this.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (spawner.State) {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }

        previousSate = spawner.State;
    }

    void UpdateCountingUI() {

        if (previousSate != WaveSpawner.SpawnState.COUNTING) { // we just switched to COUNTING; process COUNTING only once
            //Debug.Log("COUNTING");
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountDown", true);
        }

        WaveCountDownText.text = ((int) spawner.WaveCountDown).ToString();
    }

    void UpdateSpawningUI()
    {
        if (previousSate != WaveSpawner.SpawnState.SPAWNING) // we just switched to SPAWNING; process SPAWNING only once
        {
            //Debug.Log("SPAWNING");
            waveAnimator.SetBool("WaveCountDown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            WaveCountText.text = spawner.NextWave.ToString();
        }
    }
}
