using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ================================================================================================

public class powder_shader : MonoBehaviour
{
    #region Variables
    // Variables
    // =========
    private Vector2      cursorPos;

    const int            SIZE_PARTICLE = 10 * sizeof(float);
    public int           particleCount = 100000;
    int                  kernelID;
    int                  groupSizeX;

    [Header("Paramètres de poudre")]
    public float         xShift = 2f;
    public float         shiftAmount = 1.2f;
    public float         startSize = 0.5f;
    public float         maxDistance = 10f;
    public bool          pouring = false;
    public bool          selected = false;

    public Material      material;
    public ComputeShader shader;
    ComputeBuffer        particleBuffer;
    RenderParams         rp;
    #endregion

    // --------------------------------------------------------------------------------------------
    #region Structure
    // Structure
    // =========
    struct Particle
    {
        public Vector3 position;
        public Vector3 truePosition;
        public Vector3 velocity;
        public float   life;
    }
    #endregion

    // --------------------------------------------------------------------------------------------
    #region Start Update
    // Initialization
    // ==============
    void Start()
    {
        Init();
    }
    #endregion

    // Update
    // ======
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selected)
        {
            shader.SetBool("pouring", true);
        }

        if (Input.GetMouseButtonUp(0) && selected)
        {
            shader.SetBool("pouring", false);
        }

        float[] mousePosition2D = {cursorPos.x, cursorPos.y};

        // on envoie la mouse position au compute shader
        // ---------------------------------------------
        shader.SetFloat("deltaTime", Time.deltaTime);
        shader.SetFloats("mousePosition", mousePosition2D);
        shader.SetFloat("timeUpdate", Time.time);

        // Update des particules
        // ---------------------
        shader.Dispatch(kernelID, groupSizeX, 1, 1);
        Graphics.RenderPrimitives(rp, MeshTopology.Points, 1, particleCount);
    }

    void OnGUI()
    {
        Vector3 p = new Vector3();
        Camera c = Camera.main;
        Event e = Event.current;
        Vector2 mousePos = new Vector2();

        mousePos.x = e.mousePosition.x;
        mousePos.y = c.pixelHeight - e.mousePosition.y;

        p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.nearClipPlane));

        cursorPos.x = p.x;
        cursorPos.y = p.y;
    }

    // --------------------------------------------------------------------------------------------
    #region Functions
    // Functions
    // =========
    void Init()
    {
        // Creation d'un array de particules
        // ---------------------------------
        Particle[] particleArray = new Particle[particleCount];

        for (int i = 0; i < particleCount; i++)
        {
            // Positions random dans un carré de hauteur égale à celle de la chute
            // -------------------------------------------------------------------
            float y = UnityEngine.Random.Range(-10, 10);

            particleArray[i].position.x = -100;
            particleArray[i].position.y = y;
            particleArray[i].position.z = 0;
            particleArray[i].truePosition = particleArray[i].position;

            // Vitesse initiale nulle
            // ----------------------
            particleArray[i].velocity.x = 0;
            particleArray[i].velocity.y = 0;
            particleArray[i].velocity.z = 0;

            // vie Random entre 1 et 6 ??
            // --------------------------
            particleArray[i].life = Random.value * 5.0f + 1.0f;
        }

        // Création du buffer (structure de transfert vers le GPU)
        // -------------------------------------------------------
        particleBuffer = new ComputeBuffer(particleCount, SIZE_PARTICLE);
        

        // Remplissage du buffer avec les données
        // --------------------------------------
        particleBuffer.SetData(particleArray);


        // on récupère l'identifiant du compute shader custom
        // ---------------------------------------------------
        kernelID = shader.FindKernel("CSParticle");
        uint threadsX;
        shader.GetKernelThreadGroupSizes(kernelID, out threadsX, out _, out _);
        groupSizeX = Mathf.CeilToInt((float)particleCount / (float)threadsX);


        // on envoie le buffer sur le shader et le compute shader
        // ------------------------------------------------------
        shader.SetBuffer(kernelID, "particleBuffer", particleBuffer);
        material.SetBuffer("particleBuffer", particleBuffer);
        shader.SetFloat("xShift", xShift);
        shader.SetFloat("shiftAmount", shiftAmount);
        shader.SetFloat("startSize", startSize);
        shader.SetFloat("maxDistance", maxDistance);
        shader.SetFloat("timeUpdate", Time.time);
        shader.SetBool("pouring", false);

        // On setup le matériau des particules et les world bounds (au delà desquelles on kill les particules)
        // ---------------------------------------------------------------------------------------------------
        rp = new RenderParams(material);
        rp.worldBounds = new Bounds(Vector3.zero, 0.1f*Vector3.one);
    }

    // Pour vider le buffer en cas d'arrêt
    // -----------------------------------
    void OnDestroy()
    {
        if (particleBuffer != null)
            particleBuffer.Release();
    }
    #endregion
}
