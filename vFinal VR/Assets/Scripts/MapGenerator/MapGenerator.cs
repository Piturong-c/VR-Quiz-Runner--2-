using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    /* Road Prefab */
    public GameObject road;
    /* Singleton declaration */
    public static MapGenerator self;
    public Vector2 sizes = new Vector2(.1f,.1f);
    public Vector2 randomOffsets;
    private int _visible = 0;
    private int slope = 0;
    
    public void Awake()
    {
        /* Set value of singleton to current instance */
        if(self == null)
            self = this;
        /* Random offsets of Perlin-noise used in MeshGenerator */
        randomOffsets = new Vector2(Random.Range(0,100000),Random.Range(0,100000));
    }

    public void Start()
    {
        InitializeRoads();
    }

    private void InitializeRoads()
    {
        /* Generate 10 roads to be visible */
        for (int i = 0; i <= 10; i++)
            GenerateRoad();
    }

    public void GenerateRoad()
    {
        /* increase _visible by 1 and Instantiate road and set its id to _variable in order to check how far the obstacle should be animated. */
        _visible++;
        GameObject r = (GameObject)Instantiate(road, transform.position + Vector3.right * 10 * _visible, Quaternion.identity);
        r.GetComponent<Road>().id = _visible;
    }

    public void GenerateQuiz(Answer answer)
    {
        /* increase _visible by 1 and Instantiate road and set its id to _variable in order to check how far the obstacle should be animated. */
        _visible++;
        GameObject r = (GameObject)Instantiate(road, transform.position + Vector3.right * 10 * _visible, Quaternion.identity);
        r.GetComponent<Road>().id = _visible;
        /**/
        r.transform.GetChild(0).GetChild(1).gameObject.SetActive(false); // force hide hole
        r.transform.GetChild(0).GetChild(0).gameObject.SetActive(true); // force show normal road
        /**/
        r.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < 3; i++) // change code here
        {
            r.transform.GetChild(0).GetChild(0).GetChild(0).Find("Answer"+(i+1).ToString() + "/Cube/Canvas/Text (TMP)").GetComponent<TMP_Text>().text =
                answer.answers[i].ToString();
        }

        r.name = "Quiz!";
    }
}
