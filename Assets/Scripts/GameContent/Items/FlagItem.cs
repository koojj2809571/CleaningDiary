using Base;
using Util;

namespace GameContent.Items
{
    public class FlagItem : BaseItem
    {

        private SaveLoadUtils RunData => GameManager.Instance.recordRunData;

        protected override void OnTrigger()
        {
            RunData.SaveMoney(RunData.money);
            RunData.SaveLevel(RunData.unLockedLevel + 1);
            // SceneManager.LoadScene(0);
        }

        protected override bool CanTrigger()
        {
            return triggerGo.CompareTag("Player") && Player.curKill >= Player.killTarget;
        }
    }
}