namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("AScoreJam13/ScrollingMapGenerator")]
    public class ScrollingMapGeneratorBehvaiour : BehaviourBase
    {
        #region Members

        public Tilemap Floor;

        public TileBase[] FloorTiles;

        public TileBase[] WallTiles;

        public float ScrollSpeed;

        protected float ScrollY { get; set; }

        protected Vector2 CameraEdge { get; set; }

        protected Playermovement Player { get; set; }

        protected bool TilesPopulated { get; set; }

        #endregion

        #region Event Handlers

        private void Player_Died(object sender, System.EventArgs e)
        {
            Reset();
        }

        #endregion

        #region Public Methods

        public void Reset()
        {
            Floor
                .ClearAllTiles();

            Floor.transform.position = new Vector3(0, 0, 0);

            PopulateTiles();
        }

        #endregion

        #region Protected Methods

        protected void PopulateTiles()
        {
            var min = Floor
                .WorldToCell(-new Vector3(CameraEdge.x + 0.64f, CameraEdge.y + 0.64f, 0));

            var max = Floor
                .WorldToCell(new Vector3(CameraEdge.x + 0.64f, CameraEdge.y + 0.64f, 0));

            var size = new Vector3Int(max.x - min.x, max.y - min.y, 1);

            var tileBounds = new BoundsInt(min, size);

            for (int y = tileBounds.min.y; y < tileBounds.max.y; y++)
            {
                for (int x = tileBounds.min.x; x < tileBounds.max.x; x++)
                {
                    var tile = FloorTiles[0];
                    //if (x <= tileBounds.xMin + WallWidth ||
                        //x >= tileBounds.xMax - WallWidth)
                    //{
                        //tile = WallTiles[0];
                    //} 

                    Floor
                        .SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        protected void ScrollTiles()
        {
            var toScroll = Time.deltaTime * ScrollSpeed;

            Floor.transform.position += new Vector3(0, -toScroll, 0);

            ScrollY += toScroll;

            while (ScrollY >= 0.64f)
            {
                ScrollY -= 0.64f;

                var bounds = Floor.cellBounds;

                //Debug
                //.Log($"CELL BOUNDS BEFORE ADDING '{Floor.cellBounds}'");

                var tiles = new TileBase[bounds.size.x];
                // add new row of cells
                for (int x = 0; x < bounds.size.x; x++)
                {
                    var tile = FloorTiles[0];
                    //if (x <= WallWidth ||
                        //x >= bounds.size.x - WallWidth)
                    //{
                        //tile = WallTiles[0];
                    //}
                    //else
                    //{
                        if (Random.value >= 0.9)
                        {
                            tile = WallTiles[0];
                        }
                    //}

                    tiles[x] = tile;
                }
                var addBounds = new BoundsInt(bounds.xMin, bounds.yMax, 0, bounds.size.x, 1, 1);
                Floor
                    .SetTilesBlock(addBounds, tiles);

                //Debug
                //.Log($"CELL BOUNDS AFTER ADDING '{Floor.cellBounds}'");

                bounds = Floor.cellBounds;

                for (int i = 0; i < tiles.Length; i++)
                {
                    tiles[i] = null;
                }

                // delete the bottom row of cells
                var deleteBounds = new BoundsInt(bounds.xMin, bounds.yMin, 0, bounds.size.x, 1, 1);
                Floor
                    .SetTilesBlock(deleteBounds, tiles);

                Floor
                    .CompressBounds();

                //Debug
                //.Log($"CELL BOUNDS AFTER DELETING '{Floor.cellBounds}'");
            }
        }

        #endregion

        #region Unity

        protected void Update()
        {
            if (!TilesPopulated)
            {
                TilesPopulated = true;

                Vector2 topRightCorner = new Vector2(1, 1);

                CameraEdge = Camera
                    .main
                    .ViewportToWorldPoint(topRightCorner);

                Reset();
            }
            
            ScrollTiles();
        }

        protected override void Awake()
        {
            Player = FindObjectOfType<Playermovement>();
            Player.Died += Player_Died;
        }

        #endregion
    }
}