using UnityEngine;
using BlackGardenStudios.HitboxStudioPro;

 [RequireComponent(typeof(Animator))]
 [RequireComponent(typeof(HitboxManager))]
 [RequireComponent(typeof(SpriteRenderer))]
 [RequireComponent(typeof(Destructible))]
public class HitboxCharacter : MonoBehaviour, ICharacter
{     
    private Animator animator;
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
        SpriteRenderer = GetComponent<SpriteRenderer>();
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

                        var target = (HitboxCharacter)data.TheirHitbox.Owner;
                        var destructible = target.GetComponent<Destructible>();
				        destructible.OnDamageTaken(data.Damage);
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
    
    public void EVENT_SET_SPEED(float speed)
    {
    }
}