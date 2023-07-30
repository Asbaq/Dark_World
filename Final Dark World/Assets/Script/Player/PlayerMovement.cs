using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask platformlayerMask;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider2d;
    private float dirX = 0f;
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float jumpforce = 10f;
    public bool onMovingPlatform = false;
    public bool active = true;
    [SerializeField] private AudioSource jumpSoundEffect;

    // public Joystick joystick;
    bool jump = false;

    private void Start()
    {
        active = true;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider2d = transform.GetComponent<CapsuleCollider2D>();
    }


// void FixedUpdate() {
          
 //   }
   // void Move()
   // {
        // horizontalmove = joystick.Horizontal * movespeed;
        // horizontalmoves = joystick.Horizontal * (-movespeed);
    //     if(joystick.Horizontal > 0f)
    //     {
    //         rb.velocity = new Vector2(movespeed, rb.velocity.y);
    //     }

    //     if(joystick.Horizontal < 0f)
    //     {
    //         rb.velocity = new Vector2(-movespeed, rb.velocity.y);
    //     }
    // }
    // Update is called once per frame

    private void Update()
    {
        // Move();    
       // Horizontal run;
       //  dirX = joystick.Horizontal * movespeed;
       //   rb.velocity = new Vector2(joystick.Horizontal * movespeed ,rb.velocity.y);

        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movespeed,rb.velocity.y);

        //Jump
        // float verticalMove = joystick.Vertical;

        // if(IsGrounded() && verticalMove >= 0.5f)
        // {
        //     Debug.Log("Three");
        //     rb.velocity = new Vector2(rb.velocity.x,jumpforce);
        //     jumpSoundEffect.Play();
        // }

         if(IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpforce);
            jumpSoundEffect.Play();
        } 
        }

    private bool IsGrounded() {
       // RaycastHit raycastHit = Physics.CapsuleCast()
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(capsuleCollider2d.bounds.center, capsuleCollider2d.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.down, 0.1f, platformlayerMask);
        return raycastHit.collider != null;
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(collision.gameObject.tag == "MovingPlatform")
    //     {
    //         transform.parent = collision.gameObject.transform;
    //         onMovingPlatform = true; 
    //     }
    // }

    // void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "MovingPlatform")
    //     {
    //         transform.parent = null;
    //         onMovingPlatform = false;
    //     }
    // }
}
