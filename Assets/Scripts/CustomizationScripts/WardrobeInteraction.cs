using UnityEngine;

public class WardrobeInteraction : MonoBehaviour
{
    public GameObject wardrobeUI;
    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleWardrobeUI();
        }
    }

    private void ToggleWardrobeUI()
    {
        if (wardrobeUI != null)
        {
            bool isActive = wardrobeUI.activeSelf;  // Check if wardrobeUI is currently active
            wardrobeUI.SetActive(!isActive);
        }
        else
        {
            Debug.LogError("wardrobeUI not linked in the inspector");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}

