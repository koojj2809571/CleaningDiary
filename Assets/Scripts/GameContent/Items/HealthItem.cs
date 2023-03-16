using Base;
using Util.ext;

namespace GameContent.Items
{
    public class HealthItem: BaseItem
    {
        protected override void OnTrigger()
        {
            Player.curHp.PlusLimit(0.2f, 1);
        }

        protected override bool CanTrigger()
        {
            return Player.curHp < Player.maxHp;
        }
    }
}