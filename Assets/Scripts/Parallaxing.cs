using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;    // Array of back- and foregrounds to be parallaxed.
    public float smoothing = 1f;       // How smooth the parallax is going to be. Must be > 0.

    float[] parallaxedScales;          // The proportion of the camera's movement to move the background by.
    Transform cam;                     // reference to the main camera.
    Vector3 previousCamPos;            // the position of the camera in the previous frame.

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousCamPos = cam.position;

        parallaxedScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxedScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // the parallax is the opposite of the camera movement because the previous frame is multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxedScales[i];

            // set a tagret x position
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current position and target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // set the previous cam position to the camera's position
        previousCamPos = cam.position;
    }
}
