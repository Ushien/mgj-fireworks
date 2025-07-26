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
    public static powder_shader Instance;

    const int            SIZE_PARTICLE = 12 * sizeof(float);
    public int           particleCount = 100000;
    public int           selectedIndex = 0;
    int                  kernelID;
    int                  groupSizeX;
    public bool          overShelf = false;
    public int           filling = 0;

    [Header("Paramètres de poudre")]
    public float         xShift = 2f;
    public float         shiftAmount = 1.2f;
    public float         startSize = 0.5f;
    public float         maxDistance = 10f;
    public bool          pouring = false;
    public bool          selected = false;
    public float         yBoxMax = -4f;
    public float         xBoxMin = -3f;
    public float         xBoxMax = 3f;

    public Material      material;
    public ComputeShader shader;
    ComputeBuffer        particleBuffer;
    ComputeBuffer        addedCounter;
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
        public Vector2Int type;
    }
    #endregion

    // --------------------------------------------------------------------------------------------
    #region Start Update
    // Initialization
    // ==============
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Init();
    }
    #endregion

    // Update
    // ======
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selected &&  !overShelf)
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
        shader.SetInt("selectedIndex", selectedIndex);

        // Update des particules
        // ---------------------
        shader.Dispatch(kernelID, groupSizeX, 1, 1);
        Graphics.RenderPrimitives(rp, MeshTopology.Points, 1, particleCount);

        // On récupère le nombre de particules dans la fusée cette frame
        // -------------------------------------------------------------
        uint[] addCount = new uint[1];
        addedCounter.GetData(addCount);
        int addCountInt = (int)addCount[0];
        filling += addCountInt;
        uint[] zero = new uint[1] { 0 };
        addedCounter.SetData(zero);
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
            particleArray[i].position = new Vector3(-100, y, 0);
            particleArray[i].truePosition = particleArray[i].position;

            // Vitesse initiale nulle
            // ----------------------
            particleArray[i].velocity = Vector3.zero;

            // vie Random entre 1 et 6 ??
            // --------------------------
            particleArray[i].life = Random.value * 5.0f + 1.0f;

            // couleur initiale
            // ----------------
            particleArray[i].type = new Vector2Int (1,1); // A gauche le current et displayed, à droite le sélected (qui change le current au respawn)
        }

        // Création du buffer (structure de transfert vers le GPU)
        // -------------------------------------------------------
        particleBuffer = new ComputeBuffer(particleCount, SIZE_PARTICLE);

        // Buffer de comptage de particules dans la fusée
        // ----------------------------------------------
        addedCounter = new ComputeBuffer(1, sizeof(uint), ComputeBufferType.Raw);
        uint[] zero = new uint[1] { 0 };
        addedCounter.SetData(zero);
        

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
        shader.SetBuffer(kernelID, "addedCounter", addedCounter);
        material.SetBuffer("particleBuffer", particleBuffer);
        shader.SetFloat("xShift", xShift);
        shader.SetFloat("shiftAmount", shiftAmount);
        shader.SetFloat("startSize", startSize);
        shader.SetFloat("maxDistance", maxDistance);
        shader.SetFloat("timeUpdate", Time.time);
        shader.SetBool("pouring", false);
        shader.SetInt("selectedIndex", 1);
        shader.SetFloat("xBoxMax", xBoxMax);
        shader.SetFloat("xBoxMin", xBoxMin);
        shader.SetFloat("yBoxMax", yBoxMax);

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
        if (addedCounter != null)
        {
            addedCounter.Release();
            addedCounter = null;
        }
    }
    #endregion
}
