using UnityEngine;

namespace Constant
{
    public static class SaveLoadConstant
    {
        public const string PkMoney = "PrefsKeyMoney";
        public const string PkLevels = "PrefsKeyLevels";
        public const string PkVolume = "PrefsKeyVolume";
        public const string PkJoyStickSize = "PKJoyStickSize";
    }

    public static class AssetsConstant
    {
        public static readonly string WeaponJsonPath = Application.streamingAssetsPath + "/WeaponJson.json";
    }

    public static class AssetsPathExt
    {
        public static string AudioGunPath(this string file)
        {
            return $"AudioCLips/GunClip/{file}";
        }

        public static string PrefabBulletPath(this string file)
        {
            return $"Prefabs/Andrew/{file}"; 
        }
        
        public static string PrefabWeaponsPath(this string file)
        {
            return $"Prefabs/Weapons/{file}"; 
        }
    }

    public static class WeaponName
    {
        public const string Pistol = "W001_PISTOL";
        public const string Ak = "W002_AK";
        public const string Mg = "W003_MG";
        public const string Sniper = "W004_SNIPER";
        public const string ShootGun = "W005_SHOOT_GUN";
        public const string CrossBow = "W006_CROSS_BOW";
    }
}