using UnityEngine;

public class ShadowInstance : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        if (LightManager.Instance != null)
        {
            Light strongestLight = LightManager.Instance.GetStrongestLightAt(transform.position);
            if (strongestLight != null)
            {
                Vector3 directionToLight = (strongestLight.transform.position - transform.position).normalized;
                directionToLight.y = 0;

                if (directionToLight.sqrMagnitude < 0.0001f) return;
                Quaternion targetRot = Quaternion.LookRotation(directionToLight);

                transform.rotation = targetRot;
            }
        }
    }
}
