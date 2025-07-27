using System.Collections.Generic;
using UnityEngine;

public class YellowPowder : PowderModificator
{
    // il faut mettre la poudre dans la scene et ensuite rajoute la texture et le sprite renderer.
    [SerializeField]
    List<SpriteRenderer> spritesRenderers;
    
    [SerializeField]
    List<Texture2D> textures;
    override public void ApplyModifier()
    {
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            // permet de faire une explosion dans la forme du spriterender/texture
            ParticleSystem.SubEmittersModule subEmitters = attachedFirework.subEmitters;
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.EmissionModule emission = burstSystem.emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);
            float numEmitted = bursts[0].count.constant;
            numEmitted = 750;
            bursts[0].count = new ParticleSystem.MinMaxCurve(numEmitted);
            emission.SetBursts(bursts);
            ParticleSystem.ShapeModule shapeModule = burstSystem.shape;
            shapeModule.shapeType = ParticleSystemShapeType.SpriteRenderer;
            shapeModule.meshShapeType = ParticleSystemMeshShapeType.Triangle;
            int randomIndex = Random.Range(0, textures.Count); 
            shapeModule.spriteRenderer = spritesRenderers[randomIndex];
            shapeModule.texture = textures[randomIndex];
            shapeModule.scale = new Vector3(10,10,10);
        }
    }
}
