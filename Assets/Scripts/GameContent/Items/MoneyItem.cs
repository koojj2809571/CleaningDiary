using Base;

namespace GameContent.Items
{
    public class MoneyItem: BaseItem
    {
        protected override void OnTrigger()
        {
            GameManager.Instance.recordRunData.money += 5;
        }
        
        protected override bool CanTrigger()
        {
            return true;
        }
    }
}