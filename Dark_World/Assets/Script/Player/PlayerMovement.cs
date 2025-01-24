using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    public Joystick joystick;
    public Button jumpButton; // Assign in the Inspector

    private bool jump = false;
    private float moveDirection = 0f;

    private void Start()
    {
        active = true;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider2d = transform.GetComponent<CapsuleCollider2D>();

        // Assign jump button event
        if (jumpButton != null)
            jumpButton.onClick.AddListener(OnJumpPressed);
    }

    // Update is called once per frame

    private void Update()
    {
        moveDirection = joystick.Horizontal;

        if(moveDirection != 0)
        { 
            Debug.Log("moveDirection" + moveDirection);
        }
        
        if (Mathf.Abs(moveDirection) > 0.1f) // Add threshold to avoid accidental movements
        {
            rb.velocity = new Vector2(moveDirection * movespeed, rb.velocity.y);
        }

            // Jump if grounded
            if (IsGrounded() && jump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
                jumpSoundEffect.Play();
                jump = false; // Reset jump after execution
            }
    }

    private void OnJumpPressed()
    {
        if (IsGrounded())
        {
            jump = true; // Set jump flag, executed in Update()
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(capsuleCollider2d.bounds.center, capsuleCollider2d.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.down, 0.1f, platformlayerMask);
        return raycastHit.collider != null;
    }

}

