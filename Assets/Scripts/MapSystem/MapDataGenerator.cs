using UnityEngine;

namespace MapSystem
{
    public class MapDataGenerator : MonoBehaviour
    {
        [SerializeField] private int _seed;
        [SerializeField] [Range(1, 100)] private float _weight;
        [SerializeField] [Range (1, 100)] private float _blur;
        [SerializeField] private int _width;
        [SerializeField] private int _hight;

        public int Seed => _seed;
        public int Width => _width;
        public int Hight => _hight;

        [ContextMenu("Generate New Seed")]
        private void GenerateNewSeed()
        {
            Debug.Log("previos seed: " + _seed);
            _seed = Random.Range(0, 999999);
        }

        public int GetTileNoize(int x, int y)
        {
            float noize = Mathf.PerlinNoise((x + _seed) / _blur, (y + _seed) / _blur);
            if (noize < 0) noize = 0;
            else if (noize > 1) noize = 1;
            int result = Mathf.RoundToInt(noize * _weight);
            return result;
        }
    }
}