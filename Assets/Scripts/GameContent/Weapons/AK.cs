using System;
using Base;
using Constant;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Weapons
{
    public class AK : BaseWeapon
    {
        protected override string Name => WeaponName.Ak;
        protected override AudioClip FireClip => Resources.Load<AudioClip>("Ak2MG3".AudioGunPath());
        protected override AudioClip ReloadClip => Resources.Load<AudioClip>("Reload".AudioGunPath());
        protected override float AttackRange => Random.Range(-AttackCtr.curInaccuracy, AttackCtr.curInaccuracy);
        protected override GameObject CurBullet => Resources.Load<GameObject>("Bullet0".PrefabBulletPath());
    }
}
