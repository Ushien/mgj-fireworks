using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class powder_charge : MonoBehaviour
{
    // Variables
    // =========
    public int maxCharge;
    public int chargeNeed;
    public List<GameObject> flagGO;
    public int chargeCount = 0;
    private float width;
    public float xOffset = 10f;
    public float yOffset = -10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x - xOffset;
    }

    void Update(){
        if(powder_shader.Instance.filling >= chargeNeed && chargeCount <= maxCharge){
            powder_shader.Instance.filling = 0;
            AddFlag(powder_shader.Instance.selectedIndex);
        }
    }

    public void AddFlag(int Index){
        GameObject instance = Instantiate(flagGO[Index], transform);
        instance.transform.position = new Vector3(transform.position.x + width/2 - (width/maxCharge)*chargeCount, transform.position.y + yOffset, transform.position.z);
        chargeCount ++;
    }
}
