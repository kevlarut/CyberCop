using UnityEngine;
using UnityEngine.SceneManagement;
using BlackGardenStudios.HitboxStudioPro;

public class PlayerMovementController : MonoBehaviour, ICharacter
{
    public float PunchCoolDownTimeInSeconds = 1f;
    public float jumpForce = 150f;
    public float runningSpeed = 2.0f;
    public Gun Gun;

    private float punchCoolDownTimeStamp;
    private bool grounded = true;
    private bool isFacingRight = true;
    private bool isTalking = false;

    private Animator animator;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D rigidBody;

    [SerializeField]
    protected SpritePalette m_ActivePalette;
    [SerializeField]
    protected SpritePaletteGroup m_PaletteGroup;
    public SpritePalette ActivePalette { get { return m_ActivePalette; } }
    public SpritePaletteGroup PaletteGroup { get { return m_PaletteGroup; } }
    protected float m_BasePoise;
    protected float m_Poise;
        public float Poise
        {
            get { return m_BasePoise + m_Poise; }
            set { m_Poise = value; }
        }
        
        protected bool LockFlip { get; set; }

        public bool FlipX
        {
            get { return SpriteRenderer.flipX; }
            protected set { if(LockFlip == false) SpriteRenderer.flipX = value; }
        }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isTalking) {
            return;
        }

        var walkingInput = Input.GetAxisRaw("Horizontal");
        var jumpingInput = Input.GetAxisRaw("Vertical");

        var isJumping = !grounded && rigidBody.velocity.y > 0.1f;
        var isRunning = !isJumping && walkingInput != 0;

        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsJumping", isJumping);

        rigidBody.velocity = new Vector2(walkingInput * runningSpeed, rigidBody.velocity.y);

        if (jumpingInput > 0 && grounded)
        {
            rigidBody.AddForce(Vector2.up * jumpForce);
            grounded = false;
        }

        if (walkingInput > 0 && !isFacingRight)
        {
            FlipFacing();
        }
        else if (walkingInput < 0 && isFacingRight)
        {
            FlipFacing();
        }

        if (Input.GetButton("Fire1"))
        {
            if (Gun.CanShoot() && Gun.Shoot(isFacingRight)) {                
                animator.SetBool("IsShooting", true);
            }
        }

        if (Input.GetButton("Fire2"))
        {             
            if (CanPunch()) {
                PunchTarget();
            }
        }
        
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Punching"))
        {
            SpriteRenderer.sortingOrder = 3;
        }
        else {
            SpriteRenderer.sortingOrder = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy") 
        {
            grounded = true;
        }
    }

    public bool GetIsFacingRight() {
        return isFacingRight;
    }

    void FlipFacing()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
    public void OnDeath() {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    void OnConversationEnd() {
        isTalking = false;
    }

    void OnConversationStart() {
        isTalking = true;
        rigidBody.velocity = new Vector2(0, 0);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsJumping", false);
    }

    void StopTalking() {
        isTalking = false;
    }

    
	public bool CanPunch() {
		return punchCoolDownTimeStamp <= Time.time;
	}

    void PunchTarget() {
        if (CanPunch()) {
		    punchCoolDownTimeStamp = Time.time + PunchCoolDownTimeInSeconds;
            animator.SetTrigger("IsPunching");
        }
    }

    
    public virtual void HitboxContact(ContactData data)
    {
        switch (data.MyHitbox.Type)
        {
            // case HitboxType.ARMOR:
            //     if (data.TheirHitbox.Type == HitboxType.TECH)
            //         OnHitReceived(data.Damage, data.PoiseDamage, data.Force);
            //     break;
            case HitboxType.TRIGGER:
                {
                    // if (data.TheirHitbox.Type == HitboxType.GRAB)
                    //     m_Animator.SetTrigger("stagger");
                    // else
                    // {
                        // Character enemy = (Character)data.TheirHitbox.Owner;

                        // enemy.OnAttackHit();
                        // OnHitReceived(data.Damage, data.PoiseDamage, data.Force);
                        // PlayHitSound(2f);
                        // EffectSpawner.PlayHitEffect(data.fxID, data.Point, m_Renderer.sortingOrder + 1, !data.TheirHitbox.Owner.FlipX);

                        var target = (StreetThug)data.TheirHitbox.Owner;
				        target.GetComponent<Destructible>().OnDamageTaken(1f);
                    //}
                }
                break;
            // case HitboxType.GUARD:
            //     {
            //         if (data.TheirHitbox.Type == HitboxType.GRAB)
            //             m_Animator.SetTrigger("stagger");
            //         else
            //         {
            //             Character enemy = (Character)data.TheirHitbox.Owner;

            //             OnGuardReceived(enemy.m_Transform.position);
            //             enemy.OnAttackGuarded();
            //             PlayHitSound(1f);
            //             //2 == block FX
            //             EffectSpawner.PlayHitEffect(2, data.Point, m_Renderer.sortingOrder + 1, !data.TheirHitbox.Owner.FlipX);
            //         }

            //     }
            //     break;
            // case HitboxType.GRAB:
            //     {
            //         if(data.TheirHitbox.Type == HitboxType.GRAB ||
            //             data.TheirHitbox.Type == HitboxType.TECH)
            //         {
            //             CancelGrab();
            //         }
            //         else
            //         {
            //             m_Animator.SetTrigger("grab_confirm");
            //             //attach the opponent to my hand through events and lock him in the stagger state
            //         }
            //     }
            //     break;
            // case HitboxType.TECH:
            //     if (data.TheirHitbox.Type == HitboxType.TECH)
            //         CancelGrab();
            //     break;
        }
    }
}