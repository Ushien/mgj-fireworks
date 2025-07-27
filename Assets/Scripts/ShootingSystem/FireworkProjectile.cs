using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ShootingSystem {
    public class FireworkProjectile : MonoBehaviour
    {
        public ParticleSystem attachedFirework;
        private ObjectPool pool; /// The object pool managing the projectile instances.
        private Vector2 startPosition; /// The starting position of the projectile.
        [SerializeField]
        private List<PowderModificator> powderList;
        public Vector3 finalColor = new Vector3 (0f, 0f, 0f);


        /// <summary>
        /// Initializes the projectile with the specified parameters.
        /// </summary>
        /// <param name="pool">The object pool managing the projectile instances.</param>
        /// <param name="startPosition">The starting position of the projectile.</param>
        public void Init(ObjectPool pool, Vector2 startPosition, List<PowderModificator> powderList)
        {
            this.startPosition = startPosition;
            this.pool = pool;
            this.powderList = powderList;

            //Place les purplePowders dans une liste à part, afin de gérer l'héritage des caractéristiques
            List<PowderModificator> purplePowders = new List<PowderModificator>();
            List<PowderModificator> otherPowders = new List<PowderModificator>();

            foreach (PowderModificator powder in powderList)
            {
                if (powder is PurplePowder)
                {
                    purplePowders.Add(powder);
                }
                else
                {
                    otherPowders.Add(powder);
                }
            }

            foreach (PowderModificator powder in otherPowders)
            {
                powder.FireworkScript = this;
                powder.attachedFirework = GetComponent<ParticleSystem>();
                powder.ApplyModifier();
            }

            ChangeFinalColor();

            foreach (PowderModificator purplePowder in purplePowders)
            {
                purplePowder.FireworkScript = this;
                purplePowder.attachedFirework = GetComponent<ParticleSystem>();
                purplePowder.ApplyModifier(otherPowders);
            }
        }

        public void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive(withChildren: true))
            {
                pool.ReturnObject(gameObject);
            }
        }

        public void ChangeFinalColor(){
            Debug.Log(finalColor);
            // // Change la couleur du projectile principal
            ParticleSystem.MainModule mm = attachedFirework.GetComponent<ParticleSystem>().main;
            finalColor = finalColor.normalized;
            mm.startColor = new ParticleSystem.MinMaxGradient(new Color(finalColor.x, finalColor.y, finalColor.z));

            // Change la couleur du trail
            var subEmitters = attachedFirework.subEmitters;
            ParticleSystem.MainModule mm1 = subEmitters.GetSubEmitterSystem(0).main;
            mm1.startColor = mm.startColor;

            // Si on manipule l'explosion principale
            if (attachedFirework.name == "Fireworks(Clone)")
            {
                // Change la couleur des projectiles de l'explosion
                ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
                ParticleSystem.MainModule mm2 = burstSystem.main;
                mm2.startColor = mm.startColor;

                // Change la couleur du trail de l'explosion
                ParticleSystem trailSystem = burstSystem.subEmitters.GetSubEmitterSystem(0);
                ParticleSystem.MainModule mm3 = trailSystem.main;
                mm3.startColor = mm.startColor;
            }
        }
    }
}
