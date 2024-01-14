using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using Random = UnityEngine.Random;
using System.Reflection;
using System;
using UnityEditor.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool isFacingRight = true;

    private float horizontal;
    public float speed = 4f;
    public float jumpHeight = 5f;

    public Transform groundCheck;
    public LayerMask groundLayer;


    private bool canDash = true;
    private bool isDashing = false;

    public float dashSpeed = 12f;
    public float dashingTime = 0.15f;

    public float dashCoolDown = 1f;




    void Start()
    {
        //Application.targetFrameRate = 10;
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        
        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
        if (Input.GetButtonUp("Jump") && !isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
        }
        Flip();

    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, rb.velocity.y);
        }


        if (Input.GetButton("Sprint") && canDash && isGrounded())
        {
            canDash = false;
            isDashing = true;

            rb.velocity = new Vector2(0, rb.velocity.y);
            AsyncTimer.EventTimer ET = new AsyncTimer.EventTimer((int)(dashingTime * 1000));
            ET.ProcessCompleted += DashingDuration;
            ET.StartProcess();

            AsyncTimer.EventTimer ET2 = new AsyncTimer.EventTimer((int)(dashCoolDown * 1000));
            ET2.ProcessCompleted += DashReset;
            ET2.StartProcess();


        }

    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void DashingDuration(object sender, EventArgs e)
    {
        isDashing = false;
    }
    private void DashReset(object sender, EventArgs e)
    {
        canDash = true;
    }






}
