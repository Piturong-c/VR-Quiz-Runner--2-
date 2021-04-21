using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public Gradient color;
    void Start()
    {
        GetComponent<Renderer>().material.color = color.Evaluate(Random.value);
    }
}
