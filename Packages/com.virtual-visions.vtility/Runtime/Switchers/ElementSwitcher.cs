using System;
using JetBrains.Annotations;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public enum ElementSwitcher_Values
    {
        Active,
        ElementList,
        ElementChange,
        // ---
        Count
    }

    public abstract class ElementSwitcher : DataList
    {
        /// Since Udon doesn't natively support fields of this type,
        /// it is recommended to use this format for your fields:
        ///
        ///    public ElementSwitcher pageSwitcher => _pageSwitcher.AsElementSwitcher();
        ///    private DataList _pageSwitcher;
        ///      -----
        ///    _pageSwitcher = ElementSwitcher.Create(_pages, _currentPage);
        ///

        public static ElementSwitcher Create(CanvasElement[] elementList, CanvasElement initialValue = default)
        {
            DataToken[] values = new DataToken[(int)ElementSwitcher_Values.Count];

            foreach (CanvasElement element in elementList)
            {
                if (!element) continue;
                element._HideElement(true);
            }

            Debug.Log($"Created ElementSwitcher with default value of {initialValue}", initialValue);

            values[(int)ElementSwitcher_Values.Active] = initialValue;
            values[(int)ElementSwitcher_Values.ElementList] = elementList.ToRefList();
            values[(int)ElementSwitcher_Values.ElementChange] = ValueChangeCallback.Create(initialValue);

            DataList objectSwitcher = new DataList(values);
            return (ElementSwitcher)objectSwitcher;
        }
    }

    public static class ElementSwitcherExtensions
    {

        public static ElementSwitcher AsElementSwitcher(this DataToken token) => (ElementSwitcher)token.DataList;
        public static ElementSwitcher AsElementSwitcher(this DataList list) => (ElementSwitcher)list;

        // ---

        public static DataList ElementList(this ElementSwitcher switcher) => switcher[(int)ElementSwitcher_Values.ElementList].DataList;
        public static CanvasElement ActiveElement(this ElementSwitcher switcher) => switcher.ElementChange().Value().CastReference<CanvasElement>();
        public static ValueChangeCallback ElementChange(this ElementSwitcher switcher) => switcher[(int)ElementSwitcher_Values.ElementChange].AsValueChangeCallback();


        /// <summary>
        /// Passes the new value that is switched to.
        /// </summary>
        public static UdonAction OnElementSwitched(this ElementSwitcher switcher) =>
            switcher.ElementChange().OnChanged();

        [PublicAPI]
        public static void AddElement(this ElementSwitcher switcher, CanvasElement element)
        {
            switcher.ElementList().Add(element);
            element._HideElement(true);
        }

        [PublicAPI]
        public static void SwitchTo(this ElementSwitcher switcher, CanvasElement element)
        {
            DataList list = switcher.ElementList();
            if (!list.Contains(element)) switcher.AddElement(element);
            switcher.SwitchToIndex(list.IndexOf(element));
        }

        [PublicAPI]
        public static void SwitchToIndex(this ElementSwitcher switcher, Enum index) => switcher.SwitchToIndex(Convert.ToInt32(index));

        [PublicAPI]
        public static void SwitchToIndex(this ElementSwitcher switcher, int index)
        {
            switcher.ElementList().TryIndex(index, out DataToken value);

            // Sometimes, the CastReference value returns as the self inconsistently when both Null and the same parent type.
            // I don't even want to understand, but just making sure it's not in the page list is safe enough for us.
            CanvasElement newPage = value.CastReference<CanvasElement>();
            if (!switcher.ElementList().Contains(newPage)) newPage = null;

            CanvasElement currentPage = switcher.ActiveElement();
            if (!switcher.ElementList().Contains(currentPage)) currentPage = null;


            if (currentPage) currentPage._HideElement();
            currentPage = newPage;
            if (currentPage) currentPage._ShowElement();

            switcher.ElementChange().SetValue(newPage);
        }
    }
}