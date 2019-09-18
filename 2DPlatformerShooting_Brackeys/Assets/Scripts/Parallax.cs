using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] background;
     float[] parallaxScales;      // The proportion of the camera movement to the backgrounds by

    public float smoothing;

     Transform cam;
    Vector3 previousCamPos;

    //Called before start(). Great for references
    private void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;

        //Assigning corresponding parallax scales
        parallaxScales = new float[background.Length];
        for(int i=0; i<background.Length; i++)
        {
            parallaxScales[i] = background[i].position.z * -1;
        }

    }

    void Update()
    {
        for (int i = 0; i < background.Length; i++) {
            //The parallax is opposite of camera movement because the previous frame multiplied by the scale
            float parallax = (cam.position.x - previousCamPos.x) * parallaxScales[i];

            //Set a target x position which is the current position plus the parallx
            float backgroundTargetPosX = background[i].position.x + parallax;

            //Create a target pos which is the backgrounds current pos with its target x pos
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, background[i].position.y, background[i].position.z);

            //Fade between current pos and target pos
            background[i].position = Vector3.Lerp(background[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

        }

        //Set previous cam pos to new cam pos at the end of the frame
        previousCamPos = cam.position;
    }
}
