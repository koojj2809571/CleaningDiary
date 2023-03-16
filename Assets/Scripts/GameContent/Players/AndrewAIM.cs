using UnityEngine;

namespace GameContent.Players
{
    public class AndrewAIM : MonoBehaviour
    {
        [SerializeField] private Transform cameraTrans;
        [SerializeField] private GameObject mark;
        [SerializeField] private float nearestDis;
        [SerializeField] private float farthestDis;

        private Andrew _player;
        private Enemy.Enemy _nearestEnemy;
        private RaycastHit2D _rayHit;
        private float _distance;
        private LayerMask _layerMask;

        private void Start()
        {
            if (Camera.main != null) cameraTrans = Camera.main.gameObject.transform;
            _player = GameManager.Instance.andrew;
            mark.SetActive(false);
            nearestDis = farthestDis = 10;
            _layerMask = ~(1 << 9) & ~(1 << 2);
        }

        private void Update()
        {
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
                _rayHit = Physics2D.Raycast(
                    position,
                    enemyPosition - position,
                    10,
                    _layerMask
                );

                if (_rayHit.collider == null) continue;
                if (_rayHit.collider.gameObject.CompareTag("Wall")) continue;
                _distance = Vector3.Distance(position, enemyPosition);
                if (_distance < farthestDis && _distance < nearestDis)
                {
                    nearestDis = _distance;
                    _nearestEnemy = enemy;
                }
            }

            MarkChange();

            cameraTrans.transform.position = new Vector3(position.x, position.y, -7.8f);
        }

        private void MarkChange()
        {
            if (_nearestEnemy == null)
            {
                transform.rotation = Quaternion.Euler(0, 0, _player.moveAngle);
                mark.SetActive(false);
                return;
            }

            var enemyTrans = _nearestEnemy.transform;
            var position = enemyTrans.position;
            // mark.transform.position = position;
            mark.transform.SetParent(enemyTrans);
            mark.transform.localPosition = Vector3.zero;
            mark.transform.rotation = transform.rotation;
            mark.SetActive(true);
            Vector3 moveDir = position - transform.position;
            if (moveDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            }
            
            TestWall();
            
        }

        private void TestWall()
        {
            if (!_rayHit.collider.CompareTag("Wall")) return;
            nearestDis = 10;
            _nearestEnemy = null;
        }

        private void MarkChangeLerp()
        {
            mark.transform.position = Vector3.Lerp(mark.transform.position, _rayHit.collider.transform.position,
                4 * Time.deltaTime);
            if (Vector3.Distance(mark.transform.position, _rayHit.collider.transform.position) > 0.2)
            {
                mark.transform.position += _rayHit.collider.transform.position * (2 * Time.deltaTime);
            }
            else
            {
                mark.transform.position = _rayHit.collider.transform.position;
            }
        }
    }
}