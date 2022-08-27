using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectableBehaviour : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject selection;
    private bool lastSelection = false;

    public void IsSelected(bool state)
    {
        lastSelection = state;
        selection.SetActive(state);
    }

    public void IsClicked()
    {
        lastSelection = !lastSelection;
        selection.SetActive(lastSelection);
    }

    void Start()
    {
        IsSelected(false);
    }
    

    
}
