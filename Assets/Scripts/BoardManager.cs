using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count (5, 9);
    public Count foodCount = new Count (1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        //loop through columns (x axis)
        for (int x = 1; x < columns - 1; x ++)
        {
            //within each column, loop through rows (y axis)
            for (int y = 1; y < rows - 1; y ++)
            {
                //at each index add a new Vector3 to our list with the x and y co ordinates of that position
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }


    //set up outer walls and floor of game board
    void BoardSetup ()
    {
        boardHolder = new GameObject("Board").transform;

        //loop x axis starting from -1 (to fill corner) with floor or outerwall edge tiles
        for (int x = -1; x < columns + 1; x ++)
        {
            //loop along y axis from -1 to place floor or outerwall tiles
            for (int y = -1; y < rows; y ++)
            {
                //choose a random tile from array of floor tile prefabs and prepare to instantiate it
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                
                //check if current position is at board edge, if so choose outerwall prefab from array
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //set parent of new object instance to boardHolder, organisational and avoids clutter of hierachy
                instance.transform.SetParent(boardHolder);
            }
        }
    }
}
