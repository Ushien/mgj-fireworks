using System.Collections.Generic;
using UnityEngine;

public class YellowPowder : PowderModificator
{
    [System.Serializable]
    public class YellowAsset
    {
        public SpriteRenderer spriteRenderer;
        public Texture2D texture;
        public Vector3 size;
        public int particles;
    }

    // il faut mettre la poudre dans la scene et ensuite rajoute la texture et le sprite renderer.
    [SerializeField]
    List<YellowAsset> yellowAsset;

    override public void ApplyModifier()
    {
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            // permet de faire une explosion dans la forme du spriterender/texture
            ParticleSystem.SubEmittersModule subEmitters = attachedFirework.subEmitters;
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.EmissionModule emission = burstSystem.emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];

            // Modification du module shape
            ParticleSystem.ShapeModule shapeModule = burstSystem.shape;
            shapeModule.shapeType = ParticleSystemShapeType.SpriteRenderer;
            shapeModule.meshShapeType = ParticleSystemMeshShapeType.Triangle;
            shapeModule.rotation = new Vector3(-90f, 180f, 0f);

            // Assignation de la texture
            int randomIndex = Random.Range(0, yellowAsset.Count); 
            shapeModule.spriteRenderer = yellowAsset[randomIndex].spriteRenderer;
            shapeModule.texture = yellowAsset[randomIndex].texture;
            shapeModule.scale = yellowAsset[randomIndex].size;

            // Assingation du nombre de particules
            emission.GetBursts(bursts);
            float numEmitted = bursts[0].count.constant;
            numEmitted = yellowAsset[randomIndex].particles;
            bursts[0].count = new ParticleSystem.MinMaxCurve(numEmitted);
            emission.SetBursts(bursts);

        }
    }
}
