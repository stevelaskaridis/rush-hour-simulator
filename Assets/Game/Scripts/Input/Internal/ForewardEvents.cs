using UnityEngine;
using UnityEngine.EventSystems;

public class ForewardEvents : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, ISubmitHandler,
ICancelHandler, ISelectHandler, IDeselectHandler, IMoveHandler, IScrollHandler, IUpdateSelectedHandler,
IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IEventSystemHandler, IDropHandler
{
    [SerializeField]
    private GameObject _forwardTarget;

    public GameObject ForwardTarget
    {
        get
        {
            return _forwardTarget;
        }
        set
        {
            _forwardTarget = value;
        }
    }

    #region IInitializePotentialDragHandler implementation
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IInitializePotentialDragHandler>(_forwardTarget, eventData, ExecuteEvents.initializePotentialDrag);
    }
    #endregion
    
    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IBeginDragHandler>(_forwardTarget, eventData, ExecuteEvents.beginDragHandler);
    }
    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IDragHandler>(_forwardTarget, eventData, ExecuteEvents.dragHandler);
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IEndDragHandler>(_forwardTarget, eventData, ExecuteEvents.endDragHandler);
    }

    #endregion

    #region ISubmitHandler implementation

    public void OnSubmit(BaseEventData eventData)
    {
        ExecuteEvents.Execute<ISubmitHandler>(_forwardTarget, eventData, ExecuteEvents.submitHandler);
    }

    #endregion

    #region ICancelHandler implementation

    public void OnCancel(BaseEventData eventData)
    {
        ExecuteEvents.Execute<ICancelHandler>(_forwardTarget, eventData, ExecuteEvents.cancelHandler);
    }

    #endregion

    #region ISelectHandler implementation

    public void OnSelect(BaseEventData eventData)
    {
        ExecuteEvents.Execute<ISelectHandler>(_forwardTarget, eventData, ExecuteEvents.selectHandler);
    }

    #endregion

    #region IDeselectHandler implementation

    public void OnDeselect(BaseEventData eventData)
    {
        ExecuteEvents.Execute<IDeselectHandler>(_forwardTarget, eventData, ExecuteEvents.deselectHandler);
    }

    #endregion

    #region IMoveHandler implementation

    public void OnMove(AxisEventData eventData)
    {
        ExecuteEvents.Execute<IMoveHandler>(_forwardTarget, eventData, ExecuteEvents.moveHandler);
    }

    #endregion

    #region IScrollHandler implementation

    public void OnScroll(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IScrollHandler>(_forwardTarget, eventData, ExecuteEvents.scrollHandler);
    }

    #endregion

    #region IUpdateSelectedHandler implementation

    public void OnUpdateSelected(BaseEventData eventData)
    {
        ExecuteEvents.Execute<IUpdateSelectedHandler>(_forwardTarget, eventData, ExecuteEvents.updateSelectedHandler);
    }

    #endregion

    #region IPointerEnterHandler implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerEnterHandler>(_forwardTarget, eventData, ExecuteEvents.pointerEnterHandler);
    }

    #endregion

    #region IPointerExitHandler implementation

    public void OnPointerExit(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerExitHandler>(_forwardTarget, eventData, ExecuteEvents.pointerExitHandler);
    }

    #endregion

    #region IPointerDownHandler implementation

    public void OnPointerDown(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerDownHandler>(_forwardTarget, eventData, ExecuteEvents.pointerDownHandler);
    }

    #endregion

    #region IPointerUpHandler implementation

    public void OnPointerUp(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerUpHandler>(_forwardTarget, eventData, ExecuteEvents.pointerUpHandler);
    }

    #endregion

    #region IPointerClickHandler implementation

    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerClickHandler>(_forwardTarget, eventData, ExecuteEvents.pointerClickHandler);
    }

    #endregion

    #region IDropHandler implementation

    public void OnDrop(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IDropHandler>(_forwardTarget, eventData, ExecuteEvents.dropHandler);
    }

    #endregion
}
