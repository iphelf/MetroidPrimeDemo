using System.Collections.Generic;
using MetroidPrimeDemo.Scripts.UI;
using MetroidPrimeDemo.Scripts.UI.Menu;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Scenes
{
    public class SampleUISceneCtrl : MonoBehaviour
    {
        [SerializeField] private MenuViewCtrl menu;

        private void Start()
        {
            var entryList1 = new List<MenuEntry>
            {
                new MenuInfoEntry { EntryName = "An info" },
                new MenuButtonEntry { EntryName = "EntryList2" },
                new MenuSliderEntry { EntryName = "A slider" },
            };
            var entryList2 = new List<MenuEntry>
            {
                new MenuButtonEntry { EntryName = "Back" },
            };
            ((MenuButtonEntry)entryList1[1]).Callback = () =>
                menu.FillEntries("EntryList2", entryList2);
            ((MenuButtonEntry)entryList2[0]).Callback = () =>
                menu.FillEntries("Sample UI", entryList1);
            menu.FillEntries("Sample UI", entryList1);
        }
    }
}