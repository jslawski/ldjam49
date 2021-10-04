using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPosition;
    public GameObject gameCamera;
    public float parallaxStrength;

    // Start is called before the first frame update
    void Start()
    {
        this.startPosition = this.transform.position.x;
        this.length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMovedFromCamera = (this.gameCamera.transform.position.x * (1 - this.parallaxStrength));

        float distance = (this.gameCamera.transform.position.x * this.parallaxStrength);
        this.transform.position = new Vector3(this.startPosition + distance, this.transform.position.y, this.transform.position.z);

        if (distanceMovedFromCamera > (this.startPosition + this.length))
        {
            this.startPosition += this.length;
        }
        else if (distanceMovedFromCamera < (this.startPosition - this.length))
        {
            this.startPosition -= this.length;
        }
    }
}
