using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour
{

    public int offsetX = 2; // offset to instantiate the buddya bit earlier

    // do we need to instantiate something ?
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;
    
    public bool reverseScale = false;  // used for non-tilable object

    float spriteWidth = 0f;
    Camera cam;
    Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.sprite.bounds.size.x * myTransform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasALeftBuddy || !hasARightBuddy)
        {
            // calculate camera extent (half the width) of what the camera can see in world coordinates.
            float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;

            // calculate the x position when the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtent;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtent;

            // cam we see the edge of an element ?
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasARightBuddy) {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            } else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && !hasALeftBuddy) {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    void MakeNewBuddy(int rightOrLeft) {
        // rightOrLeft = 1 or -1

        // calculate position for the new Buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);

        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // if not tilable -> reverse x size to take get rid of ugly seams
        if (reverseScale) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        //newBuddy.parent = myTransform.parent;
        newBuddy.parent = myTransform;

        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
