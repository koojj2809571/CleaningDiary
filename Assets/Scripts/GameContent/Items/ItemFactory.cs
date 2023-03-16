using Base;
using UnityEngine;

namespace GameContent.Items
{
    public static class ItemFactory
    {
        public static BaseItem CreateItem(GameObject go)
        {
            Debug.Log("Factory");
            if (go.name.Contains("BulletItem")) return go.AddComponent<BulletItem>();
            if (go.name.Contains("HealthItem"))  return go.AddComponent<HealthItem>();
            if (go.name.Contains("MineItem"))  return go.AddComponent<MineItem>();
            if (go.name.Contains("MoneyItem"))  return go.AddComponent<MoneyItem>();
            if (go.name.Contains("TurretItem"))  return go.AddComponent<TurretItem>();
            if (go.name.Contains("BulletsBox"))  return go.AddComponent<BulletBox>();
            if (go.name.Contains("HealthBox"))  return go.AddComponent<HealthBox>();
            if (go.name.Contains("TurretBox"))  return go.AddComponent<TurretBox>();
            if (go.name.Contains("KeyItem"))  return go.AddComponent<KeyItem>();
            if (go.name.Contains("FlagItem"))  return go.AddComponent<FlagItem>();
            
            return null;
        }
    }
}