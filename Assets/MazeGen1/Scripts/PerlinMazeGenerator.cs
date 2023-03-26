using UnityEngine;

public class PerlinMazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab; // reference to the cube wall prefab
    public int mazeSize = 15; // size of the maze
    public float perlinThreshold; // threshold value for Perlin noise
    public float startPointThreshold = 0.1f; // threshold value for start point
    public float endPointThreshold = 0.9f; // threshold value for end point

    public bool wallHere = false;

    public GameObject startPoint; // reference to the start point object

    void Start()
    {
        perlinThreshold = Random.Range(0.1f, 0.9f);
        // generate maze walls
        for (int i = 0; i < mazeSize; i++)
        {
            for (int j = 0; j < mazeSize; j++)
            {
                
                // generate Perlin noise value for current cell
                float perlinValue = Mathf.PerlinNoise(i / (float)mazeSize, j / (float)mazeSize);
                
                // instantiate a new wall prefab at the current position if Perlin noise value is below threshold
                if (perlinValue < perlinThreshold)
                {
                    GameObject newWall = Instantiate(wallPrefab, new Vector3(i, 0, j), Quaternion.identity);
                    newWall.transform.parent = transform;
                }
            }
        }
    }
}

/*
No idea what the perlin noise does (i get it effects how the walls are instantiated just dont understand how)
Creates a 15 - 15 grid of walls at the original rotation
*/


/*
It seems that you are creating the start point object inside the loop that generates the maze. 
This means that every time a cell with a Perlin noise value below the start point threshold is found, a new start point object will be created, which is not what you want.
To fix this issue, you can move the creation of the start point object outside of the loop that generates the maze, 
 and use the startPoint variable to store a reference to the start point object. 
Here's the modified code ^
With this modification, the script should generate the maze walls and create only one start point object when a cell with a Perlin noise value below the start point threshold is found.
*/