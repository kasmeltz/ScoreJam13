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

        public float SpeedIncreasePerSecond;

        public CoinBehaviour CoinPrefab;

        public GameObject CoinHolder;

        protected float ScrollY { get; set; }

        protected Vector2 CameraEdge { get; set; }

        public float ActualScrollSpeed { get; protected set; }

        protected Playermovement Player { get; set; }

        protected ScoreCounter ScoreCounter { get; set; }

        #endregion

        #region Public Methods

        public void Reset()
        {
            ScoreCounter
                .Reset();

            ActualScrollSpeed = ScrollSpeed;
                
            Vector2 topRightCorner = new Vector2(1, 1);

            CameraEdge = Camera
                .main
                .ViewportToWorldPoint(topRightCorner);

            foreach (Transform coin in CoinHolder.transform)
            {
                DestroyComponent(coin);
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
                if (child.gameObject.CompareTag("Coin"))
                {
                    shouldScroll = true;
                }

                if (shouldScroll)
                {
                    child.position += scrollVector;
                }

                bool shouldBeRemoved = false;
                if (child.gameObject.CompareTag("Coin"))
                {
                    shouldBeRemoved = true;
                }

                if (shouldBeRemoved && child.position.y < -CameraEdge.y)
                {
                    DestroyComponent(child);
                }
            }
        }        

        protected void SpawnCoin()
        {
            var coin = Instantiate(CoinPrefab);

            var sr = Player.GetComponent<SpriteRenderer>();            
            var w = (sr.sprite.rect.width * Player.transform.localScale.x) / 100;
            var h = (sr.sprite.rect.height * Player.transform.localScale.y) / 100;

            float minX = -CameraEdge.x - w;
            float maxX = CameraEdge.x + w;
            float minY = -CameraEdge.y - h;
            float maxY = CameraEdge.y + h;

            float x = Random.Range(minX, maxX);
            float y = Random.Range(maxY, maxY);
            
            coin.transform.SetParent(CoinHolder.transform);
            coin.transform.position = new Vector3(x, y, 0);
        }

        protected void ScrollTiles()
        {
            int tileLevel = ScoreCounter.Level;
            if (tileLevel >= FloorTiles.Length)
            {
                tileLevel = FloorTiles.Length - 1;
            }

            if (Random.value >= 0.998)
            {
                SpawnCoin();
            }

            var toScroll = Time.deltaTime * ActualScrollSpeed;

            Vector3 scrollVector = new Vector3(0, -toScroll, 0);
            Floor.transform.position += scrollVector;

            UpdateChildren(scrollVector);

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
                    if (Random.value >= 0.9)
                    {
                        tile = WallTiles[tileLevel];
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
        }

        protected void Update()
        {           
            if (!Playermovement.IsPlaying)
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