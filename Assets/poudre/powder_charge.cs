using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class powder_charge : MonoBehaviour
{
    // Variables
    // =========
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
        width = GetComponent<SpriteRenderer>().bounds.size.x - xOffset;
    }

    void Update(){
        if (currentCharge >= chargeNeed)
        {
            AddFlag(currentIndex);
            currentCharge = 0;
        }
    }

    public void AddFlag(int Index){
        GameObject instance = Instantiate(flagGO[Index], transform);
        instance.transform.position = new Vector3(transform.position.x + width/2 - (width/maxCharge)*chargeCount, transform.position.y + yOffset, transform.position.z);
        chargeCount ++;
    }
}
