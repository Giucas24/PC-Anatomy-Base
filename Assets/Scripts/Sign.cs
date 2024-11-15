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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
            // Attiva o disattiva il Canvas
            bool isActive = signUI.activeSelf;
            signUI.SetActive(!isActive);
        }
        else
        {
            Debug.LogError("Il riferimento al Canvas del sign non è impostato!");
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
