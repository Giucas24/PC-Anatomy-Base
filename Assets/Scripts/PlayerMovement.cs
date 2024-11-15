using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;
    public VectorValue startingPosition;

    // Aggiungiamo una variabile per controllare se il quiz è attivo
    public bool isQuizActive = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Controlla se il quiz è attivo; se lo è, blocca il movimento
        if (isQuizActive)
        {
            // Ferma il movimento e blocca l'animazione
            change = Vector3.zero;
            animator.SetBool("moving", false);
            return;
        }

        // Se il quiz non è attivo, consenti il movimento
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        UpdateAnimationAndMove();
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