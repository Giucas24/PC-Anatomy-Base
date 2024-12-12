using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector2 maxPosition;
    public Vector2 minPosition;

    void Start()
    {
        if (target == null)
        {
            FindPlayer();
        }

        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
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
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }

}
