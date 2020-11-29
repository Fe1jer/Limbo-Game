using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    private BoxCollider2D boxColl;
    public GameObject Door;

    private Vector3 heightDoor = new Vector3(0,10f,0);
    private Vector3 heightButton = new Vector3(0, 0.15f, 0);
    private Vector3 doorMinPosition;
    private Vector3 doorMaxPosition;
    private float buttonPosition;
    private int countInTrigger = 0;

    private void Start()
    {
        doorMinPosition = Door.transform.position;
        doorMaxPosition = Door.transform.position + heightDoor;
        boxColl = GetComponentInParent<BoxCollider2D>();
        buttonPosition = boxColl.transform.position.y;
    }

    public IEnumerator DoorMovement(bool toVisible)
    {
        Vector3 step = toVisible ? new Vector3(0, 0.05f, 0) : new Vector3(0, -0.05f, 0);
        Vector3 heigt = toVisible ? doorMaxPosition : doorMinPosition;
        while (Door.transform.position != heigt)
        {
            Door.transform.position += step;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("InteractionObject"))
        {
            countInTrigger += 1;
            if (transform.localPosition.y >= buttonPosition)
            {
                StopAllCoroutines();
                StartCoroutine(DoorMovement(true));
                boxColl.transform.localPosition -= heightButton;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("InteractionObject"))
        {
            countInTrigger -= 1;
            if (transform.localPosition.y < buttonPosition && countInTrigger == 0)
            {
                StopAllCoroutines();
                StartCoroutine(DoorMovement(false));
                boxColl.transform.localPosition += heightButton;
            }
        }
    }
}