using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;

    public GameObject player;

    private void Start()
    {
        StartCoroutine(GenerateMaze(mazeSize));
        player = GameObject.Find("Player");
    }

    IEnumerator GenerateMaze(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        //Create the nodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                // Centres the maze aeound 0 instead of the bottom left
                Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f));

                //Instantiate the prefabs at 0
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);

                //Add newNode to the nodes list
                nodes.Add(newNode);

                yield return null;
            }
        }

        //Create new lists for the current nodes and the previous nodes
        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> CompletedNodes = new List<MazeNode>();

        //Create new lists for the starting and ending nodes
        List<MazeNode> startNode = new List<MazeNode>();
        List<MazeNode> endNode = new List<MazeNode>();



        //Select a starting node at random from all nodes genrated in the list
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
        //startNodePos.Add(currentPath);

        //List to hold start and end nodes positions
        List<Vector3> startNodePos = new List<Vector3>();
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                for (int k = 0; k < nodes.Count; k++)
                {
                    Vector3 startPosition = new Vector3(i, j, k);
                    //Add the position to the list
                    startNodePos.Add(startPosition);
                    Vector3 firstStartPosition = startNodePos[0];

                    //Move the player to the start location
                    player.transform.position = new Vector3(i, j, k);
                }
            }
        }

        List<Vector3> endNodePos = new List<Vector3>();
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                for (int k = 0; k < nodes.Count; k++)
                {
                    Vector3 endPosition = new Vector3(i, j, k);
                    //Add the position to the list
                    endNodePos.Add(endPosition);
                }
            }
        }

        //Add the starting node to the current path state
        currentPath[0].SetState(NodeState.Current);

        //Check if there are still nodes left, if so continue looping
        while (CompletedNodes.Count < nodes.Count)
        {
            //Check the nodes next to the current node

            //Create new lists for which nodes can be selected next
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            //Index of current nodes in the currentNodes list instead of currentPaths list
            //Get position of nodes by checking the lists
            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            //4 If statments (1 for each direction)

            //Check node to the right of current node
            if (currentNodeX < size.x - 1)
            {
                //Checking if the node to the right is not in completed nodes or in the current path
                if (!CompletedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    //Add this node to possible directions and nodes index
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }

            //Check node to the left of current node
            if (currentNodeX > 0)
            {
                if (!CompletedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }

            //Check node to the above of current node
            if (currentNodeY < size.y - 1)
            {
                if (!CompletedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }

            //Check node to the below of current node
            if (currentNodeY > 0)
            {
                if (!CompletedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            //Chosing next node
            if (possibleDirections.Count > 0)
            {
                //Randomly choose direction
                int chosenDirection = Random.Range(0, possibleDirections.Count);

                //Add the chosen node to the chosen dirtection index
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                //Switch statement to decide which walls are destroyed
                switch (possibleDirections[chosenDirection])
                {
                    //Every node in the maze overlaps therefore you must remove the walls opposite sides of each node
                    //For example I will remove the top wall from the current node and the bottom wall from the chosen node so there is no overlapping
                    case 1:
                        //Remove the left wall from the chosen node
                        chosenNode.RemoveWall(1);
                        //Removing the wall from the right of the current wall
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;

                    case 2:
                        //Remove the right wall from the chosen node
                        chosenNode.RemoveWall(0);
                        //Remove the left wall from the current node
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;

                    case 3:
                        //Remove the bottom wall from the chosen node
                        chosenNode.RemoveWall(3);
                        //Remove the top wall from the current node
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;

                    case 4:
                        //Remove the top wall from the chosen node
                        chosenNode.RemoveWall(2);
                        //Remove the bottom wall from the current node
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                //Add the chosen node to current path
                currentPath.Add(chosenNode);

                //Set chosen node to current state
                chosenNode.SetState(NodeState.Current);
            }
            //No more possible nodes so backtrack the path until there are new possible directions
            else
            {
                //Add current node to the completed nodes path
                CompletedNodes.Add(currentPath[currentPath.Count - 1]);

                //Changing state of current node
                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                //removing that node from currentPath
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            //To show the generation of the maze
            yield return new WaitForSeconds(0.005f);
        }

        //Select a random node to become the end point
        CompletedNodes[^1].SetState(NodeState.Start);

        CompletedNodes[0].SetState(NodeState.End);
    }
}