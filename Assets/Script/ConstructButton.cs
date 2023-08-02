using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConstructButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private Image image;
    private int id;
    UnityEvent<int> onClickEvent;
    public Image Image { get => image; set => image = value; }
    public UnityEvent<int> OnClickEvent { get => onClickEvent; set => onClickEvent = value; }
    public int Id { get => id; set => id = value; }

    private void Awake()
    {
        if (OnClickEvent == null) OnClickEvent = new UnityEvent<int>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent.Invoke(Id);
    }
}
