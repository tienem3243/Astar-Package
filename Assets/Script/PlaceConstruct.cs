using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceConstruct : MonoBehaviour
{
    [SerializeField] private Transform holder;
    [SerializeField] private GameObject constructIcon;
    [SerializeField] private Transform toolTrans;
    [SerializeField] private ConstructButton constructButton;
    [SerializeField] DatabaseObjectSO subList;
    bool isDraging;
    Construct currentDrag;
    DatabaseObjectSO.ObjectData currentData;
    Dictionary<Vector2Int, Construct> holderDictionary= new Dictionary<Vector2Int, Construct>();
 
    private void LoadConstruct()
    {

         
        int i = 0;
 
        foreach (DatabaseObjectSO.ObjectData obj in subList.ObjectsData)
        {
            ConstructButton button = Instantiate(constructButton, toolTrans) ;
            currentData = obj;
            button.Image.sprite = obj.Icon;
            
            button.Id = i;
            i++;
            button.OnClickEvent.AddListener((x) => SelectConstruct(obj.Construct));
        }
    }
    public void SelectConstruct(Construct construct)
    {
        currentDrag = Instantiate(construct, holder);
        
        isDraging = true;
    }
    private void Update()
    {
        if (isDraging)
        {
            Vector2 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GridMap.Instance.GetXY(dragPos, out int x, out int y);
           
            currentDrag.transform.position = (Vector2)GridMap.Instance.GetWorldPosition(x,y);
           
        }
        if (Input.GetMouseButtonDown(0)&&isDraging)
        {
           
            Vector2 dragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (IsPlaceAble(dragPos, new Vector2Int(currentData.Size.x, currentData.Size.y)))
            {

               
                SetPlace(dragPos,currentDrag);
            }
        }
        if(Input.GetMouseButtonDown(1) && isDraging)
        {
            Destroy(currentDrag.gameObject);
            isDraging = false;
            currentData = null;
            currentDrag = null;
        }
    }

    private void SetPlace(Vector2 position,Construct owner)
    {
        Instantiate(currentDrag, holder);

        GridMap.Instance.GetXY(position, out int x, out int y);
        for (int i = x; i < x + currentData.Size.x; i++)
            for (int j = y; j < y + currentData.Size.y; j++)
            {
                GridMap.Instance.GetValue(i, j).IsWalkable = false;
                holderDictionary.Add(new Vector2Int(i,j),owner);
            }
    }
    private bool IsPlaceAble(Vector2 position,Vector2Int checkZone)
    {
        GridMap.Instance.GetXY(position, out int x, out int y);
        for (int i = x; i < x + checkZone.x; i++)
            for (int j = y; j < y + checkZone.y; j++)
            {   if (GridMap.Instance.GetValue(i, j) == null) return false;
                if (!GridMap.Instance.GetValue(i, j).IsWalkable) return false;
            }
        return true;
    }

    private void Start()
    {
        LoadConstruct();
    }
}
