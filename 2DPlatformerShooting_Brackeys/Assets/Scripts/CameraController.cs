using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    Vector3 offset;
    public float smoothing;
    float lowestY;

    void Start()
    {
        offset = transform.position - target.position;      // Taking the dist between current cam and player pos as offset
        lowestY = transform.position.y - 1;     // finding the lowest point until the cam will follow
    }

    void Update()
    {
        // Setting an offset value 
        Vector3 targetCamPos = target.position + offset;

        // Follow player
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

        // If player falls down follow up to lowest Y.
        if (transform.position.y < lowestY) {
            transform.position = new Vector3(transform.position.x, lowestY, transform.position.z);
        }
        
    }
}
