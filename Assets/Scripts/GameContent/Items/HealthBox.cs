using Base;

namespace GameContent.Items
{
    public class HealthBox: BaseItem
    {
        
        protected override bool IsDestroyAfterTrigger() => false;
        
        protected override void OnTrigger()
        {
            Player.ObserveHp.Value = 1f;
        }

        protected override bool CanTrigger()
        {
            return Player.ObserveHp.Value < Player.maxHp;
        }
    }
}