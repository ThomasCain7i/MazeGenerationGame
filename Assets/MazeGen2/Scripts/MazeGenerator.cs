using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;

    private void Start()
    {
        StartCoroutine(GenerateMaze(mazeSize));
    }

    IEnumerator GenerateMaze(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        //Create the nodes
        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
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

        //Select a starting node at random from all nodes genrated in the list
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);

        //Add the starting node to the current path state
        currentPath[0].SetState(NodeState.Current);

        //Check if there are still nodes left if so continue looping
        while(CompletedNodes.Count < nodes.Count)
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
            if(currentNodeY < size.y - 1)
            {
                if (!CompletedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }

            //Check node to the below of current node
            if(currentNodeY > 0)
            {
                if (!CompletedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            //Chosing next node
            if(possibleDirections.Count > 0)
            {
                //Randomly choose direction
                int chosenDirection = Random.Range(0, possibleDirections.Count);

                //Add the chosen node to the chosen dirtection index
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                //Add the chosen node to current path
                currentPath.Add(chosenNode);

                //Set chosen node to current state
                chosenNode.SetState(NodeState.Current);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
