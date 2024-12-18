using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lucchetto : MonoBehaviour
{
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    public bool playerInRange;

    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answer1Text;
    public TextMeshProUGUI answer2Text;

    public Button button1;
    public Button button2;
    public string question;
    public string[] answers;
    public int correctAnswerIndex;

    private Animator animator;

    public static int totalLocks = 4;
    public static int unlockedLocks = 0;

    void Start()
    {
        dialogBox.SetActive(false);

        button1.onClick.AddListener(() => CheckAnswer(0));
        button2.onClick.AddListener(() => CheckAnswer(1));

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else
            {
                ShowQuestion();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }

    void ShowQuestion()
    {
        dialogBox.SetActive(true);
        questionText.text = question;

        answer1Text.text = answers[0];
        answer2Text.text = answers[1];
    }

    void CheckAnswer(int index)
    {
        if (index == correctAnswerIndex)
        {
            Debug.Log("Correct answer");
            dialogBox.SetActive(false);
            RemoveLock();

            unlockedLocks++;

            if (unlockedLocks == totalLocks)
            {
                // Unlock a new skin
                if (SkinManager.Instance != null)
                {
                    SkinManager.Instance.UnlockSkin(7);
                    Debug.Log($"New skin unlocked: {SkinManager.Instance.skinNames[7]}");
                }
            }
        }
        else
        {
            Debug.Log("Wrong Answer");
            dialogBox.SetActive(false);
        }
    }


    void RemoveLock()
    {
        animator.SetTrigger("Open");

        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
