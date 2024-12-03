using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    public VectorValue startingPositionDefault; // Posizione iniziale della scena
    public VectorValue startingPositionDynamic; // Posizione aggiornata dopo il cambio scena
    public VectorValue startingPositionPreviousScene;  // Nuova variabile per memorizzare la posizione della scena precedente


    // Aggiungiamo una variabile per controllare se il quiz è attivo
    public bool isQuizActive = false;

    // Aggiungiamo una variabile per verificare se il tutorial è attivo
    public bool isTutorialActive = false;

    // Gestire una sola istanza del player tra le scene
    private static PlayerMovement instance;

    // Variabile per controllare se il salvataggio è temporaneamente disabilitato
    private bool preventPositionUpdate = false;

    // Metodo per disabilitare temporaneamente il salvataggio della posizione
    public void PreventPositionUpdate()
    {
        preventPositionUpdate = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Se esiste già un'istanza del player, distruggiamo quella nuova
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Altrimenti, impostiamo l'istanza e non distruggiamo questo oggetto al cambio di scena
        instance = this;
        DontDestroyOnLoad(gameObject);  // Mantenere il player tra le scene

        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        // Se la posizione dinamica è definita, usa quella; altrimenti usa la posizione predefinita
        transform.position = startingPositionDefault.initialValue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Controlla se il quiz è attivo o se il tutorial è attivo; se uno dei due lo è, blocca il movimento
        if (isQuizActive || isTutorialActive)
        {
            // Ferma il movimento e blocca l'animazione
            change = Vector3.zero;
            animator.SetBool("moving", false);
            return;
        }

        // Se il quiz e il tutorial non sono attivi, consenti il movimento
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        UpdateAnimationAndMove();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Applica la posizione dinamica solo dopo il caricamento
        Debug.Log("Posizionamento Player nella scena: " + scene.name + " a " + startingPositionDynamic.initialValue);
        transform.position = startingPositionDynamic.initialValue;

    }

    void UpdateAnimationAndMove()
    {
        if (change.x != 0) change.y = 0;

        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        myRigidBody.MovePosition(
             transform.position + change.normalized * speed * Time.deltaTime
        );
    }
}
