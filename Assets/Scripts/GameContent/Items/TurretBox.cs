using Base;

namespace GameContent.Items
{
    public class TurretBox:BaseItem
    {
        
        protected override bool IsDestroyAfterTrigger() => false;
        
        protected override void OnTrigger()
        {
            Attack.hasTurret = true;
            Attack.curMine = Attack.carryMineSize;
        }
        
        protected override bool CanTrigger()
        {
            return !Attack.hasTurret || Attack.curMine < Attack.carryMineSize;
        }
    }
}