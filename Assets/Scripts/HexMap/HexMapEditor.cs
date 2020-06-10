using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    public HexGrid hexGrid;

    HexCell editingHex;
    private HexCell EditingHex
    {
        get => editingHex;
        set
        {
            if (editingHex)
            {
                editingHex.SetHighlightStatus(false, Color.white);
            }
            editingHex = value;
            if (editingHex)
            {
                editingHex.SetHighlightStatus(true, Color.yellow);
            }
        }
    }
    HexCell clickedHex;
    private HexCell ClickedHex
    {
        get => clickedHex;
        set
        {
            if (clickedHex && clickedHex != editingHex)
            {
                clickedHex.SetHighlightStatus(false, Color.white);
            }
            clickedHex = value;
            if (clickedHex)
            {
                clickedHex.SetHighlightStatus(true, Color.blue);
            }
        }
    }
    HexDirection selectedDirection = HexDirection.NE;

    public Text editingHexCoordinatesText;
    public Text clickedHexCoordinatesText;

    private void Start()
    {
        UpdateUI();
        hexGrid.ShowEditGrid(true);
    }

    public void UpdateUI()
    {
        editingHexCoordinatesText.text = editingHex ? editingHex.coordinates.ToString() : "No hex selected for editing";
        clickedHexCoordinatesText.text = clickedHex ? clickedHex.coordinates.ToString() : "No hex clicked";
    }

    private void Update()
    {
        //if (!EventSystem.current.CompareTag("UI"))
        //{
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
        //}
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UpdateClickedHex();
        }
    }

    void UpdateClickedHex()
    {
        HexCell cell = hexGrid.GetCell();
        if (cell != clickedHex)
        {
            ClickedHex = cell;
            UpdateUI();
        }
    }

    public void ChooseEditingHex()
    {
        if (clickedHex)
        {
            EditingHex = clickedHex;
            ClickedHex = null;
            UpdateUI();
        }
    }

    public void ChooseConnectionDirection(int newDir)
    {
        selectedDirection = (HexDirection)newDir;
    }

    public void ChangeTraversable(bool status)
    {
        if (editingHex)
        {
            editingHex.Traversable = status;
        }
    }

    public void OverrideConnection()
    {
        editingHex.SetNeighbor(selectedDirection, clickedHex);
    }

    public void ShowGameGrid(bool status)
    {
        hexGrid.ShowGameGrid(status);
    }

    public void ShowEditGrid(bool status)
    {
        hexGrid.ShowEditGrid(status);
    }

    public void ShowNeighborGizmos(bool status)
    {
        hexGrid.ShowNeighborGizmos(status);
    }
}
