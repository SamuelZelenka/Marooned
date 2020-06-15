using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    [Header("References")]
    public HexGrid hexGrid;
    public Text editingHexCoordinatesText;
    public Text clickedHexCoordinatesText;
    public Text newMapSize;
    public SpriteRenderer playerShip;
    public SpriteRenderer enemyShip;
    public SpriteRenderer landRight;
    public SpriteRenderer fullLand;

    public enum EditorVisuals { ShipToShip, ShipToLand, LandOnly }
    [Header("Graphics")]
    public EditorVisuals editorVisuals;
    public Sprite playerShipSprite;
    public Sprite enemyShipSprite;
    public Sprite landRightSprite;
    public Sprite fullLandSprite;

    int xSize;
    int ySize;

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

    private void Start()
    {
        UpdateUI();
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

    public void CreateNewMap()
    {
        hexGrid.CreateMap(xSize, ySize, true);
    }

    public void SetXSizeByString(string stringInput)
    {
        if (int.TryParse(stringInput, out int number))
        {
            xSize = number;
            newMapSize.text = "X = " + xSize + "\nY = " + ySize;
        }
    }

    public void SetYSizeByString(string stringInput)
    {
        if (int.TryParse(stringInput, out int number))
        {
            ySize = number;
            newMapSize.text = "X = " + xSize + "\nY = " + ySize;
        }
    }

    public void UpdateSprites()
    {
        playerShip.enabled = false;
        enemyShip.enabled = false;
        landRight.enabled = false;
        fullLand.enabled = false;
        playerShip.sprite = null;
        enemyShip.sprite = null;
        landRight.sprite = null;
        fullLand.sprite = null;

        switch (editorVisuals)
        {
            case EditorVisuals.ShipToShip:
                playerShip.sprite = playerShipSprite;
                enemyShip.sprite = enemyShipSprite;
                playerShip.enabled = true;
                enemyShip.enabled = true;
                break;
            case EditorVisuals.ShipToLand:
                playerShip.sprite = playerShipSprite;
                landRight.sprite = landRightSprite;
                playerShip.enabled = true;
                landRight.enabled = true;
                break;
            case EditorVisuals.LandOnly:
                fullLand.sprite = fullLandSprite;
                fullLand.enabled = true;
                break;
        }
    }
}
