using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CustomInput
{
    public interface IPointerPressedHandler : IEventSystemHandler
    {
        void OnPointerPress(PointerEventData pointerEventData);
    }

    public interface IPointerMovedHandler : IEventSystemHandler
    {
        void OnPointerMove(PointerEventData pointerEventData);
    }

    public interface IPointerUpInside : IEventSystemHandler
    {
        void OnPointerUpInside(PointerEventData pointerEventData);
    }

    public interface IPointerUpOutside : IEventSystemHandler
    {
        void OnPointerUpOutside(PointerEventData pointerEventData);
    }
}
