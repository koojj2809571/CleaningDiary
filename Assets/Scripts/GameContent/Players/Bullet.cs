using UnityEngine;
using UnityEngine.Serialization;

namespace GameContent.Players
{
    public class Bullet : MonoBehaviour
    {

        public float lifeTime;
        public float speed;
        public GameObject destroyParticle;
        public bool isFromPlayer;

        public void SetFromPlayer(bool from)
        {
            isFromPlayer = from;
        }

        private void Start()
        {
            Destroy(gameObject,lifeTime);
        }

        private void Update()
        {
            transform.Translate(transform.up * (Time.deltaTime * speed),Space.World);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Wall"))
            {
                Destroy(gameObject);
                GameObject go = Instantiate(destroyParticle, transform.position, Quaternion.identity);
                Destroy(go,1f);
            }
        }
    }
}