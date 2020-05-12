using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {
    public Region region;

    public Vector2 pos;

    //public Sprite sprite;

    public Building (int x, int y, Region region) {
        this.pos.x = x;
        this.pos.y = y;
        this.region = region;
    }
}