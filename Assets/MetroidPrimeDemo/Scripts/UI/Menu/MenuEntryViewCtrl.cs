using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MetroidPrimeDemo.Scripts.UI.Menu
{
    public abstract class MenuEntryViewCtrl : MonoBehaviour
    {
        protected TViewCtrl Clone<TViewCtrl>(Transform parent)
        {
            return Instantiate(gameObject, parent).GetComponent<TViewCtrl>();
        }

        public abstract MenuEntryViewCtrl Clone(Transform parent);
        public abstract void Fill(MenuEntry entry);
        protected abstract Selectable GetSelectable();

        public void Focus() => EventSystem.current.SetSelectedGameObject(GetSelectable().gameObject);

        public void LinkNavigation(MenuEntryViewCtrl up, MenuEntryViewCtrl down)
        {
            GetSelectable().navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                wrapAround = false,
                selectOnUp = up?.GetSelectable(),
                selectOnDown = down?.GetSelectable(),
                selectOnLeft = null,
                selectOnRight = null,
            };
        }
    }
}