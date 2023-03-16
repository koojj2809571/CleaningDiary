using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using GameContent.Players;
using static Constant.SaveLoadConstant;
using UnityEngine;

namespace Util
{
    [Serializable]
    public class SaveLoadUtils: System.Object
    {
        //玩家数据
        public int money;
        public int unLockedLevel;
        public int selectLevel;
        public int gunLevel;

        
        public float volume;
        public float joystickSize;

        //游戏数据
        public List<WeaponProperties> weaponPropertiesList;
        public Dictionary<string,WeaponProperties> WeaponPropertiesDic;

        public void InitRecord()
        {
            money = (int)LoadData(PkMoney, 0);
            unLockedLevel = (int)LoadData(PkLevels, 1);
            volume = (float)LoadData(PkVolume, 1.0f);
            joystickSize = (float)LoadData(PkJoyStickSize, 0.3f);
            gunLevel = 6;
        }

        public void InitWeapons()
        {
            weaponPropertiesList = new List<WeaponProperties>();
            weaponPropertiesList =
                SerializeFileUtil.ParseJsonFileTo<List<WeaponProperties>>(AssetsConstant.WeaponJsonPath);
            WeaponPropertiesDic = weaponPropertiesList.ToDictionary(x => x.name);
        }

        public WeaponProperties CurWeapon()
        {
            return weaponPropertiesList[gunLevel - 1];
        }

        public void SaveLevel(int newLevel)
        {
            if (newLevel > unLockedLevel)
            {
                SaveData(PkLevels, unLockedLevel + 1);
            }
        }

        public void SaveMoney(int newMoney)
        {
            SaveData(PkMoney, newMoney);
        }
        
        public void SaveVolume(float newVolume)
        {
            SaveData(PkVolume, newVolume);
        }
        
        public void SaveJoystickSize(float newJoystickSize)
        {
            SaveData(PkJoyStickSize, newJoystickSize);
        }

        public void SaveData<T>(string key, T data)
        {
            if (data is int i)
            {
                PlayerPrefs.SetInt(key,i);
            }
            if (data is float f)
            {
                PlayerPrefs.SetFloat(key,f);
            }
            if (data is string s)
            {
                PlayerPrefs.SetString(key,s);
            }
        }

        public object LoadData<T>(string key , T defaultValue)
        {
            if (typeof(T) == typeof(int))
            {
                return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : defaultValue;
            }
            if (typeof(T) == typeof(float))
            {
                return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
            }
            if (typeof(T) == typeof(string))
            {
                return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : defaultValue;
            }

            return null;
        }
    }
}