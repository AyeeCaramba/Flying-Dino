using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlanetManager : MonoBehaviour
{

    public static PlanetManager instance { get; set; }

    public static List<Transform> planetTransforms;

    // Use this for initialization
    void Start()
    {
        planetTransforms = new List<Transform>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Planet")) {
            planetTransforms.Add(obj.transform);
        }
    }
}
