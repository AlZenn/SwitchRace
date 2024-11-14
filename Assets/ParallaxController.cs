using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxController : MonoBehaviour
{
    private float startPost,length;
    public GameObject cam;
    public float parallaxEffect;

    private void Awake()
    {
        cam = GameObject.Find("Main Camera");
    }

    private void Start()
    {
        startPost = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect; // 0= kamerayla hareket || 1= hareket etmiyecek || 0.5= yarım hareket
        float movement = cam.transform.position.x * (1 - parallaxEffect);
        transform.position = new Vector3(startPost + distance, transform.position.y, transform.position.z);

        if (movement>startPost+length)
        {
            startPost += length;
        }
        else if (movement<startPost-length)
        {
            startPost -= length;
        }
    }
}
