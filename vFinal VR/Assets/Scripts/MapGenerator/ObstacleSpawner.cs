using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static bool canGenerateTree = false;
    public virtual void Spawn()
    {
        
    }
    
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y =  Random.Range(-1f, 1f) * magnitude;
            
            Camera.main.transform.localPosition += new Vector3(x,y);
            elapsed += Time.deltaTime;
            magnitude *= 0.8f;
            yield return null;
            Camera.main.transform.localPosition = originalPos;
        }
        
    }
}