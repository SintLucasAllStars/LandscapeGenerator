using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace LandscapeGenerator
{

    [Serializable]
    public class ProceduralWorld
    {
        private bool _initialized = false;

        [Header("World Setup")] [SerializeField]
        public int size;

        [SerializeField] private List<HeightPass> passes = new List<HeightPass>();
        private List<PropGroup> _props = new List<PropGroup>();

        [Header("Rocks")] public List<GameObject> RockPrefabs;
        public float RockProbability;

        [Header("Trees")] public float TreeProbability;

        [Header("Events")] public UnityEvent generated = new UnityEvent();

        private float[,] _heights;
        float _max = 0;
        float _min = 0;

        public float[,] Heights
        {
            get
            {
                if (!_initialized) Init();
                return _heights;
            }
        }

        public float[,] NormalizedHeights(float gain)
        {
            if (!_initialized) Init();

            float[,] norm = new float[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    norm[x, z] = Utils.Map(_heights[x, z], _min, _max, 0f, gain);
                }
            }

            return norm;
        }

        public void Init()
        {
            if (!_initialized)
            {
                _heights = new float[size, size];
                _initialized = true;
                ProceduralManager.Instance.regenerate.AddListener(Generate);

                //NOTE: This class should also manage the water level and for example avoid trees from being created under water

                //Rocks
                PrefabLayer rl = new PrefabLayer();
                rl.SetPrefabs(RockPrefabs);
                _props.Add(rl);

                //trees
                TreeLayer tl = new TreeLayer();
                _props.Add(tl);

                Generate();
            }
        }

        public void Generate()
        {
            //cleaning up
            ProceduralManager.Instance.ResetSeed();
            foreach (PropGroup propGroup in _props)
            {
                propGroup.ClearAll();
            }

            //generating
            Debug.Log("Generating world...");
            float time = Time.realtimeSinceStartup;
            if (!_initialized) Init();

            foreach (PropGroup propGroup in _props)
            {
                propGroup.ClearAll();
            }

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    float height = CalculateHeight(x, z);

                    if (height > _max)
                    {
                        _max = height;
                    }
                    else if (height < _min)
                    {
                        _min = height;
                    }

                    _heights[x, z] = height;
                }
            }

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    //Rocks
                    float rockRand = UnityEngine.Random.value;

                    if (rockRand < RockProbability * Utils.Map(_heights[x, z], _min, _max, 0f, 1f))
                    {
                        int t = UnityEngine.Random.Range(0, RockPrefabs.Count);
                        _props[0].Add(new Vector3(x, 0.0f, z), t);
                    }

                    //Trees
                    float treeRand = UnityEngine.Random.value;

                    if (treeRand < TreeProbability * Utils.Map(_heights[x, z], _min, _max, 0f, 1f))
                    {
                        int t = UnityEngine.Random.Range(0, Terrain.activeTerrain.terrainData.treePrototypes.Length);
                        _props[1].Add(new Vector3(x, 0.0f, z), t);
                    }
                }
            }

            Debug.Log("Finished Generating - took " + (Time.realtimeSinceStartup - time) + "s");
            generated.Invoke();
        }

        public void InstantiateProps()
        {
            Debug.Log("Instantiating props...");
            float time = Time.realtimeSinceStartup;
            foreach (PropGroup propGroup in _props)
            {
                Debug.Log(propGroup.Count + " props");
                propGroup.InstantiateAll();
                Terrain t = Terrain.activeTerrain;
                Debug.Log("There are " + t.terrainData.treeInstanceCount + " Trees.");
                Debug.Log("There are " + t.terrainData.treePrototypes.Length + " Prototypes");
            }

            Debug.Log("Finished Instantiating - took " + (Time.realtimeSinceStartup - time) + "s");
        }

        float CalculateHeight(int x, int z)
        {
            float hv = 0;

            for (int i = 0; i < passes.Count; i++)
            {
                float val = 0;
                switch (passes[i].type)
                {
                    case HeightPass.PassType.PerlinBased:
                        float xCoord = ProceduralManager.Instance.PerlinSeed.x + x / (float) size * passes[i].detail;
                        float yCoord = ProceduralManager.Instance.PerlinSeed.y + z / (float) size * passes[i].detail;
                        val = Mathf.PerlinNoise(xCoord, yCoord) * passes[i].height;
                        break;
                    case HeightPass.PassType.RandomBased:
                        val = UnityEngine.Random.value * passes[i].height;
                        break;
                    case HeightPass.PassType.Vignette:
                        Vector2 center = new Vector2(size / 2.0f, size / 2.0f);
                        float dist = Vector2.Distance(center, new Vector2(x, z));
                        float maxDist = new Vector2(size, size).magnitude / 2.0f;
                        maxDist -= passes[i].detail;
                        float normalizedDist = Utils.Map(dist, 0f, maxDist, 0f, 1f);
                        val = passes[i].height * Mathf.Cos(normalizedDist * Mathf.PI);
                        break;
                }

                hv += val - passes[i].height / 2f;
            }

            return hv;
        }
    }
}
