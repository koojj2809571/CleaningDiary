using UnityEngine;

namespace GameContent.Players
{
    public class MineBlinking : MonoBehaviour
    {

        public GameObject blink;
        public bool isFromPlayer;
        public float time;
        private float _timer;
        private SpriteRenderer _sp;

        public void SetFromPlayer(bool from)
        {
            isFromPlayer = from;
            if (!isFromPlayer)
            {
                _sp = transform.Find("circle").GetComponent<SpriteRenderer>();
                _sp.color = new Color(0.1f, 0.1f, 0.1f);
            }
        }
        
        // Start is called before the first frame update
        void Start()
        {
            time = 0.1f;
        }

        // Update is called once per frame
        void Update()
        {
            if (_timer <= 0)
            {
                blink.SetActive(!blink.activeSelf);
                _timer = time;
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
    }
}
