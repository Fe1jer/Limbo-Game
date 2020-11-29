using UnityEngine;

public class Turn : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, speed));
    }
}
