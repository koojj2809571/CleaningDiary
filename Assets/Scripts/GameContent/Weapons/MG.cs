using System;
using Base;
using Constant;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Weapons
{
    public class MG : BaseWeapon
    {
        protected override string Name => WeaponName.Mg;
        protected override AudioClip FireClip => Resources.Load<AudioClip>("Ak2MG3".AudioGunPath());
        protected override AudioClip ReloadClip => Resources.Load<AudioClip>("Reload".AudioGunPath());
        protected override float AttackRange => Random.Range(-AttackCtr.curInaccuracy, AttackCtr.curInaccuracy);
        protected override GameObject Bullet => Resources.Load<GameObject>("Bullet0".PrefabBulletPath());
    }
}
