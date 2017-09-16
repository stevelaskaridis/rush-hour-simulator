using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Collections;

namespace CustomInput
{
    public class PointerClickListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler
    {
        public class ClickEvent : UnityEvent<PointerEventData>{};

        private ClickEvent _onClick = new ClickEvent();
        public ClickEvent OnClick { get { return _onClick; } }

        protected DateTime _downTime;

        public bool RestrictClickDuration = true;
        public float MaxClickDuration = 200;

        public bool RestrictClickOnDrag = true;

        #region IPointerClickHandler implementation

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            eventData.Use();

            if (RestrictClickDuration)
            {
                var clickDuration = (DateTime.UtcNow - _downTime).TotalMilliseconds;
                if (clickDuration < MaxClickDuration)
                {
                    _onClick.Invoke(eventData);
                }
            }
            else
            {
                _onClick.Invoke(eventData);
            }
        }

        #endregion

        #region IPointerDownHandler implementation

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            _downTime = DateTime.UtcNow;
        }

        #endregion

        #region IBeginDragHandler implementation

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (RestrictClickOnDrag)
            {
                eventData.eligibleForClick = false;
            }

            //If we are not the dragging Target and have parents, foreward the IBeginDragHandler Event
            if (eventData.pointerDrag != this.gameObject && transform.parent != null)
            {
                ExecuteEvents.Execute<IBeginDragHandler>(transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
            }
        }

        #endregion
    }
}
