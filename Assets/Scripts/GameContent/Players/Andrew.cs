using GameContent.Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Util.ext;

namespace GameContent.Players
{
    public class Andrew : MonoBehaviour
    {
        [SerializeField] private GameObject bloodTrail;
        [SerializeField] private GameObject bloodParticle;
        [SerializeField] private GameObject deadGos;
        [SerializeField] private AudioClip bonus;

        public bool isMoving;
        [SerializeField] public float moveAngle;
        [SerializeField] public float moveSpeed;
        [SerializeField] public int maxHp;
        [SerializeField] public int regenHpSpeed;
        [SerializeField] public int delayRegenHp;
        [SerializeField] public float curHp;
        public ListenAbleValue<float> ObserveHp;
        public int killTarget;
        public int curKill;

        public float delayTimer;
        private Rigidbody2D _playerRb;
        private AudioSource _audio;
        public GameObject explosion;

        [HideInInspector] public PlayAttack attack;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.andrew = this;
            maxHp = 100;
            moveSpeed = 5;
            regenHpSpeed = delayRegenHp = 1;
            _audio = GetComponent<AudioSource>();
            _playerRb = GetComponent<Rigidbody2D>();
            ObserveHp = new ListenAbleValue<float>(ref curHp);
            AddObserver();
        }

        // Update is called once per frame
        void Update()
        {
            PlayerMoveInput();
            RegenHp();
        }
        
        private void AddObserver()
        {
            ObserveHp.SetObserve(HpObserver);
        }

        private void HpObserver(float hp)
        {
            curHp = hp;
            if (hp <= 0)
            {
                GameManager.Instance.recordRunData.SaveMoney(null);
                Instantiate(deadGos, transform.position, Quaternion.identity);
                GameManager.Instance.SwitchScene(0, 2f);
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            OnItemTrigger(col.gameObject, "Box");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            OnItemTrigger(col.gameObject, "OneOffItem");

            if (col.CompareTag("Bullet"))
            {
                if (col.gameObject.GetComponent<Bullet>().isFromPlayer) return;
                var angle = Quaternion.Euler(0, 0, col.transform.rotation.z + 180);
                var position = transform.position;
                var des = Instantiate(bloodParticle, position, angle);
                Destroy(des,1f);
                Instantiate(bloodTrail, position, angle);
                Destroy(col.gameObject);
                ObserveHp.Value -= 10;
            }

            if (col.CompareTag("Mine"))
            {
                if (col.gameObject.GetComponent<MineBlinking>().isFromPlayer) return;
                Instantiate(explosion, transform.position, Quaternion.identity);
                ObserveHp.Value -= 30;
                Destroy(col.gameObject);
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
            if (delayTimer < 0 && ObserveHp.Value < maxHp)
            {
                ObserveHp.Value += regenHpSpeed * Time.deltaTime;
            }
            else if (delayTimer > 0)
            {
                delayTimer -= Time.deltaTime;
            }

            ObserveHp.Value = ObserveHp.Value.MaxLimit(maxHp);
        }
    }
}