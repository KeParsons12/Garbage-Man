using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStats : MonoBehaviour
{
    [SerializeField] [Tooltip("The amount of points this object is worth when being picked up")] private int _pointValue;
    public int PointValue { get { return _pointValue; } }

    private bool _hasTrash = true;
    public bool HasTrash { get { return _hasTrash; } set { _hasTrash = value; } }

    private void Update()
    {
        if(!_hasTrash)
        {
            var renderer = this.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.red);
        }
    }
}
