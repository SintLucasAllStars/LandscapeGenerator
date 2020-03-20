using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LandscapeGenerator
{

    public class TreeLayer : PropGroup
    {
        public override void ClearAll()
        {
            //Terrain.activeTerrain.terrainData.SetTreeInstances(new TreeInstance[0], true);
            props.Clear();
        }

        protected override void Instantiate(Prop prop)
        {

        }

        // We need to override this one to refresh the terrain after adding all the trees.
        public override void InstantiateAll()
        {
            List<TreeInstance> trees = new List<TreeInstance>();
            for (int i = 0; i < props.Count; i++)
            {
                TreeInstance tree = new TreeInstance();
                tree.position = props[i].Position / ProceduralManager.Instance.world.size;
                tree.prototypeIndex = props[i].Type;
                tree.color = Color.white;
                tree.lightmapColor = Color.white;
                tree.heightScale = 1f;
                tree.widthScale = 1f;

                trees.Add(tree);
            }

            Terrain t = Terrain.activeTerrain;
            t.terrainData.SetTreeInstances(trees.ToArray(), true);
            t.Flush();
        }
    }
}