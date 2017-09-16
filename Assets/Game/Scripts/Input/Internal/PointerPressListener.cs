using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Collections;

namespace CustomInput
{
    public class PointerPressListener : MonoBehaviour, IPointerPressedHandler, IPointerDownHandler, IPointerUpHandler
    {
        public class PressedEvent : UnityEvent<PointerEventData>{};

        private PressedEvent _onDownEvent = new PressedEvent();
        public PressedEvent OnDown { get { return _onDownEvent; } }

        #region IPointerDownHandler implementation

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDown.Invoke(eventData);
        }

        #endregion

        private PressedEvent _onPressEvent = new PressedEvent();
        public PressedEvent OnPress { get { return _onPressEvent; } }

        #region IPointerPressedHandler implementation

        public void OnPointerPress(PointerEventData eventData)
        {
            OnPress.Invoke(eventData);
        }

        #endregion

        private PressedEvent _onUpEvent = new PressedEvent();
        public PressedEvent OnUp { get { return _onUpEvent; } }

        #region IPointerUpHandler implementation

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp.Invoke(eventData);
        }

        #endregion

        private PressedEvent _onDropEvent = new PressedEvent();
        public PressedEvent OnDropEvent { get { return _onDropEvent; } }

        #region IDropHandler implementation

        public void OnDrop(PointerEventData eventData)
        {
            OnDropEvent.Invoke(eventData);
        }

        #endregion
        
    }
}
