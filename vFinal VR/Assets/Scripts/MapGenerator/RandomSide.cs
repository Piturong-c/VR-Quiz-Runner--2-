using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int side = Random.Range(-2, 1); // -2 -1 0
        transform.localEulerAngles += new Vector3(0,0,-90.0f);
        
        if (name == "brokenFloor0" && isActiveAndEnabled)
        {
            if (Random.value <= .3f)
            {
                transform.GetChild(0).gameObject.SetActive(true); // set cover's active to true
            }
            
            Transform road = transform.parent.parent;
            for (int i = 0; i < 4; i++)
            {
                road.Find("ObstacleSpawner" + (i + 1).ToString()).gameObject.SetActive(false);
            }
        }
    }
}
