using Base;
using Util.ext;

namespace GameContent.Items
{
    public class HealthItem: BaseItem
    {
        protected override void OnTrigger()
        {
            Player.ObserveHp.Value = Player.ObserveHp.Value.PlusLimit(0.2f, 1);
        }

        protected override bool CanTrigger()
        {
            return Player.ObserveHp.Value < Player.maxHp;
        }
    }
}