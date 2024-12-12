using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LessonManager : MonoBehaviour
{
    public Text lessonText;
    public Text lessonTitle;
    public GameObject[] books;


    private LessonCollection lessonCollection;  // from JSON file

    public GameObject scrollViewContent;

    void Start()
    {

        lessonText.gameObject.SetActive(false);
        lessonTitle.gameObject.SetActive(false);


        LoadLessons();  // from JSON file
    }

    private void LoadLessons()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("lessons");

        if (jsonFile != null)
        {
            lessonCollection = JsonUtility.FromJson<LessonCollection>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Cannot load JSON file");
        }
    }

    public void OpenLesson(int bookIndex)
    {
        if (bookIndex >= 0 && bookIndex < lessonCollection.lessons.Length)
        {
            lessonText.gameObject.SetActive(true);
            lessonTitle.gameObject.SetActive(true);

            Lesson lesson = lessonCollection.lessons[bookIndex];
            lessonTitle.text = lesson.title;
            lessonText.text = lesson.content;

            Canvas.ForceUpdateCanvases();

            StartCoroutine(ResetScrollPosition());
        }
        else
        {
            Debug.LogError("Book index not valid");
        }
    }

    private IEnumerator ResetScrollPosition()
    {
        yield return null;

        ScrollRect scrollRect = scrollViewContent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    public void CloseLesson()
    {
        lessonText.gameObject.SetActive(false);
        lessonTitle.gameObject.SetActive(false);
    }
}
