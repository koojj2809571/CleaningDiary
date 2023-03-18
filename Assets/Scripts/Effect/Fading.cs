using System;
using UnityEngine;

namespace Effect
{
    public class Fading : MonoBehaviour
    {
        public SpriteRenderer sp;
        public float fadeSpeed;
        public float waiTime;
        private bool _isStartFading;

        private void Start()
        {
            Invoke(nameof(StartFading),waiTime);
        }

        private void Update()
        {
            if (_isStartFading)
            {
                var color = sp.color;
                color.a -= fadeSpeed * Time.deltaTime;
                color.a = Mathf.Clamp(color.a, 0,1);
                sp.color = color;
                if (color.a <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void StartFading()
        {
            _isStartFading = true;
        }
    }
}