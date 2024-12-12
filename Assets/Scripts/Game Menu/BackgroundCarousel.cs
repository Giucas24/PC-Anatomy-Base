using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundCarousel : MonoBehaviour
{
    public Image backgroundImage;
    public Sprite[] carouselImages;
    public float imageChangeDelay;
    public float fadeDuration;
    public float imageOpacity;

    private int currentImageIndex = 0;

    private void Start()
    {
        if (carouselImages.Length > 0 && backgroundImage != null)
        {

            backgroundImage.color = new Color(255, 255, 233, imageOpacity);

            StartCoroutine(ChangeImageLoop());
        }
        else
        {
            Debug.LogWarning("There are no images or backgroundImage is null");
        }
    }

    private IEnumerator ChangeImageLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeOut());  // ChangeImageLoop is suspended until FadeOut ends

            currentImageIndex = (currentImageIndex + 1) % carouselImages.Length;
            backgroundImage.sprite = carouselImages[currentImageIndex];

            yield return StartCoroutine(FadeIn());  // ChangeImageLoop is suspended until FadeIn ends

            yield return new WaitForSeconds(imageChangeDelay);  // same of before
        }
    }

    private IEnumerator FadeOut()
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            backgroundImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(imageOpacity, 0f, normalizedTime));
            yield return null;
        }
        backgroundImage.color = new Color(1f, 1f, 1f, 0f);
    }

    private IEnumerator FadeIn()
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            backgroundImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, imageOpacity, normalizedTime));
            yield return null;
        }
        backgroundImage.color = new Color(1f, 1f, 1f, imageOpacity);
    }
}
