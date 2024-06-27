using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Weapons
{
    public class TurretGunCtrl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem shooter;
        [SerializeField] private ParticleSystem muzzleFlash;
        public BeamParticleCtrl beam;

        public void Fire()
        {
            muzzleFlash.Play();
            shooter.Play();
        }
    }
}