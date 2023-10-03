using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public Animator animator;
    public CharacterController controller;
    public float speed = 10f;

    private Vector3 move;

    public float gravity = -10f;
    public float jumpForce = 8;
    private Vector3 velocity;

    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;


    //public Animator animator;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float X = Input.GetAxisRaw("Horizontal");
        float Z = Input.GetAxisRaw("Vertical");


        //animator.SetFloat("speed", Mathf.Abs(X) + Mathf.Abs(Z));

        move = transform.right * X + transform.forward * Z;
        controller.Move(move * speed * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
        }

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);

    }

    private void Jump()
    {
        velocity.y = jumpForce;
    }

}
