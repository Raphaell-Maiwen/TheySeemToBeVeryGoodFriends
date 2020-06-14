using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int x = 0;
    int y = 0;

    GridGen grid;
    //List<List<char>> tiles;
    int rowsLength = 0;
    int columnsLength = 0;

    void Awake() {
        grid = this.gameObject.GetComponent<GridGen>();
        x = grid.playerX;
        y = grid.playerY;
        //tiles = grid.tiles;

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
        else if (Input.GetKeyDown(KeyCode.Z))
            RevertState();
        else if (Input.GetKeyDown(KeyCode.R))
            grid.Reset();
        else if (Input.GetKeyDown(KeyCode.S))
            StartCoroutine(grid.LoadNextLevel());
    }

    void Move(int nextTileX, int nextTileY, char direction) {
        if (nextTileX > -1 && nextTileY > -1 && nextTileX < rowsLength && nextTileY < columnsLength) {
            if (MoveSprite(nextTileX, nextTileY, 'P', direction)) {
                grid.CheckHeteroMatching();
                grid.CheckWin();
                grid.AddState();
                grid.removeFirstState = true;
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
        char nextSymbol = grid.tiles[newX][newY];
        if (nextSymbol == '.') {
            grid.tiles[newX][newY] = symbol;
            if (symbol == 'P') {
                MovePlayer(newX, newY);
            }
            return true;
        }
        else if (nextSymbol == '#') {
            return false;
        }
        else if (nextSymbol == 'W') {
            if (symbol == 'W' || symbol == 'P') {
                Vector2 dir = NextPosition(direction);
                int newNewX = newX + (int)dir.x;
                int newNewY = newY + (int)dir.y;

                if (MoveSprite(newNewX, newNewY, nextSymbol, direction)) {
                    if (symbol == 'W') {
                        grid.tiles[newX][newY] = symbol;
                        return true;
                    }
                    if (symbol == 'P')
                        return MovePlayer(newX, newY);
                }
            }
        }
        else if (nextSymbol == 'M') {
            if (symbol == 'M') {
                //TODO: change this to G
                grid.tiles[newX][newY] = 'G';
                return true;
            }
            else if (symbol == 'P') {
                Vector2 dir = NextPosition(direction);
                int newNewX = newX + (int)dir.x;
                int newNewY = newY + (int)dir.y;

                if (MoveSprite(newNewX, newNewY, nextSymbol, direction))
                     return MovePlayer(newX, newY);
            }
        }

        return false;
    }

    bool MovePlayer(int newX, int newY) {
        grid.tiles[x][y] = '.';
        grid.tiles[newX][newY] = 'P';
        x = newX;
        y = newY;
        //grid.tiles = tiles;
        grid.PutSprites();

        return true;
    }

    void RevertState() {
        grid.RevertState();
        x = grid.playerX;
        y = grid.playerY;
        grid.removeFirstState = false;
    }

    Vector2 NextPosition(char direction) {
        if (direction == 'u')
            return new Vector2(0, 1);
        else if (direction == 'd')
            return new Vector2(0, -1);
        else if (direction == 'l')
            return new Vector2(-1, 0);
        else if (direction == 'r')
            return new Vector2(1, 0);

        return new Vector2(0,0);
    }
}
