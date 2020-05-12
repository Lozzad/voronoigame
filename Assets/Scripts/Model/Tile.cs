using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    EMPTY,
    NOTEMPTY
}

public class Tile {
    private TileType _type = TileType.NOTEMPTY;

    //public GameObject tileObject;
    public Region region;
    World world;
    public int X { get; protected set; }
    public int Y { get; protected set; }

    public Tile (World world, Region region, int x, int y) {
        this.world = world;
        this.region = region;
        X = x;
        Y = y;
    }

    // work out the nearest "building" to each tile. if its same dist as another tile
    public IEnumerator VoronoiOwner (System.Action<Region> callback) {
        Building nearest = null;
        float minDist = Mathf.Infinity;
        float dist;

        foreach (Building building in world.buildings) {
            dist = Vector2.Distance (building.pos, new Vector2 (X, Y));
            if (dist == minDist) {
                //Debug.Log ("Tile (" + X + ", " + Y + ") contested between region " + nearest.region + "and " + building.region);

            }
            if (dist < minDist) {
                nearest = building;
                minDist = dist;
            }
        }
        yield return null;
        callback (nearest.region);
    }
}