using System;
using Base;
using Constant;
using GameContent.Weapons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Util.ext;
using Random = UnityEngine.Random;

namespace GameContent.Players
{
    public class PlayAttack : MonoBehaviour
    {
        public GameObject[] turrets;
        private AudioSource _audio;
        public GameObject rightLine;
        public GameObject leftLine;
        private Andrew _player;
        private WeaponProperties _curWeapon;
        private BaseWeapon _curWeaponScript;
        private GameObject _curWeaponGo;
        public ParticleSystem bulletFlash;
        public GameObject mineGo;

        public float maxInaccuracy; //最大不精确度
        public float minInaccuracy; //最小不精确度
        public float recoilForce; //后坐力(攻击导致影响射击精度)
        public float attackColdDown; //攻击CD
        public float destabilization; //不稳定性(移动导致影响射击精度)
        public float aimingDeSpeed; //瞄准精度减少速度
        public int magazine; //弹夹装弹量
        public int totalBullets; //人物可携带弹量
        public float reload; // 装弹时间
        public float weight; // 枪重(影响移动速度)
        public int repulsion; // 击退距离

        public float curInaccuracy;
        public float curDestabilization;
        public float curAttackColdDown;
        public float curReloadColdDown;
        public int curMagazine;
        public int curTotalBullets;
        public bool hasTurret;
        public int curMine;
        public int carryMineSize;
        private int _turretLv;

        public float WeightFactory => 1 + 0.1f * weight; 

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            _player = GameManager.Instance.andrew;
            _turretLv = GameManager.Instance.recordRunData.gunLevel / 2;
            if (_turretLv == 0)
            {
                _turretLv = 1;
            }
            hasTurret = true;
            curMine = carryMineSize = 3;

            _curWeapon = GameManager.Instance.recordRunData.CurWeapon();

            var weapon = Resources.Load<GameObject>(_curWeapon.name.PrefabWeaponsPath());
            _curWeaponGo = Instantiate(weapon, _player.transform.position, Quaternion.identity);
            _curWeaponScript = WeaponFactory.CreateItem(_curWeaponGo);
            _curWeaponScript.BulletFlash = bulletFlash;
            _curWeaponGo.transform.SetParent(transform);

            maxInaccuracy = (float)_curWeapon.maxInaccuracy;
            minInaccuracy = (float)_curWeapon.minInaccuracy;
            recoilForce = (float)_curWeapon.recoilForce;
            attackColdDown = (float)_curWeapon.attackCD;
            destabilization = (float)_curWeapon.destabilization;
            aimingDeSpeed = (float)_curWeapon.aimingDeSpeed;
            magazine = _curWeapon.magazine;
            totalBullets = _curWeapon.totalBullets;
            reload = (float)_curWeapon.reload;
            weight = (float)_curWeapon.weight;
            repulsion = _curWeapon.repulsion;

            curTotalBullets = totalBullets;
            curMagazine = magazine;
            curReloadColdDown = curAttackColdDown = 0;

            _player.attack = this;
        }

        private void Update()
        {
            CalculateDestabilization();
            CalculateInaccuracy();
            CalculateColdDown();
            MonitorInput();
        }

        private void CalculateColdDown()
        {
            if (curAttackColdDown > 0)
            {
                curAttackColdDown -= Time.deltaTime;
            }

            if (curReloadColdDown > 0)
            {
                curReloadColdDown -= Time.deltaTime;
            }
            else
            {
                if (curMagazine <= 0)
                {
                    curMagazine = magazine;
                }
            }
        }

        private void CalculateDestabilization()
        {
            if (_player.isMoving)
            {
                if (curDestabilization < destabilization)
                {
                    curDestabilization += destabilization * Time.deltaTime * 1.5f;
                }
                else
                {
                    curDestabilization = destabilization;
                }
            }
            else
            {
                curDestabilization = 0;
            }
        }

        private void CalculateInaccuracy()
        {
            if (curInaccuracy >= minInaccuracy + curDestabilization)
            {
                curInaccuracy -= Time.deltaTime * aimingDeSpeed;
            }
            else
            {
                curInaccuracy = minInaccuracy + curDestabilization;
            }

            leftLine.transform.localRotation = Quaternion.AngleAxis(curInaccuracy, Vector3.forward);
            rightLine.transform.localRotation = Quaternion.AngleAxis(-curInaccuracy, Vector3.forward);
        }
        
        private void MonitorInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _curWeaponScript.Shoot(_audio);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _curWeaponScript.Reload(_audio);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (hasTurret)
                {
                    hasTurret = false;
                    Instantiate(turrets[_turretLv - 1], transform.position, Quaternion.identity);
                }
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (curMine > 0)
                {
                    curMine--;
                    Instantiate(mineGo, transform.position, Quaternion.identity);
                }
            }
        }
    }

    [Serializable]
    public struct WeaponProperties
    {
        public string name;
        public double maxInaccuracy; //最大不精确度
        public double recoilForce; //后坐力(攻击中，影响射击精度)
        public double destabilization; //不稳定性（在所有情况下都会影响精度）
        public double aimingDeSpeed; //瞄准精度减少速度
        public double minInaccuracy; //最小不精确度
        public double attackCD; //攻击CD
        public int magazine; //弹夹中子弹数量
        public int totalBullets; //全部弹药
        public double reload; //装弹时间
        public double weight; //枪重
        public int repulsion; //子弹击退力
    }
}