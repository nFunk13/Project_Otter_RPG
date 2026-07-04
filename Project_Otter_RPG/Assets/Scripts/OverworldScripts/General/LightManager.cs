using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }

    public List<Light> lights;

    private void Awake()
    {
        // im singletoning it so hard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // called by ShadowInstance to find the strongest light at its position and face towards it
    public Light GetStrongestLightAt(Vector3 casterPos)
    {
        Light desiredLight = null;
        Vector3 casterToLight = Vector3.zero;
        float strongestLightIntensity = 0f;

        foreach (Light light in lights)
        {
            if(light.enabled)
            {
                casterToLight = light.transform.position - casterPos;
                if(casterToLight.sqrMagnitude > light.range * light.range) continue; // skip if out of range

                float lightIntensity = light.intensity / (casterToLight.sqrMagnitude + 0.0001f);
                if (lightIntensity > strongestLightIntensity)
                {
                    strongestLightIntensity = lightIntensity;
                    desiredLight = light;
                }
            }
        }

        return desiredLight;
    }
}
