using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int x = 0;
    int y = 0;

    GridGen grid;
    List<List<char>> tiles;
    int rowsLength = 0;
    int columnsLength = 0;

    void Awake(){
        grid = this.gameObject.GetComponent<GridGen>();
        x = grid.playerX;
        y = grid.playerY;
        tiles = grid.tiles;

        rowsLength = grid.rowsLength;
        columnsLength = tiles.Count;
        print(x + " " + y);
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Move(x, y - 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(x, y + 1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
            Move(x - 1, y);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(x + 1, y);
    }

    void Move(int nextTileX, int nextTileY) {
        if (nextTileX > -1 && nextTileY > -1 && nextTileX < rowsLength && nextTileY < columnsLength) {
            if (tiles[nextTileY][nextTileX] == '.') {
                x = nextTileX;
                y = nextTileY;
            }
            else {
                print("This is a wall");
            }
        }
        else {
            print("Out of bounds");
        }
        print(x + " " + y);
    }
}
