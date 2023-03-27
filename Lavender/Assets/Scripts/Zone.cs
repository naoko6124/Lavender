using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    GameObject zoneCamera;

    // Start is called before the first frame update
    void Start()
    {
        zoneCamera = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (Camera camera in Camera.allCameras)
            {
                if (camera.gameObject.activeInHierarchy)
                    camera.gameObject.SetActive(false);
            }
            zoneCamera.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
    }
}
