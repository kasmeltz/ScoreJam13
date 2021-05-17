namespace KasJam.ScoreJam13.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("AScoreJam13/ScrollingMapGenerator")]
    public class ScrollingMapGeneratorBehvaiour : BehaviourBase
    {
        #region Members

        public float WallTileChance;

        public float SpawnPowerUpChance;
        
        public float SpawnCoinChance;

        public float SpawnBlinkTileChance;

        public float SpawnBombChance;

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

        public BombBehaviour BombPrefab;

        public bool SpawnBlinkTiles { get; set; }

        public bool SpawnBombs { get; set; }

        public bool SpawnFakeFloor { get; set; }

        protected float ScrollY { get; set; }

        protected Vector2 CameraEdge { get; set; }

        public float ActualScrollSpeed { get; protected set; }

        protected Playermovement Player { get; set; }

        protected ScoreCounter ScoreCounter { get; set; }

        protected ProbabilityChooser<PowerupBehaviourBase> PowerupProbabilityChooser { get; set; }

        #endregion

        #region Event Handlers

        private void Player_Died(object sender, System.EventArgs e)
        {
            Time.timeScale = 1;
            AudioManager.GlobalPitchModifier = 1;
        }

        #endregion

        #region Public Methods

        public void Reset()
        {
            if (ScoreCounter != null)
            {
                ScoreCounter
                    .Reset();
            }

            foreach (Transform item in CoinHolder.transform)
            {
                var powerup = item
                    .GetComponent<PowerupBehaviourBase>();

                if (powerup != null)
                {
                    powerup
                        .Die();

                    continue;
                }

                DestroyComponent(item);
            }

            Time.timeScale = 1;
            AudioManager.GlobalPitchModifier = 1;

            SpawnBlinkTiles = false;

            ScrollY = 0;

            ActualScrollSpeed = ScrollSpeed;
                
            Vector2 topRightCorner = new Vector2(1, 1);

            CameraEdge = Camera
                .main
                .ViewportToWorldPoint(topRightCorner);
           
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
                var bomb = child.GetComponent<BombBehaviour>();
                var blinkTile = child.GetComponent<BlinkPowerUp>();

                if (coin != null || 
                    blinkTile != null || 
                    bomb != null)
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

                if (coin != null || blinkTile != null)
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

        protected T SpawnObject<T>(T prefab, int mapX, int mapY) where T: Component
        {
            var worldPos = Floor
                .CellToWorld(new Vector3Int(mapX, mapY, 0));

            var item = Instantiate<T>(prefab);
            item
                .transform
                .SetParent(CoinHolder.transform);

            item.transform.position = worldPos + new Vector3(0.32f, 0.32f, 0);

            return item;
        }

        protected void SpawnCoin(int mapX, int mapY)
        {
            SpawnObject(CoinPrefab, mapX, mapY);
        }
        
        protected void SpawnBlinkTile(int mapX, int mapY)
        {
            int index = Random
                .Range(0, BlinkTilePrefabs.Length);

            var prefab = BlinkTilePrefabs[index];

            SpawnObject(prefab, mapX, mapY);
        }

        protected void SpawnPowerUp(int mapX, int mapY)
        {
            var powerUpPrefab = PowerupProbabilityChooser
                .ChooseItem();

            var powerUp = SpawnObject(powerUpPrefab, mapX, mapY);

            powerUp.Player = Player;
        }

        protected void SpawnBomb(int mapX, int mapY)
        {
            SpawnObject(BombPrefab, mapX, mapY);
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
                        if (Random.value <= WallTileChance)
                        {
                            tile = WallTiles[tileLevel];
                        }
                        else
                        {
                            if (Random.value <= SpawnCoinChance)
                            {
                                SpawnCoin(bounds.xMin + x, bounds.yMax);
                            }

                            if (Random.value <= SpawnPowerUpChance)
                            {
                                SpawnPowerUp(bounds.xMin + x, bounds.yMax);
                            }

                            if (SpawnBlinkTiles)
                            {
                                if (Random.value <= SpawnBlinkTileChance)
                                {
                                    SpawnBlinkTile(bounds.xMin + x, bounds.yMax);
                                }
                            }

                            if (SpawnBombs)
                            {
                                if (Random.value <= SpawnBombChance)
                                {
                                    SpawnBomb(bounds.xMin + x, bounds.yMax);
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
            Player.Died += Player_Died;

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
            if (!Player.IsPlaying && !IsDemo)
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