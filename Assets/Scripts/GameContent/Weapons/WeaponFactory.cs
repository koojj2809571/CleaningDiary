using Base;
using Constant;
using UnityEngine;

namespace GameContent.Weapons
{
    public static class WeaponFactory
    {
        public static BaseWeapon CreateItem(GameObject go)
        {

            if (go.name.Contains(WeaponName.Pistol)) return go.transform.Find("Pistol").GetComponent<Pistol>();
            if (go.name.Contains(WeaponName.Ak)) return go.transform.Find("AK").GetComponent<AK>();
            if (go.name.Contains(WeaponName.Mg)) return go.transform.Find("MG").GetComponent<MG>();
            if (go.name.Contains(WeaponName.Sniper)) return go.transform.Find("Sniper").GetComponent<Sniper>();
            if (go.name.Contains(WeaponName.ShootGun)) return go.transform.Find("ShootGun").GetComponent<ShootGun>();
            if (go.name.Contains(WeaponName.CrossBow)) return go.transform.Find("CrossBow").GetComponent<CrossBow>();
            
            return null;
        }
    }
}