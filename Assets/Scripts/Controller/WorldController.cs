using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
    #region singletonInstance

    public static WorldController Instance { get; protected set; }

    void Awake () {
        if (Instance != null) {
            Debug.LogError ("shouldnt be more than one worldcontroller");
        }
        Instance = this;
    }
    #endregion

    public World World { get; protected set; }

    public int numRegions = 9;
    public int regionEdgeLength = 64;

    public Dictionary<Tile, GameObject> tileGameObjectMap;
    public Dictionary<Building, GameObject> buildingGameObjectMap;

    public Sprite tileSprite;
    public Sprite buildingSprite;

    public bool voronoi;

    void Start () {
        buildingGameObjectMap = new Dictionary<Building, GameObject> ();
        tileGameObjectMap = new Dictionary<Tile, GameObject> ();

        World = new World (numRegions, regionEdgeLength);

        CreateWorldTiles ();
        GenerateBuildings ();
        if (voronoi) { StartCoroutine (GenerateVoronoiDiag ()); }
    }

    // Update is called once per frame
    void Update () {

    }

    void CreateWorldTiles () {
        for (int x = 0; x < World.Width; x++) {
            for (int y = 0; y < World.Height; y++) {
                Tile tile_data = World.GetTileAt (x, y);
                GameObject tile_go = new GameObject ();

                tileGameObjectMap.Add (tile_data, tile_go);

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector2 (tile_data.X, tile_data.Y);
                tile_go.transform.SetParent (this.transform, true);

                SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer> ();
                sr.sortingLayerName = "Tiles";

                sr.sprite = tileSprite;
                sr.color = tile_data.region.tint;
            }
        }
    }

    void GenerateBuildings () {
        foreach (var building_data in World.buildings) {
            GameObject building_go = new GameObject ();
            building_go.name = "building";

            SpriteRenderer sr = building_go.AddComponent<SpriteRenderer> ();
            sr.sortingLayerName = "Buildings";

            sr.sprite = buildingSprite;
            building_go.transform.position = new Vector2 (building_data.pos.x, building_data.pos.y);

            buildingGameObjectMap.Add (building_data, building_go);
        }
    }

    IEnumerator GenerateVoronoiDiag () {
        Debug.Log ("beginning process");
        yield return StartCoroutine (World.VoronoiizeTiles ());
        Debug.Log ("Beginning coloring");
        yield return StartCoroutine (ColorTiles ());
        Debug.Log ("Colouring finished");
    }

    private IEnumerator ColorTiles () {
        foreach (KeyValuePair<Tile, GameObject> t in tileGameObjectMap) {
            var sr = t.Value.GetComponent<SpriteRenderer> ();
            if (sr.color != t.Key.region.tint) {
                sr.color = t.Key.region.tint;
            }
            if (t.Key.Y == 0) {
                yield return new WaitForSeconds (0.1f);
            }
        }
        yield return null;
    }
}