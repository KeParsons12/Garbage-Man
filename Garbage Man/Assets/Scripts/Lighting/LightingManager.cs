using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _preset;

    [SerializeField, Range(0, 24)] private float _timeOfDay;

    private void Update()
    {
        if(_preset == null)
        {
            return;
        }

        if(Application.isPlaying)
        {
            _timeOfDay += Time.deltaTime;
            _timeOfDay %= 24; //clamp between 0-24
            UpdateLighting(_timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(_timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = _preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _preset.fogColor.Evaluate(timePercent);

        if(_directionalLight != null)
        {
            _directionalLight.color = _preset.directionalColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0f));
        }
    }

    private void OnValidate()
    {
        if(_directionalLight != null)
        {
            return;
        }

        if(RenderSettings.sun != null)
        {
            _directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    _directionalLight = light;
                    return;
                }
            }
        }
    }
}
