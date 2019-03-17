using UnityEngine;
using BlackGardenStudios.HitboxStudioPro;

public class StreetThug : MonoBehaviour, ICharacter
{
    public Transform target;
     
	public float PunchCoolDownTimeInSeconds = 1f;
    public float desiredMinimumDistanceFromPlayer = 1.0f;
    public float desiredMaximumDistanceFromPlayer = 3.0f;
    public float MinimumPunchingDistance = 1.0f;
    public float runningSpeed = 2.0f;

    private Animator animator;
	private float punchCoolDownTimeStamp;
    private Rigidbody2D rigidBody;
    private bool isFacingRight = true;
    private SpriteRenderer SpriteRenderer;

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

    void FixedUpdate()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("DoublePunch") || stateInfo.IsName("Uppercut"))
        {
            SpriteRenderer.sortingOrder = 2;
        }
        else {
            SpriteRenderer.sortingOrder = 0;            
        }

        if (target != null) {            
            var distanceFromPlayer = target.transform.position.x - transform.position.x;
            if (Mathf.Abs(distanceFromPlayer) > desiredMaximumDistanceFromPlayer) {  
                FaceTowardsTarget();
                float step = runningSpeed * Time.deltaTime;
                var targetPosition = target.position;
                targetPosition.y = transform.position.y;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
                animator.SetBool("IsWalking", true);
            }
            else {
                FaceTowardsTarget();
                animator.SetBool("IsWalking", false);
                if (Mathf.Abs(distanceFromPlayer) <= MinimumPunchingDistance) {
                    PunchTarget();
                }
            }
        }
    }

	public bool CanPunch() {
		return punchCoolDownTimeStamp <= Time.time;
	}

    //TODO: Animate for taking damage

    void PunchTarget() {
        if (CanPunch()) {
		    punchCoolDownTimeStamp = Time.time + PunchCoolDownTimeInSeconds;

            var randy = Random.Range(0f, 1f);
            if (randy >= 0.5f) {
                animator.SetTrigger("IsPunchingUppercut");
            }
            else {
                animator.SetTrigger("IsPunchingDouble");
            }
        }
    }
    
    void FaceAwayFromTarget() {
        if (target.position.x > transform.position.x && isFacingRight) {
            FlipFacing();
        }
        else if (target.position.x < transform.position.x && !isFacingRight) {
            FlipFacing();
        }
    }

    void FaceTowardsTarget() {
        if (target.position.x > transform.position.x && !isFacingRight) {
            FlipFacing();
        }
        else if (target.position.x < transform.position.x && isFacingRight) {
            FlipFacing();
        }
    }

    void FlipFacing()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
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

                        var target = (PlayerMovementController)data.TheirHitbox.Owner;
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