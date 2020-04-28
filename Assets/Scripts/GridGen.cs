using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGen : MonoBehaviour
{
    [TextArea(10, 10)]
    public string levelString;

    [HideInInspector]
    public List<List<char>> tiles;

    [HideInInspector]
    public int rowsLength = 0;

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
        TestLevel();
    }

    void BuildLevel() {
        tiles = new List<List<char>>();

        string[] rows =  levelString.Split('\n');

        if (rows.Length > 0)
            rowsLength = rows[0].Length;
        else
            InvalidLevel("No level entered");

        for (int i = 0; i < rows.Length; i++) {
            if (rows[i].Length == rowsLength) {
                tiles.Add(new List<char>());
                for (int j = 0; j < rows[i].Length; j++) {
                    tiles[i].Add(rows[i][j]);
                    if (tiles[i][j] == 'P')
                        SetPlayerPos(j, i);
                }
            }
            else {
                InvalidLevel("One row is not the right length: " + (i + 1));
                break;
            }
        }

        if (playerX == -1 || playerY == -1)
            InvalidLevel("No player");
        else {
            //PutSprites();
            this.gameObject.AddComponent<Player>();
        }
    }

    void PutSprites() {
        TileBase[] whatever = new TileBase[tiles.Count * rowsLength];
        Vector3Int[] vectWhatever = new Vector3Int[tiles.Count * rowsLength];

        int xDecal = 0;
        int yDecal = 0;
        int count = 0;
        for (int i = 0; i < whatever.Length; i++) {
            whatever[i] = background;
            vectWhatever[i] = baseLevel.origin + 
                Vector3Int.RoundToInt(new Vector3(baseLevel.cellSize.x * xDecal, baseLevel.cellSize.y * yDecal, baseLevel.cellSize.z));

            count++;
            xDecal++;
            if (count % rowsLength == 0) {
                xDecal = 0;
                yDecal++;
            }
        }

        baseLevel.SetTiles(vectWhatever, whatever);

        baseLevel.CompressBounds();

        var localTilesPositions = new List<Vector3Int>();
        foreach (var pos in baseLevel.cellBounds.allPositionsWithin) {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            localTilesPositions.Add(localPlace);
        }

        for (int i = 0; i < tiles.Count * rowsLength; i++) {
            for (int j = 0; j < tiles[i].Count; j++) {
                char character = tiles[i][j];
                if (tiles[i][j] == 'P') {
                    tiles[i][j] = '.';
                    baseLevel.SetTile(localTilesPositions[i * j + i], player);
                }
                else if (tiles[i][j] == 'W')
                    baseLevel.SetTile(localTilesPositions[i * j + i], wall);
                else if (tiles[i][j] == 'W')
                    baseLevel.SetTile(localTilesPositions[i * j + i], woman);
                else if (tiles[i][j] == 'M')
                    baseLevel.SetTile(localTilesPositions[i * j + i], man);

                //baseLevel.SetTile(localTilesPositions[i * j + i],);
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

    void TestLevel() {
        for (int i = 0; i < tiles.Count; i++) {
            for (int j = 0; j < tiles[i].Count; j++) {
                print(tiles[i][j]);
            }
        }
    }
}
