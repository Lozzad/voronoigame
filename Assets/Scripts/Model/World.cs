using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    int numRegions;
    int regionEdgeLength;
    Region[, ] regions;
    public List<Building> buildings;
    public Tile[, ] Tiles { get; protected set; }

    //initialise the world, given a num regions
    public World (int sqrRootRegions, int regionEdgeLength) {
        Width = Height = sqrRootRegions * regionEdgeLength;
        Tiles = new Tile[Width, Height];
        buildings = new List<Building> ();
        regions = new Region[sqrRootRegions, sqrRootRegions];

        for (int x = 0; x < sqrRootRegions; x++) {
            for (int y = 0; y < sqrRootRegions; y++) {
                //colour for each region
                Color col = Random.ColorHSV (0f, 1f, 1f, 1f, 0.5f, 1f);
                regions[x, y] = new Region (this, regionEdgeLength, x, y, col);
                Debug.Log ("<color=col>region</color>" + x + ", " + y + " created");
            }
        }
    }

    public void PlaceTile (Tile tile) {
        if (Tiles[tile.X, tile.Y] != null) {
            Debug.LogError ("tile added at (" + tile.X + ", " + tile.Y + ") already exists!");
            return;
        }
        Tiles[tile.X, tile.Y] = tile;
    }

    public Tile GetTileAt (int x, int y) {
        if (Tiles[x, y] == null) {
            Debug.LogError ("Tile at (" + x + ", " + y + ") does not exist");
            return null;
        }
        return Tiles[x, y];
    }

    public IEnumerator VoronoiizeTiles () {
        foreach (Tile t in Tiles) {
            //non monobehaviour scripts cant call coroutines, so theyre called thru the 
            //worldcontroller
            WorldController.Instance.StartCoroutine (
                t.VoronoiOwner ((returnValue) => {
                    t.region = returnValue;
                })
            );
        }
        yield return null;
    }
}