using System;
using Base;
using Constant;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Weapons
{
    public class Sniper : BaseWeapon
    {
        protected override string Name => WeaponName.Sniper;
        protected override AudioClip FireClip => Resources.Load<AudioClip>("Snipe4".AudioGunPath());
        protected override AudioClip ReloadClip => Resources.Load<AudioClip>("Reload".AudioGunPath());
        protected override float AttackRange => Random.Range(-AttackCtr.curInaccuracy, AttackCtr.curInaccuracy);
        protected override GameObject Bullet => Resources.Load<GameObject>("Bullet1".PrefabBulletPath());
    }
}

