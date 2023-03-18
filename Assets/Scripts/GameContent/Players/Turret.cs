using Base;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Players
{
    public class Turret : MonoBehaviour
    {
        public GameObject mark;
        private float _nearestDis;
        private float _farthestDis;
        private MonoBehaviour _nearestTarget;
        private RaycastHit2D _hit;
        private LayerMask _mask;
        private AudioSource _audio;
        public float attackColdDown;
        private float _curAttackColdDown;
        public int bullets;
        public GameObject bulletGo;
        public Transform[] shootPoints;
        private int _turretLv;
        public float inaccuracy;
        public AudioClip shootClip;
        private float _distance;
        public bool isFromPlayer;
        private SpriteRenderer _sp;

        private void Start()
        {
            
            _nearestDis = _farthestDis = 7;
            _audio = GetComponent<AudioSource>();
            mark.SetActive(false);
            _mask = ~(1 << 9)&~(1 << 2);
            _turretLv = GameManager.Instance.recordRunData.gunLevel / 2;
            if (_turretLv == 0)
            {
                _turretLv = 1;
            }
        }

        private void Update()
        {
            if (_curAttackColdDown > 0)
            {
                _curAttackColdDown -= Time.deltaTime;
            }
            
            if (isFromPlayer)
            {
                for (int i = 0; i < GameManager.Instance.EnemyCount; i++)
                {
                    FindTarget(GameManager.Instance.enemies[i]);
                }
            }
            else
            {
                _mask = ~(1 << 10) & ~(1 << 2);
                FindTarget(GameManager.Instance.andrew);
            }

            MarkChange();
        }

        private void FindTarget(MonoBehaviour target)
        {
            if (target == null) return;
            var position = transform.position;
            var targetPosition = target.transform.position;
            /*
             * 射线遮罩：以第9层为例
             * 1、打开一层：layerMask = 1 << 9;
             * 2、除了某一层打开其他所有层：layerMask = ~(1 << 9);
             * 3、打开所有层：layerMask = ~(1 << 0);
             * 4、打开某几层：layerMask = (1 << 1)|(1 << 2)|(1 << 3)|....;
             */
            _hit = Physics2D.Raycast(
                position,
                targetPosition - position,
                7,
                _mask
            );

            if (_hit.collider == null) return;
            if (_hit.collider.gameObject.CompareTag("Wall")) return;
            _distance = Vector3.Distance(position, targetPosition);
            if (_distance < _farthestDis && _distance < _nearestDis)
            {
                _nearestDis = _distance;
                _nearestTarget = target;
            }
        }

        public void SetFromPlayer(bool from)
        {
            isFromPlayer = from;
            if (!isFromPlayer)
            {
                _sp = transform.Find("Gun").GetComponent<SpriteRenderer>();
                _sp.color = new Color(0.8f, 0.1f, 0.1f);
            }
        }
        
        private void MarkChange()
        {
            if (_nearestTarget == null)
            {
                mark.SetActive(false);
                return;
            }

            var targetTrans = _nearestTarget.transform;
            var position = targetTrans.position;
            mark.transform.position = position;
            mark.SetActive(true);
            Vector3 moveDir = position - transform.position;
            if (moveDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            }

            if (_curAttackColdDown <= 0 && bullets > 0)
            {
                float angle = transform.rotation.eulerAngles.z + Random.Range(-inaccuracy,inaccuracy);
                Bullet goBullet1 = Instantiate(bulletGo, shootPoints[0].position, Quaternion.Euler(0, 0, angle)).GetComponent<Bullet>();
                goBullet1.SetFromPlayer(isFromPlayer);
                if (_turretLv >= 3)
                {
                    Bullet goBullet2 = Instantiate(bulletGo, shootPoints[1].position, Quaternion.Euler(0, 0, angle)).GetComponent<Bullet>();
                    goBullet2.SetFromPlayer(isFromPlayer);
                }
                _audio.PlayOneShot(shootClip,GameManager.Instance.recordRunData.volume * 0.6f);
                _curAttackColdDown = attackColdDown;
                bullets -= 1;
            }
            TestWall();
            
        }

        private void TestWall()
        {
            if (_hit.collider == null) return;
            if (!_hit.collider.CompareTag("Wall")) return;
            _nearestDis = 7;
            _nearestTarget = null;
        }
    }
}