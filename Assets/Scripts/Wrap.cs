using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrap : MonoBehaviour
{
    // Grab the attached Gameobject's transform and make it wrap around the screen.
    void Update()
    {
        // TODO CACHE Camera MAIN
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Wrap the location to the viewport as this
        // game object moves to the side of the screen.
        if (viewportPosition.x < 0)
        {
            viewportPosition.x +=1;
        }
        else if (viewportPosition.x > 1)
        {
            viewportPosition.x -=1;
        }
        else if (viewportPosition.y < 0)
        {
            viewportPosition.y +=1;
        }
        else if (viewportPosition.y > 1)
        {
            viewportPosition.y -=1;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition);
    }
}
