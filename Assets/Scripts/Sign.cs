using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    public bool playerInRange;

    public GameObject signUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
                ToggleSignUI();
            }
            else
            {
                ToggleSignUI();
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
        }
    }

    private void ToggleSignUI()
    {
        if (signUI != null)
        {
            bool isActive = signUI.activeSelf;
            signUI.SetActive(!isActive);
        }
        else
        {
            Debug.LogError("Canvas is not correctly linked");
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
}
