using DG.Tweening;
using MetroidPrimeDemo.Scripts.Gameplay.Level.Doors;
using MetroidPrimeDemo.Scripts.General;
using MetroidPrimeDemo.Scripts.Modules;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Level
{
    public class ExitCtrl : MonoBehaviour
    {
        [SerializeField] private TriggerListener trapTrigger;
        [SerializeField] private GuardedDoorCtrl guardedDoor;
        [SerializeField] private DestroyListener guardDestroyListener;
        [SerializeField] private float hidingDepth = 10.0f;
        [SerializeField] private Transform leftDoor;
        [SerializeField] private Transform rightDoor;
        [SerializeField] private float doorCrackDistance = 2.0f;
        [SerializeField] private TriggerListener exitTrigger;

        private bool trapTriggered;
        private bool trapSolved;

        private void Start()
        {
            exitTrigger.OnTriggerEnterEvent.AddListener(OnExitTriggerEnter);
            trapTrigger.OnTriggerEnterEvent.AddListener(OnTrapTriggerEnter);

            guardedDoor.transform.position -= guardedDoor.transform.up * hidingDepth;
            guardDestroyListener.OnDestroyEvent.AddListener(OnGuardDestroy);
        }

        private void OnDestroy()
        {
            exitTrigger.OnTriggerEnterEvent.AddListener(OnExitTriggerEnter);
            trapTrigger.OnTriggerEnterEvent.AddListener(OnTrapTriggerEnter);
            guardDestroyListener?.OnDestroyEvent.RemoveListener(OnGuardDestroy);
        }

        private void OnTrapTriggerEnter(Collider other)
        {
            if (trapTriggered) return;
            if (!other.gameObject.CompareTag("Player") || !other.GetComponent<PlayerCharacterCtrl>()) return;
            trapTriggered = true;
            guardedDoor.transform.position += guardedDoor.transform.up * hidingDepth;
            leftDoor.DOMove(leftDoor.position + leftDoor.right * (doorCrackDistance / 2.0f), 0.5f);
            rightDoor.DOMove(rightDoor.position - rightDoor.right * (doorCrackDistance / 2.0f), 0.5f);
        }

        private void OnGuardDestroy(GameObject guard)
        {
            if (trapSolved || !trapTriggered) return;
            trapSolved = true;
            guardDestroyListener.OnDestroyEvent.RemoveListener(OnGuardDestroy);
            guardDestroyListener = null;
            leftDoor.DOMove(leftDoor.position - leftDoor.right * (doorCrackDistance / 2.0f), 2.0f);
            rightDoor.DOMove(rightDoor.position + rightDoor.right * (doorCrackDistance / 2.0f), 2.0f);
        }

        private void OnExitTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerCharacterCtrl>();
                if (player)
                    GameFlow.FinishGame(player);
            }
        }
    }
}