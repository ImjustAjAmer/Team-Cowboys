using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ColorTransition : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color[] srColor;
    private int index;
    public float speed;
    private float colorChanger;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sr.material.color = Color.Lerp(sr.material.color, srColor[index], speed * Time.deltaTime);

        colorChanger = Mathf.Lerp(colorChanger, 1f, speed * Time.deltaTime);

        if(colorChanger > 0.9f)
        {
            colorChanger = 0;
            index++;

            index = (index > srColor.Length) ? 0 : index;
        }
    }

}