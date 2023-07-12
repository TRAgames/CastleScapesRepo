using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water2DSpawner : MonoBehaviour
{

    public GameObject DropObject;
    public int sizeOfLiquid;
    GameObject[] WaterDropsObjects;
    public float size = .45f;
    public Vector2 initSpeed = new Vector2(1f, -1.8f);
    // Start is called before the first frame update
    void Start()
    {
        WaterDropsObjects = new GameObject[sizeOfLiquid];
        for (int i = 0; i < WaterDropsObjects.Length; i++)
        {
            Vector3 pos = new Vector3(gameObject.transform.position.x + Random.Range(-0.25f, 0.25f),
                   gameObject.transform.position.y + Random.Range(-0.25f, 0.25f), 0.0f);
            WaterDropsObjects[i] = Instantiate(DropObject, pos, new Quaternion(0, 0, 0, 0)) as GameObject;
            //WaterDropsObjects[i].GetComponent<MetaballParticleClass>().Active = false;
            WaterDropsObjects[i].transform.localScale = new Vector3(size, size, 1f);
            WaterDropsObjects[i].layer = WaterDropsObjects[0].layer;
            WaterDropsObjects[i].GetComponent<Rigidbody2D>().velocity = initSpeed;
            //LevelPool._instance.AddObject(WaterDropsObjects[i]);

        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
