using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Naming the states
public enum NodeState
{
    Available,
    Current,
    Completed
}
public class MazeNode : MonoBehaviour
{
    //Allow editing in inspector
    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer floor;

    //Removing the walls
    public void RemoveWall(int wallToRemove)
    {
        //Set walls to inactive
        walls[wallToRemove].gameObject.SetActive(false);
    }

    public void SetState(NodeState state)
    {
        //State machine to control the colours of the generating maze
        switch (state)
        {
            case NodeState.Available:
                floor.material.color = Color.white; 
                break;
            case NodeState.Current:
                floor.material.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.material.color = Color.red;
                break;
        }
    }
}
