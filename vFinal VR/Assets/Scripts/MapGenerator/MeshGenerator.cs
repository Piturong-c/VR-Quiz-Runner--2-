using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshGenerator))]
public class MeshGenerator : MonoBehaviour
{

    public GameObject[] trees;
    public GameObject[] flowers;
    public GameObject[] rocks;
    public GameObject[] cacti;

    private Mesh mesh;
    private Color[] uv;
    private Vector3[] vertices;
    private MeshCollider _collider;
    private int[] triangles;
    public int sizeX = 10;
    public int sizeZ = 10;
    public bool isRight;
    public int x;
    private float minHeight;
    private float maxHeight;
    private Renderer renderer;
    private static readonly int State = Shader.PropertyToID("_State");

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent < MeshCollider>().sharedMesh = mesh;
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        renderer.material.SetInt(State, (int)Manager.self.env);
        CreateShape();
        UpdateMesh();
        UpdateCollider();
        UpdateTerrainDecoration();
    }

    private void UpdateCollider()
    {
        /* Force update collider */
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<MeshCollider>().enabled = true;
    }

    private void UpdateTerrainDecoration()
    {

        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 3; z < sizeZ; z++)
            {
                RaycastHit hit;
                Vector3 plotter = transform.position + ((isRight) ? new Vector3(-x, 10, -z) : new Vector3(x, 10, z));
                if (Physics.Raycast(plotter, Vector3.down ,  out hit, 50f, 1 << 0))
                {
                    /* Check if the terrain is slope. if so, fill it with rocks. */
                    if (Vector3.Dot(hit.normal,Vector3.up) <= 0.65f)
                    {
                        // Generate rock based on diagonally normal vector.
                        SpawnRock(hit.point);
                    }
                    else
                    {
                        /* Every grid has chance of 75 percent to grow plants */
                        if (Random.value < .75 && Manager.self.env == Manager.Environment.Jungle)
                        {
                            // Generate flower randomly between 1 to 3 space.
                            if (x % (int)Random.Range(1,3) == 0 && z % (int)Random.Range(1,3) == 0)
                                SpawnFlower(hit.point, hit.normal);
                            // Generate tree not too density.
                            if (x % 6 == 0 && z % 6 == 0)
                                SpawnTree(hit.point);
                        }

                        /* Every grid has chance of 35 percent to spawn cacti */
                        if (Random.value < .35 && Manager.self.env == Manager.Environment.Desert)
                        {
                            // Generate rocks not too density.
                            if (x % 6 == 0 && z % 6 == 0)
                                SpawnStuff(GetRandomCactus() ,hit.point);
                        }
                    }
                }
            }
        }
    }
    // 1  2  3  4  5  *6* 7  8  9  10 11 *12* 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30

    private void SpawnStuff(GameObject stuff,Vector3 point) //สุ่มเกิดทั่วไป 3f-2
    {
        Quaternion randomRotation = Quaternion.Euler(0,Random.value*360f,0);
        GameObject spawnStuff = Instantiate(stuff, point, randomRotation);
        spawnStuff.transform.localScale = Vector3.one * Random.Range(0.3f, 2);
        spawnStuff.transform.SetParent(transform);
    }

    private void SpawnRock(Vector3 point) //สุ่มเกิดหิน ทั่วไป
    {
        Quaternion randomRotation = Quaternion.Euler(0,Random.value*360f,0);
        GameObject spawnRock = Instantiate(GetRandomRock(), point, randomRotation);
        spawnRock.transform.localScale = Vector3.one * Random.Range(0.3f, 2);
        spawnRock.transform.SetParent(transform);
    }

    /*private void SpawnFlower(Vector3 point) //สุ่มเกิดดอก -2f -1
    {
        Quaternion randomRotation = Quaternion.Euler(0,Random.value*360f,0);
        GameObject spawnFlower = Instantiate(GetRandomFlower(), point, randomRotation);
        // random size
        spawnFlower.transform.localScale = Vector3.one * Random.Range(0.5f, 1);
        spawnFlower.transform.SetParent(transform);
    }*/
    private void SpawnFlower(Vector3 point, Vector3 direction) //สุ่มเกิดดอก -2f -1
    {
        Quaternion randomRotation = Quaternion.Euler(0,Random.value*360f,0);
        GameObject spawnFlower = Instantiate(GetRandomFlower(), point, randomRotation);
        // random size
        spawnFlower.transform.localScale = Vector3.one * Random.Range(0.5f, 1);
        spawnFlower.transform.LookAt(point + direction);
        spawnFlower.transform.localEulerAngles += new Vector3(90,0,0);
        spawnFlower.transform.SetParent(transform);
    }

    private void SpawnTree(Vector3 point)
    {
        Quaternion randomRotation = Quaternion.Euler(0,Random.value*360f,0);
        GameObject spawnTree = Instantiate(GetRandomTree(), point, randomRotation);
        // random size
        spawnTree.transform.localScale = Vector3.one * Random.Range(0.5f, 1);
        spawnTree.transform.SetParent(transform);
    }

    private GameObject GetRandomRock()
    {
        return rocks[Random.Range(0,rocks.Length-1)];
    }

    private GameObject GetRandomCactus()
    {
        return cacti[Random.Range(0, cacti.Length - 1)];
    }

    private GameObject GetRandomTree()
    {
        return trees[Random.Range(0,trees.Length-1)];
    }
    private GameObject GetRandomFlower()
    {
        return flowers[Random.Range(0,flowers.Length-1)];
    }

    private void CreateShape() //Mesh สร้างพื้น
    {
        vertices = new Vector3[(sizeX + 1) * (sizeZ + 1)];
        for (int i = 0,z = 0; z <= sizeZ; z++)
        {
            for (int x = 0; x <= sizeX; x++)
            {
                if(!isRight){ //ใช้ +
                    float y = Mathf.PerlinNoise((x  + transform.position.x+ MapGenerator.self.randomOffsets.x)*MapGenerator.self.sizes.x  ,(z+ MapGenerator.self.randomOffsets.y+transform.position.z)*MapGenerator.self.sizes.y )*4f;
                    if (minHeight > y) minHeight = y;
                    if (maxHeight < y) maxHeight = y;
                    y = Mathf.Round(y)/2f;
                    y *= 2;
                    vertices[i] = new Vector3(x,y * (1 - MathUtility.Zigmoid(z + 4,8)) - 1,z );
                    i++;
                }
                else //ใช้ -
                {
                    float y = Mathf.PerlinNoise((x  - transform.position.x- MapGenerator.self.randomOffsets.x)*MapGenerator.self.sizes.x ,(z+ MapGenerator.self.randomOffsets.y+ transform.position.z + sizeZ)*MapGenerator.self.sizes.y )*4f;
                    if (minHeight > y) minHeight = y;
                    if (maxHeight < y) maxHeight = y;
                    y = Mathf.Round(y)/2f;
                    y *= 2;
                    vertices[i] = new Vector3(x,y * (1 - MathUtility.Zigmoid(z + 4,8)) - 1,z);
                    i++;
                }
            }
        }
        triangles = new int[sizeX*sizeZ*6];
        int verts = 0;
        int tris = 0;
        for (int i = 0; i < sizeZ; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                triangles[tris + 0] = verts;
                triangles[tris + 1] = verts + sizeX + 1;
                triangles[tris + 2] = verts + 1;
                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + sizeX + 1;
                triangles[tris + 5] = verts + sizeX + 2;
                verts++;
                tris += 6;
            }
            verts++;
        }

        uv = new Color[vertices.Length];
        for (int i = 0,z = 0; z <= sizeZ; z++)
        {
            for (int x = 0; x <= sizeX; x++)
            {
                //float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                //uv[i] = gradient.Evaluate(height);
                if (Math.Abs(vertices[i].y - 1.0f) < 0.0001f) uv[i] = Color.green;
                i++;
            }

        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = uv;
        mesh.RecalculateNormals();
    }
}
