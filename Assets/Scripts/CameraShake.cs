using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Camera mainCam;

    float shakeAmount = 0f;

    private void Awake()
    {
        if (mainCam == null) {
            mainCam = Camera.main;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shake(float amt, float length) {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0f, 0.01f);
        Invoke("StopShake", length);
    }

    void DoShake() {
        if (shakeAmount > 0f) {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
        }
    }

    void StopShake() {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
