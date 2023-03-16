using GameContent.Items;
using UnityEngine;
using Util.ext;

namespace GameContent.Players
{
    public class Andrew : MonoBehaviour
    {
        [SerializeField] private GameObject bloodTrail;
        [SerializeField] private GameObject bloodParticle;
        [SerializeField] private AudioClip bonus;

        public bool isMoving;
        [SerializeField] public float moveAngle;
        [SerializeField] public float moveSpeed;
        [SerializeField] public int maxHp;
        [SerializeField] public int regenHpSpeed;
        [SerializeField] public int delayRegenHp;
        [SerializeField] public float curHp;
        public int killTarget;
        public int curKill;

        private float _delayTimer;
        private Rigidbody2D _playerRb;
        private AudioSource _audio;

        [HideInInspector] public PlayAttack attack;

        private void Awake()
        {
        }

        private void OnEnable()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.andrew = this;
            maxHp = 100;
            moveSpeed = 5;
            regenHpSpeed = delayRegenHp = 1;
            _audio = GetComponent<AudioSource>();
            _playerRb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            PlayerMoveInput();
            RegenHp();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            OnItemTrigger(col.gameObject, "Box");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            OnItemTrigger(col.gameObject, "OneOffItem");
            
            string colTagStr = col.tag;
            switch (colTagStr)
            {
                case "Bullet":
                    break;
            }
        }

        private void OnItemTrigger(GameObject go, string triggerTag)
        {
            if (go.CompareTag(triggerTag))
            {
                var item = ItemFactory.CreateItem(go);
                if (item == null) return;
                item.triggerGo = go;
                item.OnTouched(_audio,bonus);
            }
        }
        
        private void PlayerMoveInput()
        {
            isMoving = false;
            if (Input.GetKey(KeyCode.W))
            {
                PlayerMove(Vector2.up, 0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                PlayerMove(Vector2.down, 180);
            }

            if (Input.GetKey(KeyCode.A))
            {
                PlayerMove(Vector2.left, 90);
            }

            if (Input.GetKey(KeyCode.D))
            {
                PlayerMove(Vector2.right, -90);
            }

        }

        private void PlayerMove(Vector2 v, float angle)
        {
            _playerRb.AddForce(v * moveSpeed / attack.WeightFactory * (400 * Time.deltaTime));
            moveAngle = angle;
            isMoving = true;
        }

        private void RegenHp()
        {
            if (_delayTimer < 0 && curHp < maxHp)
            {
                curHp += regenHpSpeed * Time.deltaTime;
            }
            else if (_delayTimer > 0)
            {
                _delayTimer -= Time.deltaTime;
            }

            curHp = curHp.MaxLimit(maxHp);
        }
    }
}