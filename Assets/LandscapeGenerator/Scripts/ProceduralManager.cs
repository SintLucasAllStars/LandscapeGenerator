using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace LandscapeGenerator
{

    public class ProceduralManager : MonoBehaviour
    {
        public static ProceduralManager Instance;
        public bool debug = true;

        private int _seed = 0;
        private Vector2 perlinSeed;
        public ProceduralWorld world;
        public Vector2 PerlinSeed => perlinSeed;

        public UnityEvent regenerate = new UnityEvent();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            SetSeed(_seed);
        }

        private void Start()
        {
            world.Init();
        }

        private void Update()
        {
            //Debugging tools
            if (debug && Input.GetButtonDown("Fire1"))
            {
                ProceduralManager.Instance.regenerate.Invoke();
            }

            if (debug && Input.GetButtonDown("Fire2"))
            {
                SetSeed(_seed + 1);
            }
        }

        public void SetSeed(int seed)
        {
            this._seed = seed;
            UnityEngine.Random.InitState(seed);
            perlinSeed = new Vector2();
            perlinSeed.x = UnityEngine.Random.Range(-100000f, 100000f);
            perlinSeed.y = UnityEngine.Random.Range(-100000f, 100000f);
            regenerate.Invoke();
        }

        public void ResetSeed()
        {
            UnityEngine.Random.InitState(_seed);
        }
    }
}