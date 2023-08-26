using UnityEngine;

namespace HappyHour.Concretes.Controllers
{
    public class Footprint : MonoSingleton<Footprint>
    {
        [Tooltip("this is how long the decal will stay, before it shrinks away totally")]
        public float Lifetime = 2.0f;

        private float mark;
        private Vector2 OrigSize;

        public void Start()
        {
            mark = Time.time;
            OrigSize = this.transform.localScale;
        }
        public void Update()
        {
            float ElapsedTime = Time.time - mark;
            if (ElapsedTime != 0)
            {
                float PercentTimeLeft = (Lifetime - ElapsedTime) / Lifetime;

                this.transform.localScale = new Vector2(OrigSize.x * PercentTimeLeft, OrigSize.y * PercentTimeLeft);
                if (ElapsedTime > Lifetime)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}