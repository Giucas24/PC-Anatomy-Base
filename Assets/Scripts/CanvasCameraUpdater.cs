using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class CanvasCameraUpdater : MonoBehaviour
{
    private Canvas canvas;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        canvas = GetComponent<Canvas>();

        SetCanvasCamera();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetCanvasCamera();
    }

    private void SetCanvasCamera()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null && canvas != null)
        {
            canvas.worldCamera = mainCamera;
        }
        else
        {
            Debug.LogWarning("MainCamera not found or Canvas not available");
        }
    }
}
