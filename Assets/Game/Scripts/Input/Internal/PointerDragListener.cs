using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace CustomInput
{
    public class PointerDragListener : Selectable, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public class DragEvent : UnityEvent<Vector2, PointerEventData>{};

        [SerializeField]
        private DragEvent _onDragging = new DragEvent();
        public DragEvent OnDragging { get{ return _onDragging; } }

        [SerializeField]
        private DragEvent _onDragEnd = new DragEvent();
        public DragEvent OnDragEnd { get { return _onDragEnd; }}

        private Vector2 _startPosition;

        #region IBeginDragHandler implementation

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = eventData.position;
        }

        #endregion

        #region IDragHandler implementation

        public void OnDrag(PointerEventData eventData)
        {
            OnDragging.Invoke(eventData.position - _startPosition, eventData);
        }

        #endregion

        #region IEndDragHandler implementation

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragging.Invoke(eventData.position - _startPosition, eventData);

            OnDragEnd.Invoke(eventData.position - _startPosition, eventData);
        }

        #endregion
    }
}
