using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
        private Rigidbody2D rb;
        private float dirX = 0f;
        private SpriteRenderer sprite;
        private Animator anim;
        private enum MovementState { idle, running, jumping } 

     // public Joystick joystick;
        float horizontalMove = 0f;
        bool jump = false;
        [SerializeField] private float movespeed = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal"); 
        UpdateAnimationUpdate();
    }

    //Animation
    private void UpdateAnimationUpdate()
    {
        MovementState state;

          if(dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
            Debug.Log("running ");
        }
        else if(dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
            Debug.Log("running");
        }
        else
        {
            state = MovementState.idle;
            Debug.Log("idle");
        }

        if(rb.velocity.y > 1f)
        {  
            Debug.Log(rb.velocity.y);
            state = MovementState.jumping;
        }

        anim.SetInteger("state",(int)state);
    }
}
