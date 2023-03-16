using GameContent;
using GameContent.Players;
using UnityEngine;
using UnityEngine.Serialization;
using Util.ext;

namespace Base
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public ParticleSystem BulletFlash { get; set; }
        protected abstract string Name { get; }
        protected abstract AudioClip FireClip { get; }
        protected abstract AudioClip ReloadClip { get; }
        protected abstract float AttackRange { get; }
        protected abstract GameObject Bullet { get; }
        protected virtual bool HasFlash => true;

        private WeaponProperties Properties => GameManager.Instance.recordRunData.WeaponPropertiesDic[Name];
        private PlayAttack Attack => GameManager.Instance.andrew.attack;

        protected PlayAttack AttackCtr => GameManager.Instance.andrew.attack;

        public void Shoot(AudioSource gunAudio)
        {
            if (AttackCtr.curTotalBullets <= 0 || AttackCtr.curMagazine <= 0 ||
                !(AttackCtr.curAttackColdDown <= 0)) return;
            gunAudio.PlayOneShot(FireClip, GameManager.Instance.recordRunData.volume);
            Fire();
            
            if (HasFlash)
            {
                BulletFlash.Play();
            }
            AttackCtr.curMagazine--;
            AttackCtr.curTotalBullets--;
            AttackCtr.curAttackColdDown = (float)Properties.attackCD;
            float curInaccuracy = AttackCtr.curInaccuracy;
            AttackCtr.curInaccuracy = curInaccuracy.PlusLimit((float)Properties.recoilForce, (float)Properties.maxInaccuracy);

            if (AttackCtr.curMagazine <= 0 && AttackCtr.curReloadColdDown <= 0)
            {
                Reload(gunAudio);
            }
        }

        protected virtual void Fire()
        {
            CreateBullet(AttackRange);
        }
        
        protected void CreateBullet(float offset)
        {
            var initAngle = Quaternion.Euler(0, 0, Attack.transform.eulerAngles.z + offset);
            Instantiate(Bullet, Attack.transform.position, initAngle);
        }

        public void Reload(AudioSource gunAudio)
        {
            if (AttackCtr.curMagazine >= Properties.magazine) return;
            gunAudio.PlayOneShot(ReloadClip,GameManager.Instance.recordRunData.volume);
            AttackCtr.curReloadColdDown = (float)Properties.reload;
            AttackCtr.curMagazine = 0;
        }
    }
}