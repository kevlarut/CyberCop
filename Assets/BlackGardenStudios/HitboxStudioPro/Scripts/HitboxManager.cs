using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Runtime.InteropServices;

namespace BlackGardenStudios.HitboxStudioPro
{
#pragma warning disable 0414
    public class HitboxManager : MonoBehaviour
    {
        static private readonly int MAX_HITBOXES = 10;

        #region Serialization
        [Serializable]
        public struct internalFrameData
        {
            public internalHitboxData frame;
            public internalOffsetData nextframe;
            public MovementState movementstate;
            public float framerate;
            public float movementspeed;
            public float poise;
            public float strength;
            public float damage;
            public Vector2 force;
            public Matrix2x2 direction;
            public int numtargets;
            public int hitfxuid;
            public bool hasnextframe;

            public internalFrameData(HitboxAnimation source, int frameID)
            {
                frame = source.framedata[frameID];
                if ((frameID + 1) < source.framedata.Length)
                {
                    hasnextframe = true;
                    nextframe = source.framedata[frameID + 1];
                }
                else
                {
                    hasnextframe = false;
                    nextframe = default(internalOffsetData);
                }
                movementstate = source.movementstate;
                movementspeed = source.movementspeed;
                poise = source.poise;
                strength = source.strength;
                damage = source.damage;
                force = source.force;
                direction = source.direction;
                numtargets = source.numtargets;
                hitfxuid = source.hitfxuid;

                if (source.clip != null)
                    framerate = source.clip.frameRate;
                else
                    framerate = 0f;
            }

#if UNITY_EDITOR
            public string Serialize()
            {
                List<byte> data = new List<byte>(256);
                data.AddRange(BinaryStructConverter.ToByteArray((serializableData)this));

                if (frame.collider != null && frame.collider.Length > 0)
                    for (int i = 0; i < frame.collider.Length; i++)
                    {
                        data.AddRange(BinaryStructConverter.ToByteArray(frame.collider[i]));
                    }

                return Convert.ToBase64String(data.ToArray());
            }
#endif

            static public internalFrameData Deserialize(string s)
            {
                var data = Convert.FromBase64String(s);
                var serializedResult = BinaryStructConverter.FromByteArray<serializableData>(data);
                var count = serializedResult.numColliders;
                internalFrameData result = serializedResult;

                if (count > 0)
                {
                    int sizeofFrame = Marshal.SizeOf(typeof(serializableData));
                    int sizeofCollider = Marshal.SizeOf(typeof(HitboxColliderData));
                    var colliderArray = new HitboxColliderData[count];

                    for (int i = 0; i < count; i++)
                    {
                        colliderArray[i] = BinaryStructConverter.FromByteArray<HitboxColliderData>(data, sizeofFrame + sizeofCollider * i);
                    }

                    result.frame.collider = colliderArray;
                }

                return result;
            }

            static public implicit operator internalFrameData(serializableData v)
            {
                return new internalFrameData
                {
                    damage = v.damage,
                    frame = v.frame,
                    framerate = v.framerate,
                    hasnextframe = v.hasnextframe,
                    movementspeed = v.movementspeed,
                    movementstate = v.movementstate,
                    nextframe = v.nextframe,
                    poise = v.poise,
                    strength = v.strength,
                    numtargets = v.numtargets,
                    direction = v.direction,
                    force = v.force,
                    hitfxuid = v.hitfxuid
                };
            }

            public struct serializableData
            {
                public internalOffsetData frame;
                public internalOffsetData nextframe;
                public MovementState movementstate;
                public int numColliders;
                public float framerate;
                public float movementspeed;
                public float poise;
                public float strength;
                public float damage;
                public Vector2 force;
                public Matrix2x2 direction;
                public int numtargets;
                public int hitfxuid;
                public bool hasnextframe;

                public static implicit operator serializableData(internalFrameData v)
                {
                    return new serializableData
                    {
                        damage = v.damage,
                        frame = v.frame,
                        framerate = v.framerate,
                        hasnextframe = v.hasnextframe,
                        movementspeed = v.movementspeed,
                        movementstate = v.movementstate,
                        nextframe = v.nextframe,
                        numColliders = (v.frame.collider == null ? 0 : v.frame.collider.Length),
                        poise = v.poise,
                        strength = v.strength,
                        numtargets = v.numtargets,
                        direction = v.direction,
                        force = v.force,
                        hitfxuid = v.hitfxuid
                    };
                }
            }
        }

        [Serializable]
        public struct internalHitboxData
        {
            public HitboxColliderData[] collider;
            public Vector2Int capsuleOffset;
            public bool smoothedOffset;

            public static implicit operator internalHitboxData(HitboxAnimationFrame v)
            {
                return new internalHitboxData
                {
                    collider = v.collider,
                    capsuleOffset = v.capsuleOffset,
                    smoothedOffset = v.smoothedOffset
                };
            }

            public static implicit operator internalHitboxData(internalOffsetData v)
            {
                return new internalHitboxData
                {
                    capsuleOffset = v.capsuleOffset,
                    smoothedOffset = v.smoothedOffset
                };
            }
        }

        [Serializable]
        public struct internalOffsetData
        {
            public Vector2Int capsuleOffset;
            public bool smoothedOffset;

            public static implicit operator internalOffsetData(HitboxAnimationFrame v)
            {
                return new internalOffsetData
                {
                    capsuleOffset = v.capsuleOffset,
                    smoothedOffset = v.smoothedOffset
                };
            }

            public static implicit operator internalOffsetData(internalHitboxData v)
            {
                return new internalOffsetData
                {
                    capsuleOffset = v.capsuleOffset,
                    smoothedOffset = v.smoothedOffset
                };
            }
        }
        #endregion

        #region Data Types
        [Serializable]
        public struct Matrix2x2
        {
            public Vector2 x;
            public Vector2 y;
        }

        [Serializable]
        public struct HitboxAnimation
        {
            public AnimationClip clip;
            public HitboxAnimationFrame[] framedata;
            public MovementState movementstate;
            public float movementspeed;
            public float poise;
            public float strength;
            public float damage;
            public Vector2 force;
            public Matrix2x2 direction;
            public int numtargets;
            public int hitfxlabel;
            public int hitfxuid;
        }
        [Serializable]
        public struct HitboxFrameEventData
        {
            public FrameEvent id;
            public int intParam;
            public float floatParam;
            public string stringParam;
        }
        [Serializable]
        public struct HitboxAnimationFrame
        {
            public HitboxColliderData[] collider;
            public HitboxFrameEventData[] events;
            public Vector2Int capsuleOffset;
            public float time;
            public bool smoothedOffset;
        }
        [Serializable]
        public struct HitboxColliderData
        {
            public RectInt rect;
            public HitboxType type;
        }

        private struct ContactPair
        {
            public HitboxFeeder a;
            public HitboxFeeder b;
        }
        #endregion

        #region Enums
        [Serializable]
        public enum MovementState
        {
            NO_CHANGE,
            DISABLE_MOVEMENT,
            ENABLE_MOVEMENT,
            DISABLE_DIRECTION_CHANGE,
            ENABLE_DIRECTION_CHANGE,
            ENABLE_BOTH
        }
        #endregion

        #region Properties
        [SerializeField]
        public HitboxAnimation[] m_Animations;

        private SpriteRenderer m_Renderer;
        private BoxCollider2D[] m_Colliders;
        private HitboxFeeder[] m_Feeder;
        private List<int> m_RecentHits = new List<int>(16);
        private List<ContactPair> m_Contacts = new List<ContactPair>(10);
        private ICharacter m_Character;
        private float m_UPP = 1f / 32f;
        private int m_UnitsToPixel = 32;
        private float m_Scale = 1f;
        public int UID { get; private set; }
        static private int m_UIDCounter = 0;
        #endregion

        void Awake()
        {
            var scale = transform.localScale;

            UID = m_UIDCounter++;

            m_Character = GetComponent<ICharacter>();

            m_Scale = scale.x;
            m_Colliders = GetComponentsInChildren<BoxCollider2D>();
            m_Feeder = GetComponentsInChildren<HitboxFeeder>();
            if (m_Colliders == null || m_Colliders.Length < MAX_HITBOXES)
            {
                m_Colliders = new BoxCollider2D[MAX_HITBOXES];
                m_Feeder = new HitboxFeeder[MAX_HITBOXES];
                for (int i = 0; i < m_Colliders.Length; i++)
                {
                    var newGameObject = new GameObject("collider (" + i + ")");
                    newGameObject.transform.SetParent(transform, false);
                    var collider = newGameObject.AddComponent<BoxCollider2D>();
                    var feeder = newGameObject.AddComponent<HitboxFeeder>();

                    collider.isTrigger = true;
                    collider.enabled = false;
                    m_Colliders[i] = collider;
                    m_Feeder[i] = feeder;
                }
            }

            GetSpriteBPP();
        }

        private void OnValidate()
        {
            GetSpriteBPP();
        }

        private void GetSpriteBPP()
        {
            if (m_Renderer == null) m_Renderer = GetComponent<SpriteRenderer>();

            var sprite = m_Renderer.sprite;

            if (sprite != null)
            {
                m_UPP = 1 / m_Renderer.sprite.pixelsPerUnit;
                m_UnitsToPixel = (int)m_Renderer.sprite.pixelsPerUnit;
            }
            else
            {
                Debug.LogWarning("HITBOX MANAGER WARNING: No sprite is assigned during Awake(). Unable to retreive sprite.pixelsPerUnit. Movement and hitbox location calculations will be incorrect!");
            }
        }

        #region Animation Baking //Editor Only
#if UNITY_EDITOR
        public void BakeAnimation(int animationID)
        {
            var animation = m_Animations[animationID];

            if (animation.clip == null) return;

            var clip = animation.clip;
            var numFrames = GetNumFrames(animationID);
            var events = new List<AnimationEvent>(60);

            AnimationUtility.SetAnimationEvents(clip, new AnimationEvent[0]);

            for (int k = 0; k < numFrames; k++)
            {
                if (animation.framedata.Length <= k) break;
                var framedata = animation.framedata[k];

                AnimationEvent _event = new AnimationEvent();

                _event.functionName = "UpdateHitbox";
                _event.stringParameter = new internalFrameData(animation, k).Serialize();
                _event.floatParameter = animationID;
                _event.intParameter = k;
                _event.time = framedata.time;
                events.Add(_event);

                if (animation.framedata[k].events != null)
                    for (int i = 0; i < animation.framedata[k].events.Length; i++)
                        {
                            AnimationEvent frame_event = new AnimationEvent();

                            frame_event.functionName = "EVENT_" + animation.framedata[k].events[i].id.ToString();
                            frame_event.intParameter = animation.framedata[k].events[i].intParam;
                            frame_event.floatParameter = animation.framedata[k].events[i].floatParam;
                            frame_event.stringParameter = animation.framedata[k].events[i].stringParam;
                            frame_event.time = framedata.time;
                            events.Add(frame_event);
                        }
            }

            if (animation.movementstate != MovementState.NO_CHANGE)
            {
                AnimationEvent move_event = new AnimationEvent();

                switch (animation.movementstate)
                {
                    case MovementState.ENABLE_MOVEMENT:
                        move_event.functionName = "EVENT_ENABLE_MOVE";
                        break;
                    case MovementState.DISABLE_MOVEMENT:
                        move_event.functionName = "EVENT_DISABLE_MOVE";
                        break;
                    case MovementState.ENABLE_DIRECTION_CHANGE:
                        move_event.functionName = "EVENT_ENABLE_DIRECTION";
                        break;
                    case MovementState.DISABLE_DIRECTION_CHANGE:
                        move_event.functionName = "EVENT_DISABLE_DIRECTION";
                        break;
                    case MovementState.ENABLE_BOTH:
                        move_event.functionName = "EVENT_ENABLE_BOTH";
                        break;
                }

                if (!string.IsNullOrEmpty(move_event.functionName))
                {
                    move_event.time = 0;
                    events.Add(move_event);
                }
            }

            //if (animation.movementspeed != 0f && animation.movementspeed != 1f)
            {
                AnimationEvent speed_event = new AnimationEvent();
                speed_event.functionName = "EVENT_SET_SPEED";
                speed_event.floatParameter = animation.movementspeed;
                speed_event.time = 0;
                events.Add(speed_event);

                AnimationEvent speed_event2 = new AnimationEvent();
                speed_event2.functionName = "EVENT_SET_SPEED";
                speed_event2.floatParameter = animation.movementspeed;
                speed_event2.time = animation.framedata[animation.framedata.Length - 1].time;
                events.Add(speed_event2);
            }

            AnimationUtility.SetAnimationEvents(clip, events.ToArray());
        }

        public void BakeAnimations()
        {
            for (int j = 0; j < m_Animations.Length; j++)
            {
                BakeAnimation(j);
            }
        }
#endif
        #endregion

        private void EVENT_RESET_TARGETS()
        {
            ResetReports();
        }

        private void EVENT_SET_POISE_DAMAGE(float dmg)
        {
            for (int i = 0; i < m_Feeder.Length; i++)
                m_Feeder[i].UpdatePoiseDamage(dmg);
        }

        private void EVENT_SET_ATTACK_DAMAGE(float dmg)
        {
            for (int i = 0; i < m_Feeder.Length; i++)
                m_Feeder[i].UpdateAttackDamage(dmg);
        }

        /// <summary>
        /// Record a hit that has been taken, if it has not already been taken.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetid"></param>
        /// <returns>Whether or not the hit has been recorded.</returns>
        public bool ReportHit(int id, int targetid)
        {
            bool hit = false;
            var count = m_RecentHits.Count;

            if (count > 0)
            {
                if (!(count < m_CurrentHitCount))
                    hit = true;
                else
                    hit = !m_RecentHits.TryUniqueAdd(targetid);
            }
            else
                m_RecentHits.Add(targetid);

            return !hit;
        }

        /// <summary>
        /// Check whether or not a hit has been taken without recording it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetid"></param>
        /// <returns>Whether or not the hit was recorded.</returns>
        public bool PeekReport(int id, int targetid)
        {
            return m_RecentHits.Contains(targetid);
        }

        public void ResetReports()
        {
            m_RecentHits.Clear();
        }

        /// <summary>
        /// Add a pair of hitboxes that intersected this frame to the list of pairs to solve.
        /// </summary>
        public void AddContact(HitboxFeeder a, HitboxFeeder b)
        {
            m_Contacts.Add(new ContactPair { a = a, b = b });
        }

        private void LateUpdate()
        {
            m_Contacts.Sort(ContactComparison);

            for (int i = 0; i < m_Contacts.Count; i++)
                m_Contacts[i].a.HandleContact(m_Contacts[i].b);

            m_Contacts.Clear();
        }

        private int ContactComparison(ContactPair x, ContactPair y)
        {
            return ContactDistance(x) - ContactDistance(y);
        }

        private int ContactDistance(ContactPair pair)
        {
            Vector3 aPos = pair.a.transform.position, bPos = pair.b.transform.position;
            float xDirection = Mathf.Sign(bPos.x - aPos.x);

            aPos.x -= xDirection * (pair.a.Collider.size.x / 2f);
            bPos.x += xDirection * (pair.b.Collider.size.x / 2f);

            return Mathf.RoundToInt(Vector3.Distance(aPos, bPos) * 1000f);
        }

        private int m_CurrentHitCount;
        private AnimationClip m_LastClip;
        public int CurrentAnimationUID { get { return m_CurrentId; } }
        private int m_CurrentId;
        private int m_PrivateUID;
        private int m_LastFrame;
        private void UpdateHitbox(AnimationEvent _event)
        {
            if (!string.IsNullOrEmpty(_event.stringParameter))
            {
                Profiler.BeginSample("HITBOXMANAGER: DESERIALIZE DATA");
                var animData = internalFrameData.Deserialize(_event.stringParameter);
                Profiler.EndSample();

                if (m_LastClip != _event.animatorClipInfo.clip)
                {
                    m_LastClip = _event.animatorClipInfo.clip;
                    m_CurrentId = m_PrivateUID++;
                    ResetReports();
                    m_CurrentHitCount = Mathf.Max(1, animData.numtargets);
                }

                m_LastFrame = _event.intParameter;


                UpdateHitbox(animData, Mathf.RoundToInt(_event.floatParameter), _event.intParameter);
            }
        }
        
        private Vector2Int m_NextOffset;
        private Vector2 m_OffsetStep;

        private void FixedUpdate()
        {
            if (m_OffsetStep != Vector2.zero)
            {
                var capsule = m_OffsetStep;
                transform.root.localPosition += new Vector3(capsule.x * m_UPP, capsule.y * m_UPP);
            }
        }

        private void UpdateHitbox(internalFrameData animdata, int anim, int frame)
        {
            if (m_Feeder == null || m_Feeder.Length < MAX_HITBOXES) { if (Application.isPlaying) Debug.Log("WARNING: hitbox feeder for object \"" + gameObject.name + "\" is null or insufficient"); return; }

            m_CurrentAnimation = anim;
            m_CurrentFrame = frame;
            var framedata = animdata.frame;
            var nextframedata = animdata.nextframe;

            Profiler.BeginSample("HITBOXMANAGER: RESET UNUSED COLLIDERS");
            if (framedata.collider != null)
                for (int i = framedata.collider.Length; i < m_Feeder.Length; i++)
                {
                    m_Feeder[i].Disable();
                }
            else
                for (int i = 0; i < m_Feeder.Length; i++)
                {
                    m_Feeder[i].Disable();
                }
            Profiler.EndSample();

            if (animdata.hasnextframe && nextframedata.smoothedOffset)
            {
#if UNITY_EDITOR
                if (!(Application.platform == RuntimePlatform.WindowsEditor && Application.isPlaying == false))
                    transform.root.localPosition += new Vector3(m_OffsetStep.x * m_UPP, m_OffsetStep.y * m_UPP);
#else
            transform.root.localPosition += new Vector3(m_OffsetStep.x * m_UPP, m_OffsetStep.y * m_UPP);
#endif


                m_NextOffset = nextframedata.capsuleOffset;
                if (m_Renderer != null && m_Renderer.flipX)
                    m_NextOffset.x *= -1;

                m_OffsetStep = m_NextOffset;
                m_OffsetStep /= (1f / Time.fixedDeltaTime) / animdata.framerate;
            }
            else
            {
                m_NextOffset = Vector2Int.zero;
                m_OffsetStep = Vector2.zero;
            }
            Profiler.BeginSample("HITBOXMANAGER: FEED COLLIDERS");
            if (framedata.collider != null)
                for (int i = 0; i < framedata.collider.Length; i++)
                {
                    var collider = framedata.collider[i];
                    var rect = collider.rect;

                    if (m_Renderer != null && m_Renderer.flipX)
                    {
                        rect.x *= -1;
                        rect.x -= rect.width;
                    }

                    m_Feeder[i].Feed(new Vector2(rect.width * m_UPP / m_Scale, rect.height * m_UPP / m_Scale),
                        new Vector2(rect.x * m_UPP / m_Scale + (rect.width * m_UPP / 2f / m_Scale), rect.y * m_UPP / m_Scale + (rect.height * m_UPP / 2f / m_Scale)),
                        m_CurrentId, collider.type, animdata.damage, animdata.strength, animdata.force, animdata.direction, animdata.hitfxuid);
                }
            Profiler.EndSample();
            if (framedata.smoothedOffset == true) return;
#if UNITY_EDITOR
            if (Application.platform == RuntimePlatform.WindowsEditor && Application.isPlaying == false) return;
#endif
            var capsule = framedata.capsuleOffset;
            if (m_Renderer != null && m_Renderer.flipX)
            {
                capsule.x *= -1;
            }

            if (capsule != Vector2Int.zero)
            {
                float x = capsule.x * m_UPP, y = capsule.y * m_UPP;
                transform.root.localPosition += new Vector3(x, y, 0);
            }

            m_Character.Poise = animdata.poise;
        }

        private int m_NumFrames;
        [SerializeField]
        private int m_CurrentAnimation;
        [SerializeField]
        private int m_CurrentFrame;
        [SerializeField]
        public int m_CurrentCollider;

        private void OnEnable()
        {
            var scale = transform.localScale;
            m_Scale = scale.x;

#if UNITY_EDITOR
            if (m_Animations == null || m_Animations.Length == 0) return;
            var animation = m_Animations[m_CurrentAnimation];
            if (animation.clip == null) return;
            m_NumFrames = Mathf.FloorToInt(animation.clip.length * animation.clip.frameRate);
            //if(m_Colliders == null)
            {
                m_Colliders = GetComponentsInChildren<BoxCollider2D>();
                if (m_Colliders == null || m_Colliders.Length < MAX_HITBOXES)
                {
                    m_Colliders = new BoxCollider2D[MAX_HITBOXES];
                    m_Feeder = new HitboxFeeder[MAX_HITBOXES];
                    for (int i = 0; i < m_Colliders.Length; i++)
                    {
                        var newGameObject = new GameObject("collider (" + i + ")");
                        newGameObject.transform.SetParent(transform, false);
                        var collider = newGameObject.AddComponent<BoxCollider2D>();
                        var feeder = newGameObject.AddComponent<HitboxFeeder>();

                        collider.isTrigger = true;
                        collider.enabled = false;
                        m_Colliders[i] = collider;
                        m_Feeder[i] = feeder;
                    }
                }
            }
#endif
        }

#if UNITY_EDITOR
        public int GetNumFrames(int animationID)
        {
            if (m_Animations == null || animationID >= m_Animations.Length) return 0;
            var animation = m_Animations[animationID];
            if (animation.clip == null) return 0;
            var curves = AnimationUtility.GetObjectReferenceCurveBindings(animation.clip);

            for (int i = 0; i < curves.Length; i++)
            {
                if(curves[i].propertyName.Equals("m_Sprite"))
                {
                    var keyframes = AnimationUtility.GetObjectReferenceCurve(animation.clip, curves[i]);

                    return keyframes.Length;
                }
            }

            Debug.LogWarning("No sprite keyframes have been found in the current animation.");

            return 0;
        }
#endif

        [StructLayout(LayoutKind.Explicit)]
        struct IntConverter
        {
            [FieldOffset(0)]
            public Int32 Value;
            [FieldOffset(0)]
            public Int16 LoValue;
            [FieldOffset(2)]
            public Int16 HiValue;
        }

        static public Vector2 DecodeIntToVector2(int value)
        {
            var data = new IntConverter { Value = value };

            return new Vector2(data.LoValue, data.HiValue);
        }

        static public int EncodeVector2ToInt(Vector2 value)
        {
            return new IntConverter
            {
                LoValue = (short)value.x,
                HiValue = (short)value.y
            }.Value;
        }

        /// <summary>
        /// Decode the data from an animation event to an origin -> direction.
        /// </summary>
        /// <param name="intParam">Int parameter passed by event</param>
        /// <param name="floatParam">Float parameter passed by event</param>
        /// <param name="origin">The local space origin of the gizmo</param>
        /// <param name="direction"> The forward direction of the gizmo</param>
        /// <param name="normalizeDirection"> Whether or not the direction vector output will be normalized</param>
        public void DecodeOriginAndDirection(int intParam, float floatParam, out Vector2 origin, out Vector2 direction, bool normalizeDirection = true)
        {
            origin = DecodeIntToVector2(intParam);
            direction = DecodeIntToVector2((int)floatParam) - origin;
            if (normalizeDirection)
                direction = Vector3.Normalize(direction);
            origin /= m_Renderer.sprite.pixelsPerUnit;
            direction.y *= -1f;
            origin.y *= -1f;
        }

#if UNITY_EDITOR
        public void UpdatePreview()
        {
            if (m_Animations == null || m_Animations.Length == 0 || m_CurrentAnimation >= m_Animations.Length) return;
            var animation = m_Animations[m_CurrentAnimation].clip;
            if (animation == null) return;
            animation.SampleAnimation(gameObject, m_CurrentFrame * (animation.length / GetNumFrames(m_CurrentAnimation)));
            UpdateHitbox(new internalFrameData(m_Animations[m_CurrentAnimation], m_CurrentFrame), m_CurrentAnimation, m_CurrentFrame);
        }

        private void OnDrawGizmos()
        {
            if (m_Animations == null || m_CurrentAnimation >= m_Animations.Length ||
                m_Animations[m_CurrentAnimation].framedata == null || m_CurrentFrame >= m_Animations[m_CurrentAnimation].framedata.Length) return;
            var framedata = m_Animations[m_CurrentAnimation].framedata[m_CurrentFrame];
            var collider = framedata.collider;

            for (int i = 0; i < collider.Length; i++)
            {
                var color = HitboxSettings.COLOR(collider[i].type);
                color.a = 0.75f;
                Gizmos.color = color;
                Rect rect = new Rect(collider[i].rect.x * m_UPP, collider[i].rect.y * m_UPP, collider[i].rect.width * m_UPP, collider[i].rect.height * m_UPP);

                if (m_Renderer != null && m_Renderer.flipX)
                {
                    rect.x *= -1;
                    rect.width *= -1;
                }

                Gizmos.DrawCube(new Vector3(transform.position.x + rect.x + rect.width / 2f, transform.position.y + rect.y + rect.height / 2f, transform.position.z), new Vector3(rect.width, rect.height, 1));
            }
        }
#endif
    }

#region Serialization
    public static class BinaryStructConverter
    {
        public static T FromByteArray<T>(byte[] bytes, int offset = 0) where T : struct
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int size = Marshal.SizeOf(typeof(T));
                ptr = Marshal.AllocHGlobal(size);
                Marshal.Copy(bytes, offset, ptr, size);
                object obj = Marshal.PtrToStructure(ptr, typeof(T));
                return (T)obj;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        public static byte[] ToByteArray<T>(T obj) where T : struct
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                int size = Marshal.SizeOf(typeof(T));
                ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(obj, ptr, true);
                byte[] bytes = new byte[size];
                Marshal.Copy(ptr, bytes, 0, size);
                return bytes;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
#endregion
}

