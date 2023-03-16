using Base;

namespace GameContent.Items
{
    public class TurretItem: BaseItem
    {
        protected override void OnTrigger()
        {
            Attack.hasTurret = true;
        }
        
        protected override bool CanTrigger()
        {
            return !Attack.hasTurret;
        }
    }
}