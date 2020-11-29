using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private float verticalSpeedInLadder = 5;
    [SerializeField]
    private float horizontalSpeedInLadder;
    private float oldSpeed;
    private float gravityScale;
    private Rigidbody2D rb;

    private void Start()
    {
        gravityScale = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().gravityScale;
        rb = GetComponentInParent<Rigidbody2D>();
        oldSpeed = GameObject.FindWithTag("Player").GetComponent<PlayerControl>().speed;
        horizontalSpeedInLadder = oldSpeed / 2;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (rb)
            {
                Vector3 v3Velocity = rb.velocity;
                other.transform.position += v3Velocity * Time.deltaTime * 0.5f;
            }
            other.GetComponent<PlayerControl>().speed = horizontalSpeedInLadder;
            other.GetComponent<CharacterAnimation>().isLadder = true;
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, verticalSpeedInLadder);
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -verticalSpeedInLadder);
            }
            else
            {
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterAnimation>().isLadder = false;
            other.GetComponent<PlayerControl>().speed = oldSpeed;
            other.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        }
    }
}