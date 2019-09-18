using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
  
    void Update()
    {
        // Subtracting the position of the player from the mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();     // Normalising the vector i.e the sum of the vector = 1

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;   // Find the angle in degrees

        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }
}
