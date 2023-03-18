using Base;
using GameContent.Players;
using UnityEngine;

namespace GameContent.Enemy
{
    public class ShootAbleEnemy : BaseEnemy
    {
        public GameObject bullet;
        public Transform shootPoint;
        public float attackCd;
        protected float CurCd;
        public int magazine;
        protected int CurMagazine;
        public float reload;
        protected float CurReload;

        protected AudioSource Audio;
        public AudioClip shootClip;

        public float inaccuracy;

        protected override void Start()
        {
            base.Start();
            Audio = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            CalculateCd();
            base.Update();
        }

        protected void CalculateCd()
        {
            CurCd -= Time.deltaTime;
            CurReload -= Time.deltaTime;
            if (CurReload <= 0 && CurMagazine <= 0)
            {
                CurMagazine = magazine;
            }
        }

        protected override void LookAtPlayerAndAttack()
        {
            base.LookAtPlayerAndAttack();
            if (CurCd <= 0 && CurMagazine > 0)
            {
                Audio.PlayOneShot(shootClip,GameManager.Instance.recordRunData.volume);
                ShootBullet();
                CurCd = attackCd;
                CurMagazine -= 1;
                if (CurMagazine <= 1)
                {
                    CurReload = reload;
                }
            }
        }

        protected virtual void ShootBullet()
        {
            var offset = Random.Range(-inaccuracy, inaccuracy);
            var angle = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + offset);
            Bullet goBullet = Instantiate(bullet, shootPoint.position, angle).GetComponent<Bullet>();
            goBullet.SetFromPlayer(false);
        }
    }
}