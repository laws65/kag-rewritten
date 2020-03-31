using UnityEngine;
using UnityEngine.EventSystems;

namespace KAG.Misc
{
    public class UIDraggable : MonoBehaviour, IDragHandler
    {
        RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (rectTransform)
            {
                rectTransform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
            }
        }
    }
}
