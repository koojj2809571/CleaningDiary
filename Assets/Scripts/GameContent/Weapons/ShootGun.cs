using System;
using Base;
using Constant;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Weapons
{
    public class ShootGun : BaseWeapon
    {
        protected override string Name => WeaponName.ShootGun;
        protected override AudioClip FireClip => Resources.Load<AudioClip>("Shotgun5".AudioGunPath());
        protected override AudioClip ReloadClip => Resources.Load<AudioClip>("Reload".AudioGunPath());
        protected override float AttackRange => Random.Range(-AttackCtr.curInaccuracy, AttackCtr.curInaccuracy);
        protected override GameObject Bullet => Resources.Load<GameObject>("Bullet2".PrefabBulletPath());

        protected override void Fire()
        {
            CreateBullet(-AttackCtr.curInaccuracy);
            CreateBullet(-0.5f * AttackCtr.curInaccuracy);
            CreateBullet(0);
            CreateBullet(0.5f * AttackCtr.curInaccuracy);
            CreateBullet(AttackCtr.curInaccuracy);
        }
    }
}