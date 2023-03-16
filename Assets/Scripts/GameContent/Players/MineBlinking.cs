using UnityEngine;

namespace GameContent.Players
{
    public class MineBlinking : MonoBehaviour
    {

        public GameObject blink;
        public float time;
        private float _timer;
    
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
