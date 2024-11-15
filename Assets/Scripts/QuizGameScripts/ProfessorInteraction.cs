using UnityEngine;

public class ProfessorInteraction : MonoBehaviour
{
    public QuizUI quizUI; // Reference to the QuizUI script
    private bool playerInRange = false; // Boolean to track if player is in range

    void Update()
    {
        // Check if the player is in range and presses the Space key
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            quizUI.ShowQuiz(); // Show the quiz UI when Space is pressed
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Set player in range to true when player enters the area
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Set player in range to false when player exits the area
            quizUI.HideQuiz(); // Hide the quiz UI when player moves away
        }
    }
}

