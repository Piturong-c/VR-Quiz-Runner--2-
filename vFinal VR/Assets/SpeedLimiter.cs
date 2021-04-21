using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimiter : MonoBehaviour
{
    public float limit = 15;
    // Update is called once per frame
    void Update()
    {
        Player.self.speed = limit;
        Player.self.speed = Mathf.Clamp(Player.self.speed,limit,limit);
    }
}
