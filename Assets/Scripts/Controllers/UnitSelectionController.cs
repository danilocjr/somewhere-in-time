using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionController : MonoBehaviour
{

    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private LayerMask groundLayer;

    private float mouseDragLimit = 40f;
    private Vector3 startMousePoint;
    private bool isDragging = false;

    RaycastHit hit;

    private void OnGUI()
    {
        if (isDragging)
        {
            MouseScreenDrawer.DrawFromScreenPoints(startMousePoint, Input.mousePosition);
        }
    }
  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePoint = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if ((startMousePoint - Input.mousePosition).magnitude > mouseDragLimit)
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // LEFT SHIFT press add unit to selection without removing previous selection
            if (!Input.GetKey(KeyCode.LeftShift) && selectedUnits.Count > 0)
                Deselected();

            if (!isDragging)
            {
                // Handle Single Click on a Unit
                Ray ray = Camera.main.ScreenPointToRay(startMousePoint);

                if (Physics.Raycast(ray, out hit, 5000f, selectableLayer))
                {
                    AddSelected(hit.collider.gameObject);
                }
            }
            else
            {
                isDragging = false;
                var box = SelectionBoundingBox.TriggerSelectedObjects(this.gameObject, groundLayer, startMousePoint, Input.mousePosition);
                Destroy(box, 0.02f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AddSelected(other.gameObject);
    }


    public Dictionary<int, GameObject> selectedUnits = new Dictionary<int, GameObject>();

    public void AddSelected(GameObject go)
    {
        int id = go.GetInstanceID();
        if (!(selectedUnits.ContainsKey(id)))
        {
            selectedUnits.Add(id, go);
            go.GetComponent<ISelectable>().IsSelected(true);
        }
    }

    public void Deselected(int id)
    {
        selectedUnits[id].GetComponent<ISelectable>().IsSelected(false);
        selectedUnits.Remove(id);
    }

    public void Deselected()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedUnits)
        {
            if (pair.Value != null)
                pair.Value.GetComponent<ISelectable>().IsSelected(false);
        }

        selectedUnits.Clear();
    }


}
