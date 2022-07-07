using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scritables/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject
{
    [Tooltip("")] public Gradient ambientColor;
    [Tooltip("")] public Gradient directionalColor;
    [Tooltip("")] public Gradient fogColor;
}
