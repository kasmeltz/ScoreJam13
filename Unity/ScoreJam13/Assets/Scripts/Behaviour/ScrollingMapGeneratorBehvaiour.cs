namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("AScoreJam13/ScrollingMapGenerator")]
    public class ScrollingMapGeneratorBehvaiour : BehaviourBase
    {
        #region Members

        public bool IsDemo;

        public Tilemap Floor;

        public TileBase[] FloorTiles;

        public TileBase[] WallTiles;

        public BlinkPowerUp[] BlinkTilePrefabs;

        public PowerupBehaviourBase[] Powerups;

        public float[] PowerupProbabilities;

        public float ScrollSpeed;

        public float SpeedIncreasePerSecond;

        public CoinBehaviour CoinPrefab;

        public GameObject CoinHolder;

        public bool SpawnBlinkTiles { get; set; }

        protected float ScrollY { get; set; }

        protected Vector2 CameraEdge { get; set; }

        public float ActualScrollSpeed { get; protected set; }

        protected Playermovement Player { get; set; }

        protected ScoreCounter ScoreCounter { get; set; }

        protected ProbabilityChooser<PowerupBehaviourBase> PowerupProbabilityChooser { get; set; }
        
        #endregion

        #region Public Methods

        public void Reset()
        {
            if (ScoreCounter != null)
            {
                ScoreCounter
                    .Reset();
            }

            SpawnBlinkTiles = false;

            ScrollY = 0;

            ActualScrollSpeed = ScrollSpeed;
                
            Vector2 topRightCorner = new Vector2(1, 1);

            CameraEdge = Camera
                .main
                .ViewportToWorldPoint(topRightCorner);

            foreach (Transform item in CoinHolder.transform)
            {
                var powerup = item
                    .GetComponent<PowerupBehaviourBase>();

                if (powerup != null)
                {
                    powerup
                        .DoWhenDestroyed();
                }

                DestroyComponent(item);
            }

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
                .WorldToCell(-new Vector3(CameraEdge.x + 1.28f, CameraEdge.y + 1.28f, 0));

            var max = Floor
                .WorldToCell(new Vector3(CameraEdge.x + 1.28f, CameraEdge.y + 1.28f, 0));

            var size = new Vector3Int(max.x - min.x, max.y - min.y, 1);

            var tileBounds = new BoundsInt(min, size);

            for (int y = tileBounds.min.y; y < tileBounds.max.y; y++)
            {
                for (int x = tileBounds.min.x; x < tileBounds.max.x; x++)
                {
                    var tile = FloorTiles[0];

                    Floor
                        .SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        protected void UpdateChildren(Vector3 scrollVector)
        {
            foreach (Transform child in CoinHolder.transform)
            {
                bool shouldScroll = false;

                var coin = child.GetComponent<CoinBehaviour>();
                var powerup = child.GetComponent<PowerupBehaviourBase>();

                if (coin != null)
                {
                    shouldScroll = true;

                }
                
                if (powerup != null && !powerup.IsPickedUp)
                {
                    shouldScroll = true;
                }
            
                if (shouldScroll)
                {
                    child.position += scrollVector;
                }

                bool shouldBeRemoved = false;

                if (coin != null)
                {
                    shouldBeRemoved = true;

                }

                if (powerup != null && !powerup.IsPickedUp)
                {
                    shouldBeRemoved = true;
                }

                if (shouldBeRemoved && child.position.y < -CameraEdge.y)
                {
                    DestroyComponent(child);
                }
            }
        }        

        protected void SpawnCoin(int mapX, int mapY)
        {
            var worldPos = Floor
                .CellToWorld(new Vector3Int(mapX, mapY, 0));

            var coin = Instantiate(CoinPrefab);
            coin
                .transform
                .SetParent(CoinHolder.transform);

            coin.transform.position = worldPos + new Vector3(0.32f, 0.32f, 0);
        }
        
        protected void SpawnBlinkTile(int mapX, int mapY)
        {
            var worldPos = Floor
                .CellToWorld(new Vector3Int(mapX, mapY, 0));

            int index = Random
                .Range(0, BlinkTilePrefabs.Length);

            var prefab = BlinkTilePrefabs[index];

            var tile = Instantiate(prefab);

            tile
                .transform
                .SetParent(CoinHolder.transform);

            tile.transform.position = worldPos + new Vector3(0.32f, 0.32f, 0);
        }

        protected void SpawnPowerUp(int mapX, int mapY)
        {
            var worldPos = Floor
                .CellToWorld(new Vector3Int(mapX, mapY, 0));

            var powerUpPrefab = PowerupProbabilityChooser
                .ChooseItem();

            var powerUp = Instantiate(powerUpPrefab);

            powerUp
                .transform
                .SetParent(CoinHolder.transform);

            powerUp.transform.position = worldPos + new Vector3(0.32f, 0.32f, 0);
        }

        protected void ScrollTiles()
        {
            int tileLevel = 0;
            if (!IsDemo)
            {
                tileLevel = ScoreCounter.Level / 2;
                if (tileLevel >= FloorTiles.Length)
                {
                    tileLevel = FloorTiles.Length - 1;
                }
            }
           
            var toScroll = Time.deltaTime * ActualScrollSpeed;

            Vector3 scrollVector = new Vector3(0, -toScroll, 0);
            Floor.transform.position += scrollVector;

            if (!IsDemo)
            {
                UpdateChildren(scrollVector);
            }

            ScrollY += toScroll;

            while (ScrollY >= 0.64f)
            {
                ScrollY -= 0.64f;

                var bounds = Floor.cellBounds;

                var tiles = new TileBase[bounds.size.x];
                // add new row of cells
                for (int x = 0; x < bounds.size.x; x++)
                {
                    var tile = FloorTiles[tileLevel];

                    if (!IsDemo)
                    {
                        if (Random.value >= 0.9)
                        {
                            tile = WallTiles[tileLevel];
                        }
                        else
                        {
                            if (Random.value >= 0.998)
                            {
                                SpawnCoin(bounds.xMin + x, bounds.yMax);
                            }

                            if (Random.value >= 0.998)
                            {
                                SpawnPowerUp(bounds.xMin + x, bounds.yMax);
                            }

                            if (SpawnBlinkTiles)
                            {
                                if (Random.value >= 0.98)
                                {
                                    SpawnBlinkTile(bounds.xMin + x, bounds.yMax);
                                }
                            }
                        }
                    }

                    tiles[x] = tile;
                }

                var addBounds = new BoundsInt(bounds.xMin, bounds.yMax, 0, bounds.size.x, 1, 1);
                Floor
                    .SetTilesBlock(addBounds, tiles);

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
            }
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Player = FindObjectOfType<Playermovement>();
            ScoreCounter = FindObjectOfType<ScoreCounter>();

            PowerupProbabilityChooser = new ProbabilityChooser<PowerupBehaviourBase>();

            for (int i = 0; i < PowerupProbabilities.Length; i++)
            {
                PowerupProbabilityChooser
                    .AddItem(Powerups[i], PowerupProbabilities[i]);
            }
        }

        protected void Update()
        {           
            if (!Playermovement.IsPlaying && !IsDemo)
            {
                return;
            }

            float speedUpdate = SpeedIncreasePerSecond * Time.deltaTime;
            ActualScrollSpeed *= (1 + speedUpdate);

            ScrollTiles();
        }

        #endregion
    }
}