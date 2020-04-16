using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float Speed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public float RunningSpeed;
    private Vector3 movement;
    private bool jumpIsDone = false;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        RunningSpeed = Mathf.Clamp(movement.magnitude, 0, 1);
        movement.Normalize();
        rb.velocity = movement * Speed * RunningSpeed * Time.deltaTime;
        Animate();
	}

    void Animate()
    {
        if (movement != Vector3.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", RunningSpeed);
    }
}
