using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LandscapeGenerator
{

    public class PositionInitializer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ProceduralManager.Instance.world.generated.AddListener(Center);
            Center();
        }

        public void Center()
        {
            Terrain t = Terrain.activeTerrain;
            Vector3 center = t.GetPosition() + (t.terrainData.size / 2f);
            float height = t.SampleHeight(center);
            Vector3 pos = new Vector3(center.x, height + 2f, center.z);
            GetComponent<CharacterController>().enabled = false;
            GetComponent<CharacterController>().transform.position = pos;
            GetComponent<CharacterController>().enabled = true;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}