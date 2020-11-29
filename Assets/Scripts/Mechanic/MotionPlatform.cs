using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionPlatform : MonoBehaviour
{
    [SerializeField]
    private float leftX = 0;
    [SerializeField]
    private float rightX = 0;
    [SerializeField]
    private float speed = 3f;

    private bool moveingRight;

    private void Start()
    {
        transform.localPosition = new Vector2(rightX, transform.localPosition.y);
    }

    private void Update()
    {
        if(transform.localPosition.x > rightX)
        {
            moveingRight = false;
        }
        else if (transform.localPosition.x < leftX)
        {
            moveingRight = true;
        }

        if (moveingRight)
        {
            transform.localPosition = new Vector2(transform.localPosition.x + speed * Time.deltaTime, transform.localPosition.y);
        }
        else
        {
            transform.localPosition = new Vector2(transform.localPosition.x - speed * Time.deltaTime, transform.localPosition.y);
        }
    }
}
