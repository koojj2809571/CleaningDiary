using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Players
{
    public class Turret : MonoBehaviour
    {
        public GameObject mark;
        private float _nearestDis;
        private float _farthestDis;
        private Enemy.Enemy _nearestEnemy;
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
            
            var position = transform.position;
            for (int i = 0; i < GameManager.Instance.EnemyCount; i++)
            {
                Enemy.Enemy enemy = GameManager.Instance.enemies[i];
                if (enemy == null) continue;

                var enemyPosition = enemy.transform.position;
                /*
                 * 射线遮罩：以第9层为例
                 * 1、打开一层：layerMask = 1 << 9;
                 * 2、除了某一层打开其他所有层：layerMask = ~(1 << 9);
                 * 3、打开所有层：layerMask = ~(1 << 0);
                 * 4、打开某几层：layerMask = (1 << 1)|(1 << 2)|(1 << 3)|....;
                 */
                _hit = Physics2D.Raycast(
                    position,
                    enemyPosition - position,
                    7,
                    _mask
                );

                if (_hit.collider == null) continue;
                if (_hit.collider.gameObject.CompareTag("Wall")) continue;
                _distance = Vector3.Distance(position, enemyPosition);
                if (_distance < _farthestDis && _distance < _nearestDis)
                {
                    _nearestDis = _distance;
                    _nearestEnemy = enemy;
                }
            }

            MarkChange();
        }
        
        private void MarkChange()
        {
            if (_nearestEnemy == null)
            {
                mark.SetActive(false);
                return;
            }

            var enemyTrans = _nearestEnemy.transform;
            var position = enemyTrans.position;
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
                Instantiate(bulletGo, shootPoints[0].position, Quaternion.Euler(0, 0, angle));
                if (_turretLv >= 3)
                {
                    Instantiate(bulletGo, shootPoints[1].position, Quaternion.Euler(0, 0, angle));
                }
                _audio.PlayOneShot(shootClip,GameManager.Instance.recordRunData.volume * 0.6f);
                _curAttackColdDown = attackColdDown;
                bullets -= 1;
            }
            TestWall();
            
        }

        private void TestWall()
        {
            if (!_hit.collider.CompareTag("Wall")) return;
            _nearestDis = 7;
            _nearestEnemy = null;
        }
    }
}