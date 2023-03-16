using Base;

namespace GameContent.Items
{
    public class BulletBox:BaseItem
    {
        protected override bool IsDestroyAfterTrigger() => false;

        protected override void OnTrigger()
        {
            Attack.curTotalBullets = Attack.totalBullets;
        }

        protected override bool CanTrigger()
        {
            return Attack.curTotalBullets < Attack.totalBullets;
        }
    }
}