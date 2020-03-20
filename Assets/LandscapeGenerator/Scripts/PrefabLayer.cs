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

    public class PrefabLayer : PropGroup
    {
        private List<GameObject> _prefabs;
        private int TypeCount => _prefabs.Count;
        private GameObject _parent;

        public void SetPrefabs(List<GameObject> prefabs)
        {
            this._prefabs = prefabs;
        }

        public override void ClearAll()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Rock"))
            {
                Object.Destroy(go);
            }

            props.Clear();
        }

        protected override void Instantiate(Prop prop)
        {
            if (_parent == null) _parent = new GameObject("Rocks");
            if (prop.Type <= TypeCount)
            {
                GameObject go = Object.Instantiate(_prefabs[prop.Type],
                    HeightMapToWorldPosition(prop.Position) + Vector3.down,
                    Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f), _parent.transform);
                go.tag = "Rock";
            }
            else
            {
                Debug.LogError("Prop type mismatch");
            }
        }

        Vector3 HeightMapToWorldPosition(Vector3 p)
        {
            Vector3 pos = new Vector3();
            Terrain t = Terrain.activeTerrain;
            pos.x = Utils.Map(p.x, 0f, t.terrainData.heightmapResolution, t.GetPosition().x,
                t.GetPosition().x + t.terrainData.size.x);
            pos.z = Utils.Map(p.z, 0f, t.terrainData.heightmapResolution, t.GetPosition().z,
                t.GetPosition().z + t.terrainData.size.z);
            pos.y = t.SampleHeight(pos);
            return pos;
        }
    }
}