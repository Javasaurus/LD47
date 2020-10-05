using System;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public GameObject[] PropPrefabs;
    public List<Transform> PropLocations;
    public int maxProps = 7;
    void Start()
    {
        int propCount = 0;
        PropLocations = PropLocations.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (Transform propLocation in PropLocations)
        {
            if (UnityEngine.Random.Range(0f, 1f) > .8f)
            {
                GameObject instance = GameObject.Instantiate(PropPrefabs[UnityEngine.Random.Range(0, PropPrefabs.Length)]);
                instance.transform.parent = propLocation;
                instance.transform.localPosition = Vector3.zero;

                propCount++;
                if (propCount >= maxProps)
                {
                    return;
                }
            }
        }
    }

}
