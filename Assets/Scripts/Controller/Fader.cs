using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public GameObject button;

    private float alpha;
    private CanvasGroup canvasGroup;

    public IEnumerator Fade(bool toVisible)
    {
        float step = toVisible ? 0.05f : -0.05f;
        int endValue = toVisible ? 1 : 0;

        while (alpha != endValue)
        {
            alpha += step;
            canvasGroup.alpha = alpha;

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
        canvasGroup = GetComponent<CanvasGroup>();
        alpha = 1;
        button.gameObject.SetActive(false);
    }
}
