using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerBehavior : MonoBehaviour
{
    //RigidBody for Velocity
    Rigidbody2D rb;



    //Character is Busy and can't be moved.
    bool busy;
    bool dead;

    
     [Header("Walking and Jumping Force")]
    [Tooltip("Ryu Speed")]
    public float speed;
    public float jumpForce;

    //BetterJump Behaviors.
    [Header("Jumping Attributes")]
    [Tooltip("Jumping and Falling for the betterJump Code")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    //Ground remembering.
    public bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float rememberGroundedFor;
    float lastTimeGrounded;

    //Additional Jumps
    public int defaultAdditionalJumps = 1;
    int additionalJumps;

    bool facingRight = true;

    public int currentAnim;

    public float fastFallAdder = 1;


    [Header("Crouch")]

    public float crouchColliderOffset;
    public float crouchColliderScale;

    float crouchOriginalOffset;
    float crouchOriginalScale;

  



    [Header("Hurt Info")]
    [Tooltip("Hurt Information")]

    public float hurtJumpForce = 4.0f;
    public float hurtJumpDistance = 1.0f;

    [Header("Death Info")]
    [Tooltip("Death Information")]

    public float forceFlyThreshold = 1.25f;
    public int damageFlyThreshold = 20;
    public float dyingJumpForce = 4.0f;
    public float dyingJumpDistance = 1.0f;


    //Animation.

    Animator animator;
    SpriteRenderer sr;

    public float currentHorizontalVelocity;

    public float currentVerticalVelocity;

    bool crouching = false;

    string animationState = "AnimationState";


    
    //Hurt and recover
    bool hurt;



    //Attack Data

    bool attacking;

    [Header("Attack 1")]
    public float attack1Duration;





    Ryu ryu;

    enum CharStates{

        //Level 1 Complexity Movement.
        Stand = 0,
        Crouch = 1,
        Run = 2,
        Jump = 3,
        Fall = 4,

        //Attacks:
        Attack_Stand = 5,
        Attack_Crouch = 6,
        Attack_Air = 7,

        //Damage:
        Hurt = 8,
        Dying_Crumple = 9,
        Dying_Fly = 10,
        Dying_HitGround = 11,


        //Attack1:
        Attack1_Stand = 12
    }

    public Orb orbPrefab;

    Orb orb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        additionalJumps = defaultAdditionalJumps;

        animator = GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();

        crouching = false;
        attacking = false;
        hurt = false;

        Instantiate(orbPrefab);

        ryu = GetComponent<Ryu>();

        busy = false;

        dead = false;


        //Initialize Crouch Details:

        

    }

    void Update()
    {
        
        Crouch();
        Move();
        Jump();
        BetterJump();
        CheckIfGrounded();  

        checkToEndHurt();

        if(dead == true)
        {
            DeathTrajectory();
        }
    }

    void FixedUpdate()
    {
        DetermineAnimation(); 
    }



    

    void Crouch()
    {
        if(busy == true)
        {
            return;
        }

        if(hurt == true || dead)
        {
            return;
        }
        
        float y = Input.GetAxisRaw("Vertical");

        //If we are on the ground, we need to stop what we are doing now.
        

        //If y is less than 0, we crouch
        if(y < 0)
        {
            crouching = true;

            if(isGrounded == true)
            {
                rb.velocity = new Vector2(0,0);
                crouching = true;
                return;
            }
          
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y-fastFallAdder);
        }
        else{
            crouching = false;
        }

    }


    void Move() {

        if(busy == true)
        {
            return;
        }

        if(hurt == true || dead)
        {
            return;
        }

        if(crouching == true)
        {
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");

        float moveBy = x * speed;

       

        rb.velocity = new Vector2(moveBy, rb.velocity.y);

        currentHorizontalVelocity = moveBy;
    }

    void Jump() {

        if(busy == true || hurt == true || dead)
        {
            return;
        }

        if(crouching)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            additionalJumps--;
        }
    }

    void BetterJump() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }  

        currentVerticalVelocity = rb.velocity.y; 
    }

    void CheckIfGrounded() {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);

        if (colliders != null) {
            isGrounded = true;
            additionalJumps = defaultAdditionalJumps;
        } else {
            if (isGrounded) {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }

        animator.SetBool("isGrounded", isGrounded);
    }



    void DetermineAnimation()
    {

        if(hurt == true)
        {
             animator.SetBool("Hurt", true);
        }
        else
        {
            animator.SetBool("Hurt", false);
        }
        
        
        if(!isGrounded)
        {
            if(currentVerticalVelocity > 0)
            {
                
                currentAnim = (int)CharStates.Jump;
            }
            else if(currentVerticalVelocity < 0)
            {
               
                 currentAnim = (int)CharStates.Fall;
            }
        }
        else
        {
            if(crouching == true)
            {
                currentAnim = (int)CharStates.Crouch;
            }
            else if(currentHorizontalVelocity == 0)
            {
                currentAnim = (int)CharStates.Stand;
            }
            else if(currentHorizontalVelocity != 0)
            {
                  
                currentAnim = (int)CharStates.Run;
            }
        }

        animator.SetInteger(animationState, currentAnim);

        if(currentHorizontalVelocity > 0 && busy == false)
        {
            facingRight = true;
        }
        else if(currentHorizontalVelocity < 0 && busy == false)
        {
            facingRight = false;
        }

        if(facingRight == true)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }



    }

    //Attack1
    void Attack1()
    {



    }

    public void Hurt(float forceMultiplier)
    {
        hurt = true;

        int direction;

        //We need to edit rb.velocity

        if(facingRight)
            direction = -1;
        else
            direction = 1;

        rb.velocity = new Vector2(hurtJumpDistance * direction * forceMultiplier, hurtJumpForce * forceMultiplier);
        
        
    }

    public void EndHurt()
    {
        
        hurt = false;
    }

    void checkToEndHurt()
    {
        if(isGrounded == true && hurt == true)
        {
            EndHurt();
        }
    }


    public void Dying(int damage, float forceMultiplier)
    {
        dead = true;

        

        if(damage < damageFlyThreshold && forceMultiplier < forceFlyThreshold && isGrounded)
        {
            CrumpleDeath();
        }
        else
        {
            FlyingDeath((float)forceMultiplier);
        }


    }

    void FlyingDeath(float forceMultiplier)
    {
        int direction;

        animator.SetBool("Dying", true);

        //We need to edit rb.velocity

        if(facingRight)
            direction = -1;
        else
            direction = 1;

        rb.velocity = new Vector2(hurtJumpDistance * direction * forceMultiplier, hurtJumpForce * forceMultiplier);


    }
    
    void CrumpleDeath()
    {
        animator.SetBool("Dying_Weak", true);

        rb.velocity = new Vector2(0,0);




    }

    void DeathTrajectory()
    {
        if(isGrounded == true)
        {
            rb.velocity = new Vector2(0,0);
        }
    }

}
