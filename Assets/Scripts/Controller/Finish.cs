using UnityEngine;

public class Finish : MonoBehaviour
{
    public GameController gameController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<CharacterAnimation>().Finish = true;
            StartCoroutine(gameController.SelectMap());
        }
    }
}
