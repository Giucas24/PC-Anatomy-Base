using UnityEngine;

public class ProfessorInteraction : MonoBehaviour
{
    public QuizUI quizUI;
    private bool playerInRange = false;

    void Update()
    {

        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            quizUI.ShowQuiz();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            quizUI.HideQuiz();
        }
    }
}

