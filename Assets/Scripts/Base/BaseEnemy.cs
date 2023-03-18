using GameContent;
using GameContent.Players;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Base
{
    public class BaseEnemy : MonoBehaviour
    {
        private Andrew _player;
        private Transform _playerTrans;
        private Vector3 _playerLastPos;
        private Rigidbody2D _rb;

        private bool _follow;
        protected RaycastHit2D Hit;
        private LayerMask _mask;
        public float moveSpeed;
        public float curHp;
        public ListenAbleValue<float> HpObserver;
        public int reward;

        public GameObject[] attackGos; //受到攻击特效
        public GameObject finishedGos; //被终结特效
        public GameObject attackParticle; //被攻击粒子特效
        public GameObject explosion; //踩地雷爆炸特效

        public AudioClip die;
        private AudioSource _audio;

        public Vector3 CurPos => transform.position;

        public Quaternion CurRotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }
        public Vector3 PlayerPos => _playerTrans.position;

        protected virtual void Start()
        {
            _audio = GetComponent<AudioSource>();
            
            _rb = GetComponent<Rigidbody2D>();
            _mask = ~(1 << 10) & ~(1 << 2);
            GameManager.Instance.enemies.Add(this);
            HpObserver = new ListenAbleValue<float>(ref curHp);
            AddObserver();
        }

        protected virtual void Update()
        {
            if (_player == null)
            {
                _player = GameManager.Instance.andrew;
                _playerTrans = _player.transform;
            }

            if (_player == null) return;
            Hit = Physics2D.Raycast(CurPos, PlayerPos - CurPos, 5, _mask);
            SearchAndFollowPlayer();
            Move();
            
        }
        
        private void AddObserver()
        {
            HpObserver.SetObserve(HpDelegate);
        }

        private void HpDelegate(float value)
        {
            curHp = value;
            if (value <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            _audio.PlayOneShot(die,GameManager.Instance.recordRunData.volume);
            Instantiate(finishedGos, CurPos, CurRotation);
            GameManager.Instance.andrew.curKill += 1;
            GameManager.Instance.recordRunData.money += reward;
            GameManager.Instance.enemies.Remove(this);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Bullet"))
            {
                if (!col.gameObject.GetComponent<Bullet>().isFromPlayer)return;
                var particle = Instantiate(attackParticle, CurPos, Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + 180));
                Destroy(particle,1f);

                if (col.gameObject.name.Contains("0"))
                {
                    HpObserver.Value -= 10;
                    Instantiate(attackGos[0], CurPos,
                        Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + Random.Range(-15, 15)));
                }
                if (col.gameObject.name.Contains("1"))
                {
                    HpObserver.Value -= 50;
                    Instantiate(attackGos[1], CurPos,
                        Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z));
                }
                if (col.gameObject.name.Contains("2"))
                {
                    HpObserver.Value -= 20;
                    Instantiate(attackGos[0], CurPos,
                        Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + Random.Range(-20, 20)));
                }
                if (col.gameObject.name.Contains("3"))
                {
                    HpObserver.Value -= 40;
                    Instantiate(attackGos[2], CurPos,
                        Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z));
                }
                _rb.AddRelativeForce(new Vector2(0, _player.attack.repulsion));
                
            }
            if (col.CompareTag("Mine"))
            {
                if (!col.gameObject.GetComponent<MineBlinking>().isFromPlayer)return;
                Instantiate(explosion, CurPos, Quaternion.identity);
                HpObserver.Value -= 70;
                Destroy(col.gameObject);
            }
                
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                _player.ObserveHp.Value -= 0.5f;
                _player.delayTimer = _player.delayRegenHp;
            }
        }

        private void SearchAndFollowPlayer()
        {
            CheckFollow();
            CheckHitTarget();
        }

        private void CheckFollow()
        {
            var dis = Vector3.Distance(_playerLastPos, CurPos);
            if (_follow && dis <= 0.1)
            {
                _follow = false;
            }
        }

        private void CheckHitTarget()
        {
            if (Hit.collider == null) return;
            if (!Hit.collider.CompareTag("Wall"))
            {
                LookAtPlayerAndAttack();
            }
            else if (_follow)
            {
                Vector3 moveDir = _playerLastPos - CurPos;
                if (moveDir != Vector3.zero)
                {
                    float last = Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                    CurRotation = Quaternion.AngleAxis(last, Vector3.forward);
                }
            }
        }

        protected virtual void LookAtPlayerAndAttack()
        {
            ChasePlayer();
        }

        protected void ChasePlayer()
        {
            Vector3 moveDir = PlayerPos - CurPos;
            if (moveDir != Vector3.zero)
            {
                float last = Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                CurRotation = Quaternion.AngleAxis(last, Vector3.forward);
                _follow = true;
                _playerLastPos = PlayerPos;
            }
        }
        
        private void Move()
        {
            if (Hit.collider != null && Hit.collider.CompareTag("Player"))
            {
                _rb.AddRelativeForce(new Vector2(0,moveSpeed));
                return;
            }
            if (_follow)
            {
                _rb.AddRelativeForce(new Vector2(0,moveSpeed));
            }
        }
    }
}