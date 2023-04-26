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
        [SerializeField] [Range(0, 30)] private int _bordersSize;

        public int Seed => _seed;
        public int Width => _width;
        public int Hight => _hight;
        public int BordersSize => _bordersSize;

        [ContextMenu("Generate New Seed")]
        private void GenerateNewSeed()
        {
            Debug.Log("previos seed: " + _seed);
            _seed = Random.Range(0, 999999);
        }

        public int GetTileWeight(int x, int y)
        {
            float noize = Mathf.PerlinNoise((x + _seed) / _blur, (y + _seed) / _blur);
            noize = Mathf.Clamp(noize, 0, 1);
            int result = Mathf.RoundToInt(noize * _weight);
            return result;
        }
    }
}