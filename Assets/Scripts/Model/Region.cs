using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region {
    int edgeLength;
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public Color tint;
    World world;
    Building castle;

    public Region (World world, int edgeLength, int x, int y, Color tint) {
        this.world = world;
        this.edgeLength = edgeLength;
        this.tint = tint;
        X = x;
        Y = y;
        GenerateTiles ();
        PlaceCastle ();
    }

    void GenerateTiles () {
        for (int x = 0; x < edgeLength; x++) {
            for (int y = 0; y < edgeLength; y++) {
                var tile = new Tile (world, this, (X * edgeLength) + x, (Y * edgeLength) + y);
                world.PlaceTile (tile);
                //Debug.Log ("tile added at (" + tile.X + ", " + tile.Y + ")");
            }
        }
    }

    void PlaceCastle () {
        castle = new Building (
            Random.Range (X * edgeLength, X * edgeLength + edgeLength),
            Random.Range (Y * edgeLength, Y * edgeLength + edgeLength),
            this);
        world.buildings.Add (castle);
        Debug.Log ("added castle at " + castle.pos.x + castle.pos.x);
    }
}