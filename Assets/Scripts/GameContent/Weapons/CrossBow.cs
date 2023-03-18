using System;
using Base;
using Constant;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Weapons
{
    public class CrossBow : BaseWeapon
    {
        protected override string Name => WeaponName.CrossBow;
        protected override AudioClip FireClip => Resources.Load<AudioClip>("Crossbow6".AudioGunPath());
        protected override AudioClip ReloadClip => Resources.Load<AudioClip>("Reload".AudioGunPath());
        protected override float AttackRange => Random.Range(-AttackCtr.curInaccuracy, AttackCtr.curInaccuracy);
        protected override GameObject CurBullet => Resources.Load<GameObject>("Bullet3".PrefabBulletPath());
    }
}

