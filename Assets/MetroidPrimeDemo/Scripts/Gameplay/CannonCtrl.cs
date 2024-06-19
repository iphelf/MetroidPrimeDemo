using DG.Tweening;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class CannonCtrl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem cannonParticleShooter;
        [SerializeField] private ParticleSystem chargingParticle;
        [SerializeField] private ParticleSystem chargedParticle;
        [SerializeField] private ParticleSystem lineParticles;
        [SerializeField] private ParticleSystem chargedCannonParticle;
        [SerializeField] private ParticleSystem chargedEmission;
        [SerializeField] private ParticleSystem muzzleFlash;

        [SerializeField] private float punchDistance = .2f;
        [SerializeField] private int punchVibrato = 5;
        [SerializeField] private float punchDuration = .3f;
        [SerializeField, Range(0, 1)] private float punchElasticity = .5f;

        public void Fire()
        {
            muzzleFlash.Play();

            transform.DOComplete();
            transform.DOPunchPosition(
                new Vector3(0, 0, -punchDistance),
                punchDuration, punchVibrato, punchElasticity
            );

            cannonParticleShooter.Play();
        }
    }
}