using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShootingSystem;

public class powder_charge : MonoBehaviour
{
    // Variables
    // =========
    public Transform meche;
    public List<PowderModificator> powderPrefabs;
    public static powder_charge Instance;
    public int maxCharge;
    public int chargeNeed;
    public int currentCharge = 0;
    public List<GameObject> flagGO;
    public int chargeCount = 0;
    private float width;
    public float xOffset = 10f;
    public float yOffset = -10f;
    public int currentIndex = 0;
    public bool overShelf = false;

    void Awake(){
        Instance = this;
    }
    
    void Start()
    {
        width = meche.GetComponent<SpriteRenderer>().bounds.size.x - xOffset;
    }

    void Update(){
        if (currentCharge >= chargeNeed)
        {
            AddCharge(currentIndex);
            currentCharge = 0;
        }
    }

    public void AddCharge(int Index){
        BaseShootingSystem.Instance.powderList.Add(powderPrefabs[Index]);
        GameObject instance = Instantiate(flagGO[Index], meche);
        instance.transform.position = new Vector3(meche.position.x + width/2 - (width/maxCharge)*chargeCount, meche.position.y + yOffset, meche.position.z);
        chargeCount ++;
        
    }
}
