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
    public VectorValue startingPositionDefault;
    public VectorValue startingPositionDynamic;
    public VectorValue startingPositionPreviousScene;

    public bool isQuizActive = false;

    public bool isTutorialActive = false;

    public static PlayerMovement Instance { get; private set; }

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();

        transform.position = startingPositionDefault.initialValue;
    }

    // Called with a fixed interval (Update is called each frame)
    void FixedUpdate()
    {
        if (isQuizActive || isTutorialActive)
        {
            change = Vector3.zero;
            animator.SetBool("moving", false);
            return;
        }

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
        // Set startingPositionDynamic after the scene is loaded
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
