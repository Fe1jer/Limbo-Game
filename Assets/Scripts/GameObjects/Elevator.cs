using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private float length;

    public IEnumerator Motion(GameObject player)
    {
        length = transform.position.x + 13f;
        Vector3 step = new Vector3(0.05f, 0f, 0f);
        while (transform.position.x <= length) {
            player.transform.position += step;
            transform.position += step;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
