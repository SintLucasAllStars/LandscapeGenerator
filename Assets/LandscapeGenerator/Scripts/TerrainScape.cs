using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

namespace LandscapeGenerator
{

    public class TerrainScape : Landscape
    {
        private Terrain t;
        public float gain = 1f;

        private void Start()
        {
            t = GetComponent<Terrain>();

            if (t == null)
            {
                Debug.LogError("Please put the TerrainScape script on a terrain");
            }

            Init();
        }

        public override void Clean()
        {

        }

        public override void Generate()
        {
            Clean();

            //Setting the heights of the terrain
            t.terrainData.heightmapResolution = ProceduralManager.Instance.world.size;
            t.terrainData.alphamapResolution = ProceduralManager.Instance.world.size;
            t.terrainData.SetHeights(0, 0, ProceduralManager.Instance.world.NormalizedHeights(gain));


            //Here we generate custom splat map from height data.
            float[,,] map = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight, 3];

            // For each point on the alphamap...
            for (int y = 0; y < t.terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < t.terrainData.alphamapWidth; x++)
                {
                    // Get the normalized terrain coordinate that
                    // corresponds to the the point.
                    float normX = x * 1.0f / (t.terrainData.alphamapWidth - 1);
                    float normY = y * 1.0f / (t.terrainData.alphamapHeight - 1);

                    // Get the steepness value at the normalized coordinate.
                    float angle = t.terrainData.GetSteepness(normX, normY);
                    float height = t.terrainData.GetHeight(x, y);

                    // Steepness is given as an angle, 0..90 degrees. Divide
                    // by 90 to get an alpha blending value in the range 0..1.
                    //float frac = Mathf.Clamp(Utils.Map(height, .5f, 1f, 0f, 1f), 0f, 1f);
                    float rock = Mathf.Clamp(Utils.Map(height / t.terrainData.size.y, 0, 1, -1, 1), 0f, 1f);
                    float grass = Mathf.Clamp(Utils.Map(height / t.terrainData.size.y, 0, 1, 1, -.5f), 0f, 1f);
                    float sand = Mathf.Clamp(Utils.Map(height, 0, 300, 2, 0), 0f, 1f);
                    map[x, y, 0] = rock;
                    map[x, y, 1] = grass;
                    map[x, y, 2] = sand;
                }
            }

            t.terrainData.SetAlphamaps(0, 0, map);

            ProceduralManager.Instance.world.InstantiateProps();
        }
    }
}