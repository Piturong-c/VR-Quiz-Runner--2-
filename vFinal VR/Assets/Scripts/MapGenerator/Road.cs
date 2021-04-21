using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    
    public int id = 0;
    public List<ObstacleSpawner> obstacles = new List<ObstacleSpawner>();
    public int obstacleAppear = 0;
    [Range(0,1)]
    public float obstacleChance = 0.05f;
    
    public static float runningChance = 0f;
    public float runningChanceRateMin = 0.0001f;
    public float runningChanceRateMax = 0.001f;
    public float runningMaxChance = 0.3f;
    void Start()
    {
        obstacleChance = PlayerPrefs.GetFloat("UI_OBS");
        if (Manager.self.canGenerate)
        {
            transform.GetChild(0).GetChild(Manager.self.currentRoad).gameObject.SetActive(true);
        }
        
        obstacleAppear = Random.Range(0, obstacles.Count);
        
        for (int i = 0; i < obstacles.Count; i++)
        {
            obstacles[i].gameObject.SetActive(false);
        }

        obstacleChance += runningChance;
        if(Mathf.Max(runningMaxChance, obstacleChance) >= Random.Range(0.0f,1.0f)){
            if (obstacleAppear == 0 || obstacleAppear == 2)
            {
                if (!ObstacleTree.canGenerateTree)
                {
                    obstacleAppear = Random.value > 0.5f ? 1 : 3;
                }

                ObstacleTree.canGenerateTree = !ObstacleTree.canGenerateTree;
            }

            obstacles[obstacleAppear].gameObject.SetActive(true);
        }
        runningChance += Random.Range(runningChanceRateMin, runningChanceRateMax);
    }

    public void Obstate()
    {
        foreach (ObstacleSpawner obstacle in obstacles)
        {
            if (!transform.GetChild(0).GetChild(0).GetChild(0).gameObject.activeSelf)
                obstacle.Spawn();
        }
    }
}
