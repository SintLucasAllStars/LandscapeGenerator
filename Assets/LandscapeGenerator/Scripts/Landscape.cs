using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LandscapeGenerator
{

    public abstract class Landscape : MonoBehaviour
    {


        private void Start()
        {
            Init();
        }

        protected void Init()
        {
            ProceduralManager.Instance.world.generated.AddListener(Generate);
        }



        public virtual void Clean()
        {
        }

        public virtual void Generate()
        {
            Debug.Log("This should not display EVER");
        }
    }
}