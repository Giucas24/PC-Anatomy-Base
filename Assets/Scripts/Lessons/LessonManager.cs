using UnityEngine;
using UnityEngine.UI; // Importa il namespace per il componente Text
using System.Collections; // Importa il namespace per IEnumerator e coroutines

public class LessonManager : MonoBehaviour
{
    public Text lessonText;  // Riferimento al componente Text che mostrerà la lezione
    public Text lessonTitle; // Riferimento al componente Text che mostrerà il titolo della lezione
    public GameObject[] books;  // Array di libri che il giocatore può interagire con

    // Collezione di lezioni caricate dal JSON
    private LessonCollection lessonCollection;

    public GameObject scrollViewContent;  // Il contenitore della Scroll View dove andrà il testo

    void Start()
    {
        // Assicurati che il testo della lezione non venga visualizzato all'inizio
        lessonText.gameObject.SetActive(false);  // Nasconde il testo inizialmente
        lessonTitle.gameObject.SetActive(false); // Nasconde il titolo inizialmente

        // Carica le lezioni dal file JSON
        LoadLessons();
    }

    // Funzione per caricare le lezioni dal file JSON
    private void LoadLessons()
    {
        // Carica il file JSON dalla cartella Resources
        TextAsset jsonFile = Resources.Load<TextAsset>("lessons");  // "lessons" è il nome del file JSON senza estensione

        if (jsonFile != null)
        {
            // Deserializza il JSON in un oggetto LessonCollection
            lessonCollection = JsonUtility.FromJson<LessonCollection>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Impossibile caricare il file JSON.");
        }
    }

    // Metodo per caricare una lezione quando il giocatore interagisce con un libro
    public void OpenLesson(int bookIndex)
    {
        // Verifica che l'indice sia valido
        if (bookIndex >= 0 && bookIndex < lessonCollection.lessons.Length)
        {
            // Mostra il testo della lezione e il titolo
            lessonText.gameObject.SetActive(true);
            lessonTitle.gameObject.SetActive(true);

            // Ottieni la lezione corrispondente e aggiorna il titolo e il testo
            Lesson lesson = lessonCollection.lessons[bookIndex];
            lessonTitle.text = lesson.title;  // Imposta il titolo della lezione
            lessonText.text = lesson.content; // Imposta il testo della lezione

            // Forza l'aggiornamento del layout (assicurati che i componenti siano ridisegnati)
            Canvas.ForceUpdateCanvases(); // Forza l'aggiornamento della UI

            // Attendere un frame per assicurarsi che il layout sia completamente aggiornato
            StartCoroutine(ResetScrollPosition());
        }
        else
        {
            Debug.LogError("Indice del libro non valido.");
        }
    }

    private IEnumerator ResetScrollPosition()
    {
        // Attendere un frame per l'aggiornamento completo del layout
        yield return null;

        // Ottieni il riferimento al ScrollRect
        ScrollRect scrollRect = scrollViewContent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            // Ripristina la posizione dello scroll al massimo (all'inizio del testo)
            scrollRect.verticalNormalizedPosition = 1f;  // Imposta la posizione allo start (massimo)
        }
    }





    // Metodo per chiudere il pannello della lezione
    public void CloseLesson()
    {
        lessonText.gameObject.SetActive(false);  // Nasconde il pannello del testo della lezione
        lessonTitle.gameObject.SetActive(false); // Nasconde anche il titolo
    }
}
