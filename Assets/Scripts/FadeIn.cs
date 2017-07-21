using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FadeIn : MonoBehaviour
    {
        public float FadeSpeed;
        public float DelayBeforeStart;
        public SpriteRenderer SpriteRenderer;

        private float _alpha = 0f;
        private float _delayBeforeShowingRemaining = 0f;

        void Start()
        {
            _delayBeforeShowingRemaining = DelayBeforeStart;
            SetAlpha(_alpha);
        }

        void Update()
        {
            if (_delayBeforeShowingRemaining > 0f)
            {
                _delayBeforeShowingRemaining -= Time.deltaTime;
            }
            else
            {
                _alpha += FadeSpeed * Time.deltaTime;
                SetAlpha(_alpha);
            }
        }

        private void SetAlpha(float alpha)
        {
            var color = SpriteRenderer.material.color;
            color.a = alpha;
            SpriteRenderer.material.color = color;
        }
    }
}