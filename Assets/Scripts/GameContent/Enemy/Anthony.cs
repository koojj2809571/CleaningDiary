using Base;

namespace GameContent.Enemy
{
    public class Anthony : BaseEnemy
    {
        protected override void Update()
        {
            
        }

        protected override void Die()
        {
            base.Die();
            GameManager.Instance.anthonyIsDead = true;
        }
    }
}