using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlanetManager : MonoBehaviour
{

    private static PlanetManager _instance;

    public static PlanetManager instance
    {
        get
        {
            return _instance;
        }
    }

    private List<Transform> _planetTransforms;

    public List<Transform> planetTransforms
    {
        get
        {
            if (_planetTransforms == null) _planetTransforms = new List<Transform>();
            return _planetTransforms;
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (!_instance)
            _instance = this;

        else Destroy(gameObject);

        _planetTransforms = new List<Transform>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Planet"))
        {
            _planetTransforms.Add(obj.transform);
        }
    }
}
