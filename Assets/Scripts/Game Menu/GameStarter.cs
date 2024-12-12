using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;
    public string gameSceneName;
    public string exitSceneName;

    void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
