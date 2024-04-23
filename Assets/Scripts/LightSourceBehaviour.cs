using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightSourceBehaviour : MonoBehaviour
{
    public Light sphereLight; // Ljuset som är associerat med den lysande sfären
    public float minSize = 1f; // Minsta storlek på sfären
    public float maxSize = 5f; // Största storlek på sfären
    public float minIntensity = 1f; // Minsta intensitet på ljuset
    public float maxIntensity = 5f; // Största intensitet på ljuset
    public float duration = 5f; // Total tid för övergången (för både storlek och intensitet)

    private float timer = 0f; // Timer för att spåra övergången

    private Volume volume; // Post Processing Volym som innehåller Bloom-inställningar
    private Bloom bloom; // Bloom-inställningar

    // private PlayerPerformance playerPerformance; // Referens till spelarens prestandahantering

    void Start()
    {
        // Hämta den associerade post processing volymen och Bloom-inställningarna
        volume = GetComponent<Volume>();
       // volume.profile.TryGet(out bloom);

        // Hämta referensen till spelarens prestandahantering
        // playerPerformance = FindObjectOfType<PlayerPerformance>();
    }

    void Update()
    {
        // Uppdatera övergången
        timer += Time.deltaTime;

        // Beräkna progressen av övergången
        float progress = Mathf.Clamp01(timer / duration);

        // Beräkna storlek och intensitet baserat på progressen och spelarens prestanda
       // float size = Mathf.Lerp(minSize, maxSize, progress) * playerPerformance.GetPerformance();
       // float intensity = Mathf.Lerp(minIntensity, maxIntensity, progress) * playerPerformance.GetPerformance();

        // Uppdatera storleken på sfären och intensiteten på ljuset
       // sphereLight.range = size;
       // sphereLight.intensity = intensity;

        // Uppdatera Bloom-inställningarna för intensitet baserat på intensiteten och spelarens prestanda
       // bloom.intensity.value = intensity;

        // Återställ timer när övergången är klar
        if (timer >= duration)
        {
            timer = 0f;
        }
    }
}
