using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peak.QuixelLogic.Scripts.ScenesLogic;
using Peak.QuixelLogic.Scripts.Common;

namespace Peak.QuixelLogic.Scripts.Game
{
    public class HintProjectileScript : MonoBehaviour
    {
        public GameObject target { get; set; }

        private float distanceToTarget;

        private float step;

        private bool hitTarget = false;

        [SerializeField]
        private ParticleSystem sparkle;

        [SerializeField]
        private ParticleSystem trailParticles;

        private void Start()
        {
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            step = 15 * distanceToTarget;
        }

        private void FixedUpdate()
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            //bool close = distance < 5 ? true : false;

            // Move our position a step closer to the target.
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                if (!hitTarget)
                {
                    hitTarget = true;
                    trailParticles.Stop();
                    sparkle.Play();
                    target.GetComponent<Animator>().SetTrigger("Hint");
                }

                Destroy(gameObject, 1f);
                return;
            }
        }
    }
}