using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f; // Example speed, adjust as needed
    private Vector2 moveInput;
    public Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); // Assuming Animator is on the same GameObject
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb.velocity = moveInput * moveSpeed;

        // Update the animation parameter
        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isMoving", true);

            // Flip the character based on the direction of movement
            if (moveInput.x != 0)
            {
                Vector3 characterScale = transform.localScale;
                characterScale.x = Mathf.Sign(moveInput.x) * Mathf.Abs(characterScale.x);
                transform.localScale = characterScale;
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}