using Base;

namespace GameContent.Items
{
    public class KeyItem: BaseItem
    {
        protected override void OnTrigger()
        {
            GameManager.Instance.levelDoor[0].transform.Find("Locked").gameObject.SetActive(false);
            GameManager.Instance.levelDoor[0].transform.Find("UnLocked").gameObject.SetActive(true);
        }
        
        protected override bool CanTrigger()
        {
            return true;
        }
    }
}