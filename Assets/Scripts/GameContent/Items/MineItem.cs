using Base;
using Util.ext;

namespace GameContent.Items
{
    public class MineItem: BaseItem
    {
        protected override void OnTrigger()
        {
            Attack.curMine.PlusLimit(1,Attack.carryMineSize);
        }
        
        protected override bool CanTrigger()
        {
            return Attack.curMine <  3;
        }
    }
}