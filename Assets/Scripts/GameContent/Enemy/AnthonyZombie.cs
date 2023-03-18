using Base;
using GameContent.Players;
using UnityEngine;

namespace GameContent.Enemy
{
    public class AnthonyZombie : ShootAbleEnemy
    {

        public GameObject turret;
        public GameObject mine;

        public int mineCd;
        public int turretCd;
        public int dividerCd;
        public float curMineCd;
        public float curTurretCd;
        public float curDividerCd;

        public bool see;
        public bool seePlayer;

        protected override void Update()
        {
            base.Update();
            
            if (Hit.collider != null && Hit.collider.CompareTag("Player"))
            {
                UseItem();
            }
        }

        private void UseItem()
        {
            if (curDividerCd > 0)
            {
                curDividerCd -= Time.deltaTime;
            }
            
            if (curMineCd <= 0 && curDividerCd <= 0)
            {
                curMineCd = mineCd;
                curDividerCd = dividerCd;
                MineBlinking goMine = Instantiate(mine, CurPos, Quaternion.identity).GetComponent<MineBlinking>();
                goMine.SetFromPlayer(false);
            }
            else
            {
                curMineCd -= Time.deltaTime;
            }
            
            if (curTurretCd <= 0 && curDividerCd <= 0)
            {
                curTurretCd = turretCd;
                curDividerCd = dividerCd;
                Turret goTurret = Instantiate(turret, CurPos, Quaternion.identity).GetComponent<Turret>();
                goTurret.SetFromPlayer(false);
            }
            else
            {
                curTurretCd -= Time.deltaTime;
            }

        }

        protected override void ShootBullet()
        {
            var shootPos = shootPoint.position;
            var baseAngle = transform.rotation.eulerAngles.z;
            
            CreateBullet(shootPos, baseAngle - inaccuracy);
            CreateBullet(shootPos, baseAngle - 0.05f * inaccuracy);
            CreateBullet(shootPos, baseAngle);
            CreateBullet(shootPos, baseAngle + 0.05F * inaccuracy);
            CreateBullet(shootPos, baseAngle + inaccuracy);
        }

        private void CreateBullet(Vector3 pos, float angle)
        {
            Bullet goBullet = Instantiate(bullet, pos, Quaternion.Euler(0,0, angle)).GetComponent<Bullet>();
            goBullet.SetFromPlayer(false);
        }
    }
}