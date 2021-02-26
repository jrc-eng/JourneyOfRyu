using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Transform cam;

    public float relativeMovementX = 0.3f;
    public float relativeMovementY = 0.3f;

    float startingX;
    float startingY;

    void Start()
    {
        startingX = transform.position.x;
        startingY = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newX = startingX + (cam.position.x) * relativeMovementX;
        float newY = startingY + (cam.position.y) * relativeMovementY;

        transform.position = new Vector2(newX, newY);
    }
}
