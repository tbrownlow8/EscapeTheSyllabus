using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    protected Joystick joystick;

    private void Start()
    {
        // Find joystick object
        joystick = FindObjectOfType<Joystick>();
    }

    void Update()
    {
        // Take direction & speed input from joystick & assign to movement
        //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        Vector3 movement = new Vector3(joystick.Horizontal, joystick.Vertical, 0.0f);

        // Get movement's direction & magnitude & set to animator 
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);

        // Update player position on board (and multiply by 2.5f)
        transform.position = transform.position + movement * 2.5f * Time.deltaTime;
        //rb.velocity = new Vector2(movement.x, movement.y);
    }
}