using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]
    private float gravityScaleInWater = 10f;
    [SerializeField]
    private int horizontalSpeedInWater = 2;
    private float oldSpeed;
    private float gravityScale;
    private int i = 0;

    private void Start()
    {
        gravityScale = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().gravityScale;
        oldSpeed = GameObject.FindWithTag("Player").GetComponent<PlayerControl>().speed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().isWater = true;

            if (i==1)
            {
                other.GetComponent<PlayerControl>().speed = horizontalSpeedInWater;
                other.GetComponent<Rigidbody2D>().gravityScale = gravityScaleInWater;
            }
            else
            {
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                i++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().isWater = false;

            i = 0;
            other.GetComponent<PlayerControl>().speed = oldSpeed;
            other.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        }
    }
}
