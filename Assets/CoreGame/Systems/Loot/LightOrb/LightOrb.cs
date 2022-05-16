using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : Pickup
{

    [Header("Light orb effects")]
    [SerializeField] private Light lightOrbPointLight;
    [SerializeField] private List<ParticleSystem> lightOrbDefaultEffects;
    [SerializeField] private ParticleSystem pickedEffect;
    [SerializeField] private MeshRenderer orbMesh;

    [SerializeField] private int healedAmount;
    private Material orbMaterial;
    private float originalOrbAlpha;

    private List<Material> effectsMaterials;

    private float originalLightIntensity;

    private void Awake()
    {
        originalLightIntensity = lightOrbPointLight.intensity;

        orbMaterial = orbMesh.material;
        originalOrbAlpha = orbMaterial.color.a;

        effectsMaterials = new List<Material>();
        for (int i = 0; i < lightOrbDefaultEffects.Count; ++i)
        {
            ParticleSystemRenderer particleRenderer = lightOrbDefaultEffects[i].GetComponent<ParticleSystemRenderer>();
            effectsMaterials.Add(particleRenderer.material);

            if (particleRenderer.trailMaterial != null)
            {
                effectsMaterials.Add(particleRenderer.trailMaterial);
            }

        }
    }

    protected override void ApplyEffect()
    {
        GameManager.instance.player.GetControlledCharacter().characterStatsManager.Heal(healedAmount);
    }

    protected override void ResetEffects()
    {
        lightOrbPointLight.intensity = originalLightIntensity;
        orbMaterial.SetAlpha(originalOrbAlpha);

        for (int i = 0; i < effectsMaterials.Count; ++i)
        {
            effectsMaterials[i].SetAlpha(1f);
        }
    }

    protected override void EnablePickedEffects()
    {
        pickedEffect.Play();
    }

    protected override void UpdatePickedEffects()
    {
        float progress = Mathf.Min(currentTime / beingPickedTime, 1f);

        orbMaterial.SetAlpha(originalOrbAlpha * (1f - progress));
        for (int i = 0; i < effectsMaterials.Count; ++i)
        {
            effectsMaterials[i].SetAlpha(1f - progress);
        }


        lightOrbPointLight.intensity = 1f - progress;
    }

    protected override void UpdateDespawningEffects()
    {
        float progress = Mathf.Min(currentTime / despawningTime, 1f);

        orbMaterial.SetAlpha(originalOrbAlpha * (1f - progress));
        for (int i = 0; i < effectsMaterials.Count; ++i)
        {
            effectsMaterials[i].SetAlpha(1f - progress);
        }


        lightOrbPointLight.intensity = 1f - progress;
    }
}
