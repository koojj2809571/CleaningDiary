using Base;
using Util.ext;

namespace GameContent.Items
{
    public class BulletItem:BaseItem
    {
        protected override void OnTrigger()
        {
            Attack.curTotalBullets.PlusLimit((int)(Attack.totalBullets * 0.25f), Attack.totalBullets);
        }

        protected override bool CanTrigger()
        {
            return Attack.curTotalBullets < Attack.totalBullets;
        }
    }
}