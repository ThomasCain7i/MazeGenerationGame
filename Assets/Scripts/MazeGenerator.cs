using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject Wall; //Reference to the wall prefab
    public int mazeSize = 15; //Size of the maze
    public float perlinThreshold = 0.5f; // threshold value for Perlin noise
    public float startPointThreshold = 0.1f;
    public float endPointThreshold = 0.9f;

    public void Start()
    {
        for (int i = 0; i < mazeSize; i++)
        {
            float perlinValue = Mathf.PerlinNoise(i / (float)mazeSize, i / (float)mazeSize);
            //Create a for loop that will iterate over the rows of the maze.
            if (perlinValue < perlinThreshold)
            {
                GameObject newWall = Instantiate(Wall, new Vector3(i, 0, i), Quaternion.identity);
                newWall.transform.parent = transform;
            }
        }
        for (int j = 0; j < mazeSize; j++)
        {

        }
    }

    public void Update()
    {
        transform.localScale = new Vector3(15, 15, 15);
    }
}