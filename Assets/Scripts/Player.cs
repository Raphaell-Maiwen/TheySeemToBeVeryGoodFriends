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

    void Awake() {
        grid = this.gameObject.GetComponent<GridGen>();
        x = grid.playerX;
        y = grid.playerY;
        tiles = grid.tiles;

        rowsLength = grid.rowsLength;
        columnsLength = grid.columnsLength;
        print(x + " " + y);
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Move(x, y + 1, 'u');
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(x, y - 1, 'd');
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
            Move(x - 1, y, 'l');
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(x + 1, y, 'r');
    }

    void Move(int nextTileX, int nextTileY, char direction) {
        if (nextTileX > -1 && nextTileY > -1 && nextTileX < rowsLength && nextTileY < columnsLength) {
            //if (tiles[nextTileX][nextTileY] == '.') {
            if (MoveSprite(nextTileX, nextTileY, 'P', direction)) { 
                
            }
            //}
            //else 
            if (tiles[nextTileX][nextTileY] == 'W' || tiles[nextTileX][nextTileY] == 'M') {
                if (MoveSprite(nextTileX, nextTileY, tiles[nextTileX][nextTileY], direction)) {
                    
                }
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

    bool MoveSprite(int newX, int newY, char symbol, char direction) {
        char nextSymbol = tiles[newX][newY];
        if (nextSymbol == '.') {
            tiles[newX][newY] = symbol;
            if (symbol == 'P') {
                tiles[x][y] = '.';
                tiles[newX][newY] = 'P';
                x = newX;
                y = newY;
                grid.tiles = tiles;
                grid.PutSprites();
            }
            return true;
        }
        else if (nextSymbol == '#') {
            return false;
        }

        return true;
    }
}
