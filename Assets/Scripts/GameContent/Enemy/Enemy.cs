using System;
using Base;
using GameContent.Players;
using UnityEngine;

namespace GameContent.Enemy
{
    public class Enemy : BaseEnemy
    {
        private Andrew _player;
        private Transform _playerTrans;
        private Vector3 _playerLastPos;
        private Rigidbody2D _rb;

        private bool _follow;
        private RaycastHit2D _hit;
        private LayerMask _mask;
        public float moveSpeed;
        public float curHp;
        public int maxHp;
        public int reward;

        public GameObject[] attackGos; //受到攻击特效
        public GameObject finishedGos; //被终结特效
        public GameObject attackParticle; //被攻击粒子特效
        public GameObject explosion; //踩地雷爆炸特效

        public Vector3 CurPos => transform.position;
        public Vector3 PlayerPos => _playerTrans.position;

        private void Start()
        {
            _player = GameManager.Instance.andrew;
            _playerTrans = _player.transform;
            curHp = 1.0f;
            maxHp = (int)(curHp * 100);
            _rb = GetComponent<Rigidbody2D>();
            _mask = ~(1 << 10) & ~(1 << 2);
            GameManager.Instance.enemies.Add(this);
        }

        private void Update()
        {
            _hit = Physics2D.Raycast(CurPos, PlayerPos - CurPos, 5, _mask);
            SearchAndFollowPlayer();
            Move();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Bullet"))
            {
                Instantiate(attackParticle, CurPos, Quaternion.Euler(0, 0, _playerTrans.transform.rotation.eulerAngles.z));
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
            if (!_hit.collider.CompareTag("Wall"))
            {
                Vector3 moveDir = PlayerPos - CurPos;
                if (moveDir != Vector3.zero)
                {
                    float last = Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(last, Vector3.forward);
                    _follow = true;
                    _playerLastPos = PlayerPos;
                }
            }
            else if (_follow)
            {
                Vector3 moveDir = _playerLastPos - CurPos;
                if (moveDir != Vector3.zero)
                {
                    float last = Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(last, Vector3.forward);
                }
            }
            
        }
        
        private void Move()
        {
            if (_hit.collider!=null)
            {
                if (_hit.collider.CompareTag("Player"))
                {
                    _rb.AddRelativeForce(new Vector2(0,moveSpeed));
                }
            }
            if (_follow)
            {
                _rb.AddRelativeForce(new Vector2(0,moveSpeed));
            }
        }
    }
}