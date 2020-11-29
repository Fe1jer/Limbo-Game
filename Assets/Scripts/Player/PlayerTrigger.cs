using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField]
    private bool LSide;
    private SpriteRenderer sprite;
    private float speed;
    private float startSpeed;

    private void Start()
    {
        startSpeed = GameObject.FindWithTag("Player").GetComponent<PlayerControl>().speed;
        sprite = GameObject.FindWithTag("Player").GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        speed = GameObject.FindWithTag("Player").GetComponent<PlayerControl>().speed;

        if (other.CompareTag("CollisionObject") && sprite.flipX == LSide)
        {
            GameObject.FindWithTag("Player").GetComponent<CharacterAnimation>().isRun = false;
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().speed = 0;
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<CharacterAnimation>().isRun = true;
            if (speed == 0)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerControl>().speed = startSpeed;
            }
        }
        if (other.CompareTag("InteractionObject") && sprite.flipX == LSide)
        {
            GameObject.FindWithTag("Player").GetComponent<CharacterAnimation>().isMove = true;
            GetComponentInParent<PlayerControl>().isRun = 1;
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<CharacterAnimation>().isMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("InteractionObject") || (other.CompareTag("CollisionObject") && sprite.flipX == !LSide))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerControl>().speed = startSpeed;
            GetComponentInParent<PlayerControl>().isRun = 0;
        }
    }
}
