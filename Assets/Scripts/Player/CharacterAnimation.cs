using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip jumpAudio;
    [SerializeField]
    private AudioClip runAudio;
    [SerializeField]
    private AudioClip ladderAudio;
    private Animator anim;
    private bool isGrounded = true;
    public bool isMove;
    private bool isWater;
    public bool isRun;
    private bool isDead;
    private bool isRoll;
    public bool Finish;
    private bool hit;
    public bool isLadder;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        isRoll = GetComponent<PlayerControl>().isRoll;
        isDead = GetComponent<PlayerControl>().isDead;
        isGrounded = GetComponent<PlayerControl>().isGrounded;
        isWater = GetComponent<PlayerControl>().isWater;
        hit = GetComponent<PlayerControl>().hit;

        if ((Input.GetButton("Horizontal") && isGrounded && isRun) || (Input.GetButton("Horizontal") && isWater))
        {
/*            if (!audioSource.isPlaying)
            { //проигрываем новый звук, только если сейчас никакой звук не играет

                audioSource.clip = runAudio;
                audioSource.Play();
            }*/

            anim.SetBool("IsRunning", true);
        }
        else
        {
/*            audioSource.clip = null;
*/            anim.SetBool("IsRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isWater)
        {
            audioSource.PlayOneShot(jumpAudio);
            anim.SetTrigger("IsJump");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isWater && !hit)
        {
            anim.SetTrigger("StartRoll");
            anim.SetBool("IsRoll", true);
        }
        else if (!isRoll) 
        {
            anim.SetBool("IsRoll", false);
        }
        if (isDead)
        {
            this.transform.parent = null;
            anim.SetBool("Dead", true);
            GetComponent<PlayerControl>().enabled = false;
        }
        if (!isGrounded && !isWater)
        {
            anim.SetBool("Falls", true);
        }
        else
        {
            anim.SetBool("Falls", false);
        }
        if (Finish)
        {
            anim.SetBool("Finish", true);
        }
        if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && isLadder)
        {
            if (!audioSource.isPlaying) { 
                audioSource.clip = ladderAudio;
                Debug.Log("Text: ");
                audioSource.Play(); audioSource.pitch = 3;

            }
            anim.SetBool("IsLadder", true);
        }
        else
        {
            audioSource.pitch = 1;
            audioSource.clip = null;
            anim.SetBool("IsLadder", false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isMove)
        {
            anim.SetBool("IsMove", true);
        }
        else
        {
            anim.SetBool("IsMove", false);
        }
    }
}