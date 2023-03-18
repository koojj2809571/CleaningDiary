using System;
using Base;
using Constant;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Weapons
{
    public class Pistol : BaseWeapon
    {
        protected override string Name => WeaponName.Pistol;
        protected override AudioClip FireClip => Resources.Load<AudioClip>("Pistol1".AudioGunPath());
        protected override AudioClip ReloadClip => Resources.Load<AudioClip>("Reload".AudioGunPath());
        protected override float AttackRange => Random.Range(-AttackCtr.curInaccuracy, AttackCtr.curInaccuracy);
        protected override GameObject CurBullet => Resources.Load<GameObject>("Bullet0".PrefabBulletPath());
    }
}