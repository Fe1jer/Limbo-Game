using UnityEngine;

public class DieSpace : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = GameObject.FindWithTag("Respawn").transform.position;
        }
    }
}
