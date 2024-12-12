using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    public Animator bookAnimator;
    public GameObject canvasLessonComponent;
    public LessonManager lessonManager;

    public int bookIndex;
    public string playerTag = "Player";

    private bool isPlayerNearby = false;
    private bool isBookOpened = false;

    void Start()
    {
        canvasLessonComponent.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            if (isBookOpened)
            {
                CloseBook();
            }
            else
            {
                OpenBook();
            }
        }
    }

    void OpenBook()
    {
        isBookOpened = true;

        bookAnimator.SetTrigger("OpenBook");

        lessonManager.OpenLesson(bookIndex);

        Invoke("ShowCanvasLessonComponent", 1.0f);  // open book canvas after animation time
    }

    void CloseBook()
    {
        isBookOpened = false;

        lessonManager.CloseLesson();

        canvasLessonComponent.SetActive(false);

        bookAnimator.SetTrigger("CloseBook");

        Invoke("ReturnToClosedFrontal", 1.0f);  // same delay of before but for closing the book
    }

    void ReturnToClosedFrontal()
    {
        bookAnimator.SetTrigger("ReturnToClosedFrontal");
    }

    void ShowCanvasLessonComponent()
    {
        canvasLessonComponent.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            isPlayerNearby = false;
        }
    }
}
