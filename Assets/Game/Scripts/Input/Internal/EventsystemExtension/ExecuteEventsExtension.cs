using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CustomInput
{
    public class ExecuteEventsExtension
    {
        private static readonly ExecuteEvents.EventFunction<IPointerPressedHandler> s_PointerPressedHandler = Execute;

        private static void Execute(IPointerPressedHandler handler, BaseEventData eventData)
        {
            handler.OnPointerPress(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
        }

        public static ExecuteEvents.EventFunction<IPointerPressedHandler> pointerPressedHandler
        {
            get { return s_PointerPressedHandler; }
        }
    }
}
