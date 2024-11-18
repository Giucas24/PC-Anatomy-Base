using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    public Animator bookAnimator;  // Riferimento all'Animator del libro
    public GameObject canvasLessonComponent;  // Riferimento al Canvas Lesson Component
    public LessonManager lessonManager;  // Riferimento al LessonManager per caricare le lezioni

    public int bookIndex;  // Indice del libro per identificare la lezione
    public string playerTag = "Player";  // Tag usato per identificare il giocatore

    private bool isPlayerNearby = false;  // Per verificare se il giocatore è vicino al libro
    private bool isBookOpened = false;  // Per tenere traccia dello stato del libro

    void Start()
    {
        canvasLessonComponent.SetActive(false);  // Nasconde il pannello all'inizio
    }

    void Update()
    {
        // Controlla se il giocatore è vicino e preme "spazio" per aprire o chiudere il libro
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            if (isBookOpened)
            {
                CloseBook();  // Se il libro è aperto, chiudilo
            }
            else
            {
                OpenBook();  // Se il libro è chiuso, aprilo
            }
        }
    }

    // Metodo per aprire il libro
    void OpenBook()
    {
        isBookOpened = true;

        // Attiva il trigger per l'animazione di apertura
        bookAnimator.SetTrigger("OpenBook");

        // Mostra la lezione nel pannello
        lessonManager.OpenLesson(bookIndex);

        // Mostra il Canvas Lesson Component dopo la durata dell'animazione
        Invoke("ShowCanvasLessonComponent", 1.0f);  // Puoi regolare il tempo di attesa
    }

    // Metodo per chiudere il libro
    void CloseBook()
    {
        isBookOpened = false;

        // Nascondi il Canvas Lesson Component
        lessonManager.CloseLesson();

        // Nascondi il Canvas Lesson Component prima dell'animazione di chiusura
        canvasLessonComponent.SetActive(false);

        // Attiva il trigger per l'animazione di chiusura
        bookAnimator.SetTrigger("CloseBook");

        // Aggiungi un delay per attivare la transizione finale
        Invoke("ReturnToClosedFrontal", 1.0f);  // Aggiungi un ritardo per aspettare il termine dell'animazione
    }

    void ReturnToClosedFrontal()
    {
        bookAnimator.SetTrigger("ReturnToClosedFrontal");
    }

    // Metodo per mostrare il Canvas Lesson Component
    void ShowCanvasLessonComponent()
    {
        canvasLessonComponent.SetActive(true);  // Mostra il Canvas Lesson Component dopo l'animazione di apertura
    }

    // Metodo chiamato quando un oggetto entra nel trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Controlla se l'oggetto che è entrato è il giocatore
        if (collision.CompareTag(playerTag))
        {
            isPlayerNearby = true;  // Il giocatore è vicino
        }
    }

    // Metodo chiamato quando un oggetto esce dal trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Controlla se l'oggetto che è uscito è il giocatore
        if (collision.CompareTag(playerTag))
        {
            isPlayerNearby = false;  // Il giocatore non è più vicino
        }
    }
}
