using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class WeatherSystem : MonoBehaviour
{
    [Header("Global")]
    public Material globalMaterial;
    public Light sunLight;
    public Material skyboxMaterial;
    public TMP_Text weatherText;
    public float snowTransitionSpeed = 1f;

    private Coroutine snowFadeCoroutine;

    [Header("Winter Assets")]
    public ParticleSystem winterParticleSystem;
    public Volume winterVolume;

    [Header("Rain Assets")]
    public ParticleSystem rainParticleSystem;
    public Volume rainVolume;

    [Header("Autumn Assets")]
    public Volume autumnVolume;

    [Header("Summer Assets")]
    public Volume summerVolume;


    private void Awake() {
        globalMaterial.SetFloat("_SnowFade", 0);
    }

    private void Start()
    {
        Summer();
    }

    

    public void Winter()
    {
        DisableAllWeather();
        weatherText.text = "Winter";
        winterParticleSystem.Play();
        winterVolume.enabled = true;
        sunLight.intensity = 0.5f;
        SetSnowFade(1f);
    }

    public void Rain()
    {
        DisableAllWeather();
        weatherText.text = "Rain";
        rainParticleSystem.Play();
        rainVolume.enabled = true;
        sunLight.intensity = 0.6f;
        SetSnowFade(0f);
    }

    public void Autumn()
    {
        DisableAllWeather();
        weatherText.text = "Autumn";
        autumnVolume.enabled = true;
        sunLight.intensity = 0.7f;
        SetSnowFade(0f);
    }

    public void Summer()
    {
        DisableAllWeather();
        weatherText.text = "Summer";
        summerVolume.enabled = true;
        sunLight.intensity = 1.0f;
        SetSnowFade(0f);
    }

    private void DisableAllWeather() {
        winterParticleSystem.Stop();
        rainParticleSystem.Stop();

        winterVolume.enabled = false;
        rainVolume.enabled = false;
        autumnVolume.enabled = false;
        summerVolume.enabled = false;
    }

    public void SetSnowFade(float targetValue) {
        if (snowFadeCoroutine != null) {
            StopCoroutine(snowFadeCoroutine);
        }
        snowFadeCoroutine = StartCoroutine(SmoothSnowFade(targetValue));
    }

    private IEnumerator SmoothSnowFade(float targetValue) {
        float currentValue = globalMaterial.GetFloat("_SnowFade");
        while (Mathf.Abs(currentValue - targetValue) > 0.01f)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * snowTransitionSpeed);
            globalMaterial.SetFloat("_SnowFade", currentValue);
            yield return null;
        }
        globalMaterial.SetFloat("_SnowFade", targetValue);
    }
}