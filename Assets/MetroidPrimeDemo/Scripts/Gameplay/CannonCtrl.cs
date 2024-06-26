using DG.Tweening;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class CannonCtrl : MonoBehaviour
    {
        public Transform missileSlot;

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

        public void Recoil(float punchDistanceScale = 1.0f)
        {
            transform.DOComplete();
            transform.DOPunchPosition(
                new Vector3(0, 0, -punchDistance * punchDistanceScale),
                punchDuration, punchVibrato, punchElasticity
            );
        }

        public void Fire()
        {
            muzzleFlash.Play();
            cannonParticleShooter.Play();
            Recoil();
        }

        public void StartCharging()
        {
            chargingParticle.Play();
            lineParticles.Play();
        }

        private void RecoverFromCharging()
        {
            lineParticles.Stop();
        }

        public void StopCharging(bool punchBack = true)
        {
            if (punchBack)
                RecoverFromCharging();
            chargingParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public void StartLoopingCharged()
        {
            chargedParticle.Play();
            chargedParticle.transform.DOScale(1, .4f).From(0).SetEase(Ease.OutBack);
            chargedEmission.Play();
        }

        public void StopLoopingCharged()
        {
            RecoverFromCharging();
            chargedParticle.transform.DOScale(0, .05f).OnComplete(() => chargedParticle.Clear());
            chargedParticle.Stop();
            chargedEmission.Stop();
        }

        public void FirePartiallyCharged()
        {
            chargedCannonParticle.Play();
            Recoil();
        }

        public void FireFullyCharged()
        {
            chargedCannonParticle.Play();
            Recoil();
        }
    }
}