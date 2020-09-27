using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{
    public class MouseDestruction : MonoBehaviour
    {
        [Tooltip("Radius of circle that will destroy world on click.")]
        [SerializeField]
        private int radius = 8;
        [Tooltip("Radius of a ring that will create outline on click.")]
        [SerializeField]
        private int outlineRadius = 12;
        [Tooltip("Outline is a color of an original texture multiplied by this value")]
        [SerializeField]
        [Range(0f,1f)]
        private float outlineDarkeningMultiplier = 0.25f;

        private World world;

        private IDestructor destructor;

        private Vector3 prevMousePos;
        // Start is called before the first frame update
        void Start()
        {
            destructor = new WormsDestructor(radius, outlineRadius, outlineDarkeningMultiplier);
            world = FindObjectOfType<World>();
        }

        // Update is called once per frame
        public void Destruction()
        {
            if (Input.GetMouseButton(0)) {
                // Offset should be (0,0) for calcualting our World position thats why we substract position.
                Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - world.transform.position;
                Vector3 pPos = Camera.main.ScreenToWorldPoint(prevMousePos) - world.transform.position;
                Vector3 diff = mPos - pPos;
                Vector3 normDiff = diff.normalized;

                // Draw whole diff between this and last frame
                float absRadius = radius * .5f / world.ChunkSizeX;
                float distMult = diff.magnitude;
                for (float i = 0; i < Mathf.Max(distMult, absRadius); i += absRadius) {
                    Vector3 targetPos = pPos + i * normDiff;
                    Vector2Int wPos = world.ScenePositionToWorldPosition(targetPos);
                    destructor.Destroy(wPos.x - radius, wPos.y - radius, world);
                }
                
            }
            prevMousePos = Input.mousePosition;

        }
        void Update()
        {
            Destruction();
        }

    }
}


