using UnityEngine;

/// <summary>
/// Simulates a realistic day-night cycle with independent sun and moon orbits.
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    public float dayDuration = 120f; // Full cycle duration in seconds (day + night)
    
    [Header("Sun & Moon Settings")]
    public Light sunLight;
    public Light moonLight;
    
    [Header("Skybox Settings")]
    public Material skyboxMaterial;
    public Color skyDayColor = new Color(0.5f, 0.7f, 1f); // Light blue day sky
    public Color skyNightColor = new Color(0.02f, 0.02f, 0.1f); // Dark night sky

    private float sunAngle = 0f;
    private float moonAngle = 180f;

    private void Update()
    {
        UpdateCelestialBodies();
        UpdateSkyboxColor();
    }

    /// <summary>
    /// Moves the sun and moon independently.
    /// </summary>
    private void UpdateCelestialBodies()
    {
        float deltaAngle = (Time.deltaTime / dayDuration) * 360f;

        // Rotate the Sun
        sunAngle += deltaAngle;
        if (sunAngle >= 360f) sunAngle -= 360f;
        sunLight.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f);

        // Rotate the Moon (independent orbit)
        moonAngle += deltaAngle * 0.8f; // Adjust this factor to desynchronize
        if (moonAngle >= 360f) moonAngle -= 360f;
        moonLight.transform.rotation = Quaternion.Euler(moonAngle - 90f, 170f, 0f);

        // Light intensity based on position above horizon
        sunLight.intensity = GetLightIntensity(sunAngle);
        moonLight.intensity = GetLightIntensity(moonAngle);

        sunLight.enabled = sunLight.intensity > 0.05f;
        moonLight.enabled = moonLight.intensity > 0.05f;
    }

    /// <summary>
    /// Returns the intensity of a light based on its height above the horizon.
    /// </summary>
    private float GetLightIntensity(float angle)
    {
        return Mathf.Clamp01(Mathf.InverseLerp(-10f, 50f, angle)) * 1.2f;
    }

    /// <summary>
    /// Smoothly blends the skybox color based on the positions of the sun and moon.
    /// </summary>
    private void UpdateSkyboxColor()
    {
        float sunFactor = Mathf.Clamp01(Mathf.InverseLerp(-20f, 60f, sunAngle));
        float moonFactor = Mathf.Clamp01(Mathf.InverseLerp(-20f, 60f, moonAngle));

        float skyBlend = Mathf.Max(sunFactor, moonFactor); // Take the strongest influence

        Color finalSkyColor = Color.Lerp(skyNightColor, skyDayColor, skyBlend);
        skyboxMaterial.SetColor("_Tint", finalSkyColor);

        // Update global ambient light for a smooth world transition
        RenderSettings.ambientLight = finalSkyColor;
    }
}
