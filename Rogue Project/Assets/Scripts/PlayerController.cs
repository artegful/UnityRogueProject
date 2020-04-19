using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float Speed = 5f;
    public float CROSSHAIR_DISTANCE = 0.3f;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject Crosshair;
    public AudioSource audioS;
    public AudioClip left;
    public AudioClip right;
    private bool leftToRight = false;
    private float RunningSpeed;
    private Vector3 movement;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = movement * Speed * RunningSpeed * Time.deltaTime;
        Animate();
        MakeSound();
	}

    public void ProcessInputs(Vector2 direction)
    {
        movement = new Vector2(direction.x, direction.y);
        RunningSpeed = Mathf.Clamp(movement.magnitude, 0, 1);
        movement.Normalize();
    }

    public void Aim(Vector2 aim)
    {
        if (aim == Vector2.zero)
        {
            Crosshair.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Vector2 crosshairPos = aim.normalized * CROSSHAIR_DISTANCE;
            Crosshair.transform.localPosition = crosshairPos;
            Crosshair.GetComponent<SpriteRenderer>().enabled = true;
        }
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

    private void MakeSound()
    {
        if (movement != Vector3.zero)
        {
            if (audioS.isPlaying)
            {
                return;
            }
            if (leftToRight)
            {
                audioS.clip = left;
                leftToRight = false;
            }
            else
            {
                audioS.clip = right;
                leftToRight = true;
            }
            audioS.Play();
        }
    }
}