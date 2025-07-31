using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ShootingSystem {
    public class FireworkProjectile : MonoBehaviour
    {
        public ParticleSystem attachedFirework;
        private ObjectPool pool; /// The object pool managing the projectile instances.
        private Vector2 startPosition; /// The starting position of the projectile.
        [SerializeField]
        private List<PowderModificator> powderList;
        public List<float> colorList;
        public Vector3 finalColor = new Vector3 (0f, 0f, 0f);
        public Material fireworkMaterial;
        public float emissive = 5f;



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

            if(colorList.Count > 0)
            {
                ChangeFinalColor();
            }

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

        public void ChangeFinalColor()
        {
            float colorFloat = colorList[Random.Range(0, colorList.Count)]; // pick une couleur au pif 
            float tol = 0.1f;
            if(colorFloat == 0f)
                tol = 0.04f;
            Color randomColor = RandomSaturatedColor(colorFloat - tol, colorFloat + tol);

            // Apply color to main firework system
            var subEmitters = attachedFirework.subEmitters;

            // Trail system
            ParticleSystem trailPS = subEmitters.GetSubEmitterSystem(0);
            ApplyColorToSystem(trailPS, randomColor);

            // Only for the main explosion firework
            if (attachedFirework.name == "Fireworks(Clone)")
            {
                ParticleSystem burstPS = subEmitters.GetSubEmitterSystem(1);
                burstPS.GetComponent<ParticleSystemRenderer>().material = trailPS.GetComponent<ParticleSystemRenderer>().material;

                ParticleSystem burstTrailPS = burstPS.subEmitters.GetSubEmitterSystem(0);
                burstTrailPS.GetComponent<ParticleSystemRenderer>().material = trailPS.GetComponent<ParticleSystemRenderer>().material;
            }
        }

        private void ApplyColorToSystem(ParticleSystem ps, Color color)
        {
            if (ps == null) return;

            // Change start color
            var main = ps.main;
            main.startColor = color;

            // Assign instanced material
            ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
            if (renderer != null && fireworkMaterial != null)
            {
                Material matInstance = new Material(fireworkMaterial);
                matInstance.color = color;
                matInstance.SetColor("_EmissionColor", color * emissive); // Make sure emission is enabled on the shader
                renderer.material = matInstance;
            }
        }


        // Pick a random saturated color;
        Color RandomSaturatedColor(float min, float max)
        {
            float hue = Mathf.Repeat(Random.Range(min, max), 1f);       // 0 rouge, 0.3 V, 0.6 B
            float saturation = 1f;                   // Fully saturated
            float value = Random.Range(0.8f, 1f);    // à check

            return Color.HSVToRGB(hue, saturation, value);
        }
    }
}
