using UnityEngine;

public class YellowPowder : PowderModificator
{
    // il faut mettre la poudre dans la scene et ensuite rajoute la texture et le sprite renderer.
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private Texture2D texture;
    override public void ApplyModifier()
    {
        if (attachedFirework.name == "Fireworks(Clone)")
        {
            // permet de faire une explosion dans la forme du spriterender/texture
            ParticleSystem.SubEmittersModule subEmitters = attachedFirework.subEmitters;
            ParticleSystem burstSystem = subEmitters.GetSubEmitterSystem(1);
            ParticleSystem.ShapeModule shapeModule = burstSystem.shape;
            shapeModule.shapeType = ParticleSystemShapeType.SpriteRenderer;
            shapeModule.meshShapeType = ParticleSystemMeshShapeType.Triangle;
            shapeModule.spriteRenderer = spriteRenderer;
            shapeModule.texture = texture;
            shapeModule.scale = new Vector3(20,20,20);
        }
    }
}
