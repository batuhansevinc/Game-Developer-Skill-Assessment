using UnityEngine;

namespace HappyHour.Concretes.Controllers
{
    public class PlayerFootPrint : MonoSingleton<PlayerFootPrint>
    {

        public enum enumFoot
        {
            Left,
            Right,
        }
        public GameObject LeftPrefab = null;
        public GameObject RightPrefab = null;
        public float FootprintSpacer = 1.0f;
        private Vector3 LastFootprint;
        private enumFoot WhichFoot;
        bool _isIdle;


        private void Start()
        {
            SpawnDecal(LeftPrefab);
            LastFootprint = this.transform.position;
            _isIdle = true;
        }

        private void Update()
        {
            if (!_isIdle)
            {
                float DistanceSinceLastFootprint = Vector2.Distance(LastFootprint, this.transform.position);
                if (DistanceSinceLastFootprint >= FootprintSpacer)
                {
                    if (WhichFoot == enumFoot.Left)
                    {
                        SpawnDecal(LeftPrefab);
                        WhichFoot = enumFoot.Right;
                    }
                    else if (WhichFoot == enumFoot.Right)
                    {
                        SpawnDecal(RightPrefab);
                        WhichFoot = enumFoot.Left;
                    }
                    LastFootprint = this.transform.position;
                }

            }
        }
        
        private void SpawnDecal(GameObject prefab)
        {
            //we want to cast a ray(line) from the player to the ground
            Vector2 from = this.transform.position;
            Vector2 to = new Vector2(this.transform.position.x, this.transform.position.y - (this.transform.localScale.y / 2.0f) + 0.1f);
            Vector2 direction = to - from;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 10f);
            if (Physics2D.Raycast(from, direction) == true)
            {
                //where the ray hits the ground we will place a footprint
                GameObject decal = Instantiate(prefab);
                decal.transform.position = hit.point;
                //turn the footprint to match the direction the player is facing
                decal.transform.Rotate(Vector2.up, this.transform.eulerAngles.y);
            }
        }
    }
}
