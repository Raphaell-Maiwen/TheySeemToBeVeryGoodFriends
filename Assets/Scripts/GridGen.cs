using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GridGen : MonoBehaviour
{
    public string nextScene;

    [TextArea(10, 10)]
    public string levelString;

    [HideInInspector]
    public List<List<char>> tiles;

    [HideInInspector]
    public int rowsLength = 0;
    [HideInInspector]
    public int columnsLength = 0;

    [HideInInspector]
    public int playerX = -1;
    [HideInInspector]
    public int playerY = -1;

    public Tilemap baseLevel;
    public TileBase player;
    public TileBase wall;
    public TileBase woman;
    public TileBase man;
    public TileBase background;
    public TileBase womanInLove;
    public TileBase manInLove;

    void Awake(){
        BuildLevel();
        //TestLevel();
    }

    void BuildLevel() {
        tiles = new List<List<char>>();

        string[] rows =  levelString.Split('\n');

        if (rows.Length > 0)
            rowsLength = rows[0].Length;
        else {
            InvalidLevel("No level entered");
            return;
        }

        //Confirms that rows are all of the same length
        for (int i = 0; i < rows.Length; i++) {
            if (rows[i].Length != rowsLength) {
                InvalidLevel("One row is not the right length: " + (i + 1));
                return;
            }
        }
            
        for (int i = 0; i < rowsLength; i++) {
            tiles.Add(new List<char>());
            for (int j = rows.Length - 1; j >= 0; j--) {
                tiles[i].Add(rows[j][i]);

                if (rows[j][i] == 'P') {
                    SetPlayerPos(i, rows.Length - 1 - j);
                }
            }
        }

        columnsLength = tiles[0].Count;

        if (playerX == -1 || playerY == -1)
            InvalidLevel("No player");
        else {
            PutSprites();
            this.gameObject.AddComponent<Player>();
        }
    }

    public void PutSprites() {
        TileBase[] tilesGraphics = new TileBase[tiles.Count * tiles[0].Count];
        Vector3Int[] tilesPos = new Vector3Int[tiles.Count * tiles[0].Count];

        int xDecal = 0;
        int yDecal = 0;
        int count = 0;
        for (int i = 0; i < tilesGraphics.Length; i++) {
            tilesGraphics[i] = background;
            tilesPos[i] = baseLevel.origin + 
                Vector3Int.RoundToInt(new Vector3(baseLevel.cellSize.x * xDecal, baseLevel.cellSize.y * yDecal, baseLevel.cellSize.z));

            count++;
            xDecal++;
            if (count % rowsLength == 0) {
                xDecal = 0;
                yDecal++;
            }
        }

        baseLevel.SetTiles(tilesPos, tilesGraphics);

        baseLevel.CompressBounds();

        var localTilesPositions = new List<Vector3Int>();
        foreach (var pos in baseLevel.cellBounds.allPositionsWithin) {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            localTilesPositions.Add(localPlace);
        }

        for (int i = 0; i < columnsLength; i++) {
            for (int j = 0; j < rowsLength; j++) {
                char character = tiles[j][i];
                //print(character);
                if (character == 'P') {
                    baseLevel.SetTile(localTilesPositions[i * rowsLength + j], player);
                }
                else if (character == '#')
                    baseLevel.SetTile(localTilesPositions[i * rowsLength + j], wall);
                else if (character == 'W') {
                    baseLevel.SetTile(localTilesPositions[i * rowsLength + j], woman);
                }
                else if (character == 'M') {
                    baseLevel.SetTile(localTilesPositions[i * rowsLength + j], man);
                }
                //Woman in love
                else if (character == 'L') {
                    baseLevel.SetTile(localTilesPositions[i * rowsLength + j], womanInLove);
                }
                //Man en amour
                else if (character == 'A') {
                    baseLevel.SetTile(localTilesPositions[i * rowsLength + j], manInLove);
                }
                //
            }
        }
    }

    void SetPlayerPos(int x, int y) {
        playerX = x;
        playerY = y;
        //tiles[y][x] = '.';
    }

    void InvalidLevel(string errorMessage) {
        Debug.LogError("Invalid Level >:( " + errorMessage);
    }

    public void CheckHeteroMatching() {
        for (int x = 0; x < rowsLength; x++) {
            for (int y = 0; y < columnsLength; y++) {
                if (tiles[x][y] == 'M') {
                    if (x > 0 && tiles[x - 1][y] == 'W')
                        HeteroMatch(x, y, x -1, y);
                    else if (x + 1 < rowsLength && tiles[x + 1][y] == 'W')
                        HeteroMatch(x, y, x + 1, y);
                    else if (y> 0 && tiles[x][y - 1] == 'W')
                        HeteroMatch(x, y, x, y - 1);
                    else if (y + 1 < columnsLength && tiles[x][y + 1] == 'W')
                        HeteroMatch(x, y, x, y + 1);
                }
            }
        }
    }

    void HeteroMatch(int x1, int y1, int x2, int y2) {
        tiles[x1][y1] = 'A';
        tiles[x2][y2] = 'L';
        PutSprites();
    }

    public void CheckWin() {
        bool won = true;
        for (int x = 0; x < rowsLength; x++) {
            for (int y = 0; y < columnsLength; y++) {
                if (tiles[x][y] == 'M' || tiles[x][y] == 'W') {
                    //print(tiles[x][y]);
                    won = false;
                    break;
                }
            }
            if (!won)
                break;
        }

        if (won) {
            Win();
        }
    }

    void Win() {
        print("Next level");
        Destroy(this.gameObject.GetComponent<Player>());
        StartCoroutine(LoadNextLevel());
    }

    public void Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void TestLevel() {
        for (int i = 0; i < tiles.Count; i++) {
            for (int j = 0; j < tiles[i].Count; j++) {
                //print(tiles[i][j]);
            }
        }
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }
}
