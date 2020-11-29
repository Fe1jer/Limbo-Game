using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    private float alpha;
    public Canvas canvas;
    public GameController gameController;
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<CharacterAnimation>().Finish = true;
            player.GetComponent<PlayerControl>().enabled = false;
            canvas.gameObject.SetActive(true);
            StartCoroutine(Over());
        }
    }

    public IEnumerator Over()
    {
        StartCoroutine(Fade(true)); 
        yield return new WaitForSeconds(3f);
        gameController.LoadStartMenu();
    }

    public IEnumerator Fade(bool toVisible)
    {
        float step = toVisible ? 0.05f : -0.05f;
        int endValue = toVisible ? 1 : 0;

        while (alpha != endValue)
        {
            alpha += step;
            canvas.GetComponent<CanvasGroup>().alpha = alpha;

            if (alpha < 0)
            {
                alpha = 0;
            }
            else if (alpha > 1)
            {
                alpha = 1;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void Awake()
    {
        alpha = 0;
    }
}
