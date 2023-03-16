using System.Collections.Generic;
using GameContent;
using GameContent.Players;
using UnityEngine;

namespace Base
{
    public abstract class BaseItem: MonoBehaviour
    {
        
        protected Andrew Player => GameManager.Instance.andrew;
        protected PlayAttack Attack => Player.attack;

        protected Dictionary<string, object> Extras = new();

        public GameObject triggerGo;

        public void OnTouched(AudioSource goAudio, AudioClip triggerClip)
        {
            if (IsDestroyAfterTrigger())
            {
                Destroy(gameObject);
            }

            if (PlayTriggerClip())
            {
                goAudio.PlayOneShot(triggerClip, GameManager.Instance.recordRunData.volume * 0.5f);    
            }
            
            if (CanTrigger())
            {
                OnTrigger();
            }
        }

        protected virtual bool IsDestroyAfterTrigger() => true; 
        protected virtual bool PlayTriggerClip() => true; 

        protected abstract void OnTrigger();

        protected abstract bool CanTrigger();
    }
}