using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIFImplementation : MonoBehaviour {

    [SerializeField] private Texture2D[] frames;
    [SerializeField] private  float fps = 10.0f;

    private Material mat;

    void Start () {
        mat = GetComponent<Renderer> ().material;
    }

    void Update () {
        // The index of the material array changes corresponding to the in-game time, multiplied in speed by the fps value (10 by default)
        int index = (int)(Time.time * fps);
        index = index % frames.Length;
        mat.mainTexture = frames[index];
    }
}