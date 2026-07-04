using UnityEngine;

[RequireComponent(typeof(Light))]
public class ShadowCaster : MonoBehaviour
{
    private void Start()
    {
        LightManager.Instance.lights.Add(GetComponent<Light>());
    }

    private void OnDestroy()
    {
        if(LightManager.Instance != null)
        {
            LightManager.Instance.lights.Remove(GetComponent<Light>());
        }   
    }
}
