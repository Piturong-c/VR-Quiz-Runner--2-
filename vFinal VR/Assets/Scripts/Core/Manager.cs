using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager self;
    public int currentRoad = 0;
    public bool canGenerate = true;
    public Environment env = Environment.Jungle;
    public int envTile = 30;
    public int maxTile = 30;
    public List<RoadPattern> patterns = new List<RoadPattern>(); // List<T>

    public enum Environment // Enum
    {
        Jungle = 0,
        Desert = 1
    }
    
    private void Awake()
    {
        self = this;
        env = Random.value >= .5 ? Environment.Jungle : Environment.Desert;
    }

    public void NormalizeChance()
    {
        float totalChance = 0;
        for (int i = 0; i < patterns.Count; i++)
        {
            totalChance += patterns[i].chance;
        }

        for (int i = 0; i < patterns.Count; i++)
        {
            patterns[i].chance /= totalChance;
        }
    } //weight proba by variable

    public int GetRandomRoadPattern()
    {
        float value = Random.value;
        for (int i = 0; i < patterns.Count; i++)
        {
            if (i == 0)
            {
                if (value <= patterns[0].chance) return 0;
            }else if (i == patterns.Count - 1)
            {
                if (value >= patterns[patterns.Count - 1].chance) return patterns.Count - 1;
            }
            else
            {
                if (value >= patterns[i].chance && value <= patterns[i + 1].chance)
                {
                    return i;
                }
            }
        }
        return 0;
    }
    
    public int PublishRoad()
    {
        NormalizeChance();
        if (patterns.Count <= 1)
        {
            currentRoad = patterns.Count - 1;
            return patterns.Count - 1;
        }
        int index = GetRandomRoadPattern();
        RoadPattern pattern = patterns[index];
        currentRoad = index;
        return currentRoad;
    }
}

[System.Serializable]
public class RoadPattern
{
    [Range(0,1)]
    public float chance;
}