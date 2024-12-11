using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundCarousel : MonoBehaviour
{
    public Image backgroundImage; // Riferimento al componente Image del Background Panel
    public Sprite[] carouselImages; // Array di immagini da mostrare
    public float imageChangeDelay; // Ritardo in secondi tra una immagine e l'altra
    public float fadeDuration; // Durata dell'effetto di dissolvenza
    public float imageOpacity; // Opacità delle immagini (0 = trasparente, 1 = opaco)

    private int currentImageIndex = 0;

    private void Start()
    {
        if (carouselImages.Length > 0 && backgroundImage != null)
        {
            // Imposta l'opacità iniziale
            backgroundImage.color = new Color(255, 255, 233, imageOpacity);

            StartCoroutine(ChangeImageLoop());
        }
        else
        {
            Debug.LogWarning("Immagini o componente Image non impostati correttamente.");
        }
    }

    private IEnumerator ChangeImageLoop()
    {
        while (true)
        {
            // Esegui il fade out
            yield return StartCoroutine(FadeOut());

            // Cambia immagine
            currentImageIndex = (currentImageIndex + 1) % carouselImages.Length;
            backgroundImage.sprite = carouselImages[currentImageIndex];

            // Esegui il fade in
            yield return StartCoroutine(FadeIn());

            // Aspetta per il ritardo configurato
            yield return new WaitForSeconds(imageChangeDelay);
        }
    }

    private IEnumerator FadeOut()
    {
        // Dissolvenza graduale verso trasparente
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            backgroundImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(imageOpacity, 0f, normalizedTime));
            yield return null;
        }
        // Assicurati che l'alpha sia 0 al termine
        backgroundImage.color = new Color(1f, 1f, 1f, 0f);
    }

    private IEnumerator FadeIn()
    {
        // Dissolvenza graduale verso opaco
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            backgroundImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, imageOpacity, normalizedTime));
            yield return null;
        }
        // Assicurati che l'alpha sia al valore configurato al termine
        backgroundImage.color = new Color(1f, 1f, 1f, imageOpacity);
    }
}
