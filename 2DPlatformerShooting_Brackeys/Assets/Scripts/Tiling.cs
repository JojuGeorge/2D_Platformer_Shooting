using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 2;     // the offset so that we don't get any weird errors

    //used of checking if we need to instantiate stuff
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;   // used if the object is not tileable

    float spriteWidth = 0f;     // widht of out element
    Camera cam;
    Transform myTransform;


    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    void Update()
    {
        //does it still need buddies? if not do nothing
        if (hasALeftBuddy == false || hasARightBuddy == false) {
            //calculate the camera extend (half the width) of what the camera can see in the world coordinates
            float cameraHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //calculate the x pos where the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - cameraHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + cameraHorizontalExtend;


            //checking if we can see the edge o the element and then calling MakeNewBuddy() if we can
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false) {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
        
    }

    void MakeNewBuddy(int leftOrRight) {
        //calculating the new position of the new buddy
        Vector3 newPos = new Vector3(myTransform.position.x + spriteWidth * leftOrRight, myTransform.position.y, myTransform.position.z);

        //Instantiating new buddy and storing him in a var
        Transform newBuddy = Instantiate(myTransform, newPos, myTransform.rotation) as Transform;

        //if not tilable let's reverse the x size of our object ot get rid of guly seams
        if(reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (leftOrRight > 0) {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }

    }
}
