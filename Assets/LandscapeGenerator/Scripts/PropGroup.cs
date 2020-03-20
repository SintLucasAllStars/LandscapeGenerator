using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LandscapeGenerator
{

    [Serializable]
    public class PropGroup
    {
        [Serializable]
        public struct Prop
        {
            public Vector3 Position;
            public int Type;
        }

        [SerializeField] protected List<Prop> props = new List<Prop>();

        public int Count => props.Count;

        // Adds a new instance to this group given a position (could be world coordinates or height map coordinates) and a type index.
        public void Add(Vector3 position, int type)
        {
            Prop p = new Prop();
            p.Position = position;
            p.Type = type;
            props.Add(p);
        }

        // This is the function that makes whatever is necessary to display a single prop in the unity scene.
        // Example would be Instantiating a prefab from a list or adding TreeInstances to a terrain.
        protected virtual void Instantiate(Prop prop)
        {
            //This needs to be implemented by the child classes.
        }

        // This tells the system how to clear/remove a specific prop 
        protected virtual void Clear(Prop prop)
        {
            //This is to be implemented by the child classes.
        }

        // This is just to call the Instantiate function on all the props.
        public virtual void InstantiateAll()
        {
            for (int i = 0; i < props.Count; i++)
            {
                Instantiate(props[i]);
            }
        }

        // This is just to call the Instantiate function on all the props. Or override to clear all in one go.
        public virtual void ClearAll()
        {
            for (int i = 0; i < props.Count; i++)
            {
                Clear(props[i]);
            }
        }
    }
}