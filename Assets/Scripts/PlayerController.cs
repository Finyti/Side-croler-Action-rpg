using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using Random = UnityEngine.Random;
using System.Reflection;
using System;
//using static UnityEditorInternal.VersionControl.ListControl;
//using UnityEditor.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool isFacingRight = true;

    private float horizontal;
    public float speed = 4f;
    public float jumpHeight = 5f;

    public Transform groundCheck;
    public Transform landingCheck;
    public LayerMask groundLayer;


    private bool canDash = true;
    private bool isDashing = false;

    public float dashSpeed = 12f;
    public float dashingTime = 0.15f;

    public float dashCoolDown = 1f;
    public float dashAnimationCoolDown = 0.5f;

    private AsyncTimer at;

    public Animator animator;


    private bool attackCast = true;
    public float attackCoolDown = 1f;
    private bool attackCastActive = false;
    public float attackActiveCoolDown = 1f;

    private bool thunderCast = true;
    public float thunderCoolDown = 5f;
    private bool thunderCastActive = false;
    public float thunderActiveCoolDown = 3f;

    private bool attacking = false;

    public GameObject thunderCloud;

    public AudioClip thunderAudio;
    public AudioClip jumpAudio;
    public AudioClip fallAudio;
    public AudioClip walkAudio;
    public AudioClip dashAudio;
    public AudioClip basicAttackAudio;



    void Start()
    {
        //Application.targetFrameRate = 10;
        rb = GetComponent<Rigidbody2D>();
        at = GetComponent<AsyncTimer>();
    }


    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        //if(attackCastActive || thunderCastActive)
        //{
        //    print("s");
        //    attacking = attackCastActive || thunderCastActive;
        //}
        attacking = attackCastActive || thunderCastActive;
        if (Input.GetButtonDown("Jump") && isGrounded() && !attacking)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            animator.SetBool("isJumping", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isRunning", false);

            var source = GetComponent<AudioSource>();
            source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            source.volume = 100f;
            source.PlayOneShot(jumpAudio);

        }

        if (Input.GetButtonUp("Jump") && !isGrounded() && !attacking)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
        }
        Flip();

        if (Input.GetMouseButtonDown(0) && attackCast && !thunderCastActive)
        {
            //animator.Play("BasicAttack");
            animator.SetBool("isBasicAttack", true);

            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);

            attackCast = false;
            attackCastActive = true;

            Task testTask1 = AsyncTimer.Delay(attackActiveCoolDown, AttackActiveReset, true);
            Task testTask2 = AsyncTimer.Delay(attackCoolDown, AttackReset, true);
            var source = GetComponent<AudioSource>();
            source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            source.volume = 100f;
            source.PlayOneShot(basicAttackAudio);
        }
        if (Input.GetKeyDown("q") && thunderCast && !attackCastActive && isGrounded())
        {
            //animator.Play("CastThunders");
            animator.SetBool("isCastingThunder", true);

            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);

            thunderCast = false;
            thunderCastActive = true;

            Task testTask1 = AsyncTimer.Delay(thunderActiveCoolDown, ThunderActiveReset, true);
            Task testTask2 = AsyncTimer.Delay(thunderCoolDown, ThunderReset, true);

            GameObject thunderInstantiate = Instantiate(thunderCloud, new Vector3(transform.position.x + (transform.localScale.x * 10), transform.position.y + 15, thunderCloud.transform.position.z),
                thunderCloud.transform.rotation);
            //Destroy the Instantiated ParticleSystem 
            Destroy(thunderInstantiate, 3);
            var source = GetComponent<AudioSource>();
            source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            source.volume = 100f;
            source.PlayOneShot(thunderAudio);
        }

    }
    

    void FixedUpdate()
    {
        //if (isLanding() && falling && rb.velocity.y < 0)

        //animator.SetBool("isFalling", false);
        //animator.SetBool("isIdle", true);
        //animator.SetBool("isRunning", false);

        bool falling = animator.GetCurrentAnimatorStateInfo(0).IsName("InAir");
        if (falling)
        {
            animator.SetBool("isJumping", false);
        }
        if (isLanding() && falling)
        {
            animator.SetBool("isInAir", false);
            animator.SetBool("isFalling", true);
            var source = GetComponent<AudioSource>();
            source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            source.volume = 100f;
            source.PlayOneShot(fallAudio);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }


        if (!isDashing && !attackCastActive && !thunderCastActive)
        {
            var source = GetComponent<AudioSource>();
            source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            source.volume = 100f;
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            if(horizontal > 0.2f || horizontal < -0.2f)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);

                if (!source.isPlaying)
                {
                    source.PlayOneShot(walkAudio);
                }

                
            }
            else if(horizontal < 0.2f && horizontal > -0.2f)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", true);
                if (source.clip == walkAudio && source.isPlaying && !source.clip == jumpAudio && !source.clip == fallAudio) 
                {
                    print("f");
                    source.Stop();
                }

            }

        }
        else if(!attackCastActive && !thunderCastActive)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, rb.velocity.y);
        }


        if (Input.GetButton("Sprint") && canDash && isGrounded() && !attacking)
        {
            canDash = false;
            isDashing = true;
            animator.SetBool("isDashing", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);

            rb.velocity = new Vector2(0, rb.velocity.y);

            Task testTask1 = AsyncTimer.Delay(dashingTime, DashingDuration, true);
            Task testTask2 = AsyncTimer.Delay(dashCoolDown, DashReset, true);
            Task testTask3 = AsyncTimer.Delay(dashAnimationCoolDown, DashAnimation, true);
            var source = GetComponent<AudioSource>();
            source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            source.volume = 100f;
            source.PlayOneShot(dashAudio);
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
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }

    private bool isLanding()
    {
        //if (isGrounded())
        //{
        //    return false;
        //}
        return Physics2D.OverlapCircle(groundCheck.position, 1f, groundLayer);
    }

    private void DashingDuration()
    {
        isDashing = false;
    }

    private void DashAnimation()
    {
        animator.SetBool("isDashing", false);
    }
    private void DashReset()
    {
        canDash = true;
    }

    private void AttackActiveReset()
    {
        print(1);
        attackCastActive = false;
        animator.SetBool("isBasicAttack", false);
    }
    private void ThunderActiveReset()
    {
        thunderCastActive = false;
        animator.SetBool("isCastingThunder", false);
    }

    private void AttackReset()
    {
        attackCast = true;
    }
    private void ThunderReset()
    {
        thunderCast = true;
    }

    void OnParticleCollision(GameObject other)
    {
    }





}
