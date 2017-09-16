using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomInput
{
    public class BackgroundRaycaster : BaseRaycaster
    {
        [SerializeField]
        private GameObject _eventTarget;

        public GameObject eventTarget
        {
            get
            { 
                return _eventTarget ?? this.gameObject;
            }

            set
            {
                _eventTarget = value;
            }
        }

        public virtual int depth
        {
            get
            {
                return (eventCamera != null) ? (int)eventCamera.depth : 0xFFFFFF;
            }
        }

        #region implemented abstract members of BaseRaycaster

        public override void Raycast(PointerEventData eventData, System.Collections.Generic.List<RaycastResult> resultAppendList)
        {
            if (eventCamera == null)
            {
                return;
            }

            var result = new RaycastResult
                {
                    gameObject = eventTarget,
                    module = this,
                    distance = eventCamera.farClipPlane,
                    worldPosition = eventTarget.transform.position,
                    worldNormal = eventTarget.transform.forward,
                    screenPosition = eventData.position,
                    index = resultAppendList.Count,
                    sortingLayer = 0,
                    sortingOrder = 0
                };

            resultAppendList.Add(result);
        }

        private Camera _camera;
        public override Camera eventCamera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = GetComponent<Camera>();
                }

                return _camera;
            }
        }

        #endregion
    }
}
