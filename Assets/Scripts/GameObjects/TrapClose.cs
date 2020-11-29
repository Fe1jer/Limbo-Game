using UnityEngine;

public class TrapClose : MonoBehaviour
{
    private Animator anim; 
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            anim.SetTrigger("isClose");
        }
    }
}
