using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using CMythos;

[CustomEditor(typeof(GameBoard))]
public class GameBoardEditor2 : Editor
{
    const int MAX_TILES = 100;


    private SerializedProperty sizeProperty;
    private Vector3Int Size
    {
        get => sizeProperty.vector3IntValue;
        set => sizeProperty.vector3IntValue = value;
    }

    private Vector3Int selectedTile;


    private int currentDepth;


    private Vector2Int tilemapTileSize;


    private VisualElement rootElement;

    private VisualTreeAsset visualTree;

    private GameBoard gameBoard;

    public int Width
    {
        get => Size.x;
    }
    public int Length
    {
        get => Size.z;
    }
    public int Height
    {
        get => Size.y;
    }

    public int SelectedX
    {
        get => selectedTile.x;
    }
    public int SelectedZ
    {
        get => selectedTile.z;
    }
    public int SelectedY
    {
        get => selectedTile.y;
    }


    private Image gridField;
    private Vector3IntField gridSizeField;

    private Image pathSelector;

    private ObjectField effectSelector;


    private ObjectField renderSelector;

    private ObjectField defaultRenderSelector;

    private Button nextLayerButton;

    //Textures
    private Texture2D tilemap;
    private Texture2D[] layerTextures;

    private Texture2D tileSelectionTexture;
    private bool isDown = false;
    private bool isRefreshingTileProperties = false;
    private int currentPathType;
    private void OnEnable()
    {
        gameBoard = (GameBoard)target;
        sizeProperty = serializedObject.FindProperty("size");
        currentPathType = (int)GameBoardPathType.BLOCKED;
        rootElement = new VisualElement();
        visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/GameBoardTemplate2.uxml");
        rootElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/GameBoardStyleSheet2.uss"));
        tilemap = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Scripts/Editor/boardmakertilemap.png");
        tilemapTileSize = new Vector2Int(tilemap.width / 8, tilemap.height / 4);
        tileSelectionTexture = new Texture2D(tilemapTileSize.x * 4, tilemapTileSize.y * 4, tilemap.format, false);
        if (selectedTile == null)
        {
            selectedTile = new Vector3Int(0, 0, 0);
        }
        currentDepth = selectedTile.y;

    }
    private void OnDisable()
    {
        serializedObject.ApplyModifiedProperties();
    }
    private void OnDestroy()
    {
        serializedObject.ApplyModifiedProperties();
    }
    public override VisualElement CreateInspectorGUI()
    {
        
        rootElement.Clear();
        visualTree.CloneTree(rootElement);
        gridField = rootElement.Query<Image>("grid").First();
        //Configure Grid
        gridField.RegisterCallback<MouseDownEvent>(evt =>
        {
            isDown = true;
            SelectTile(evt.localMousePosition.x, evt.localMousePosition.y, evt.ctrlKey);
        });
        gridField.RegisterCallback<MouseUpEvent>(evt =>
        {

            isDown = false;
        });
        gridField.RegisterCallback<MouseMoveEvent>(evt =>
        {
            if (isDown)
                SelectTile(evt.localMousePosition.x, evt.localMousePosition.y, evt.ctrlKey);
        });

        //Configure Next Layer Button
        nextLayerButton = rootElement.Query<Button>("next-layer").First();
        nextLayerButton.clickable.clicked += () => NextLayer();

        gridSizeField = rootElement.Query<Vector3IntField>("grid-size").First();
        //Configure Grid Size
        gridSizeField.RegisterCallback<ChangeEvent<Vector3Int>>(UpdateGridSize);
        gridSizeField.value = Size;
        UpdateGrid(Size, true);
        //Configure Path Selector
        pathSelector = rootElement.Query<Image>("path-selector").First();
        Graphics.CopyTexture(tilemap, 0, 0, 0, 0, tilemapTileSize.x * 4, tilemapTileSize.y * 4, tileSelectionTexture, 0, 0, 0, 0);
        pathSelector.RegisterCallback<MouseDownEvent>(x => UpdateSelectedPath((int)x.localMousePosition.x, (int)x.localMousePosition.y));
        pathSelector.image = tileSelectionTexture;

        pathSelector.style.width = tilemapTileSize.x * 4;
        pathSelector.style.height = tilemapTileSize.y * 4;
        //Configure Effect Selector
        effectSelector = rootElement.Query<ObjectField>("effect-selector").First();
        effectSelector.objectType = typeof(GameBoardEffect);
        effectSelector.RegisterValueChangedCallback(x => UpdateGridTileEffect(selectedTile, x.newValue));
        //Configure Render Selector
        renderSelector = rootElement.Query<ObjectField>("render-selector").First();
        renderSelector.objectType = typeof(GameObject);
        renderSelector.RegisterValueChangedCallback(x => UpdateGridTileRender(selectedTile, x.newValue));
        //Configure Default Render Selector
        defaultRenderSelector = rootElement.Query<ObjectField>("default-render-selector").First();
        defaultRenderSelector.objectType = typeof(GameObject);
        defaultRenderSelector.RegisterValueChangedCallback(x =>
        {
            gameBoard.defaultRender = x.newValue as GameObject;
            EditorUtility.SetDirty(gameBoard);
        });
        UpdateNextLayerButton();
        RefreshTileProperties(selectedTile);
        return rootElement;
    }

    private Texture2D[] GenerateTextures()
    {
        int x, y, z;
        Texture2D[] textures = new Texture2D[Size.y];
        int type;
        Texture2D texture;
        int isSelected;
        for (y = 0; y < Size.y; y++)
        {
            texture = new Texture2D(tilemapTileSize.x * Size.x, tilemapTileSize.y * Size.z, tilemap.format, false);
            for (z = 0; z < Size.z; z++)
            {
                for (x = 0; x < Size.x; x++)
                {
                    isSelected = selectedTile.x == x && selectedTile.z == z && selectedTile.y == y ? (tilemapTileSize.x * 4) : 0;
                    type = (int)(gameBoard.tiles[y * Width * Length + z * Width + x]?.PathType ?? GameBoardPathType.BLOCKED);
                    Graphics.CopyTexture(tilemap, 0, 0, type % 4 * tilemapTileSize.x + isSelected, type / 4 * tilemapTileSize.y, tilemapTileSize.x, tilemapTileSize.y, texture, 0, 0, x * tilemapTileSize.x, z * tilemapTileSize.y);
                }
            }
            textures[y] = texture;
        }
        return textures;
    }
    private void UpdateGrid(Vector3Int newGridSize, bool force = false)
    {

        int newSize = newGridSize.x * newGridSize.y * newGridSize.z;
        Vector3Int oldGridSize = Size;
        Size = newGridSize;
        int oldSize = oldGridSize.x * oldGridSize.y * oldGridSize.z;
        if (newSize != oldSize || force)
        {
            GameBoardTile[] t = new GameBoardTile[newSize];
            for (int i = 0; i < Mathf.Min(oldSize, newSize); i++)
            {
                t[i] = gameBoard.tiles[i];
            }
            gameBoard.tiles = t;

            layerTextures = GenerateTextures();
            if (currentDepth >= newGridSize.y)
                currentDepth = 0;
            if (layerTextures.Length > currentDepth)
                gridField.image = layerTextures[currentDepth];
            gridField.style.width = newGridSize.x * tilemapTileSize.x;
            gridField.style.height = newGridSize.z * tilemapTileSize.y;
            UpdateNextLayerButton();
            rootElement.MarkDirtyRepaint();
        }
    }
    private void NextLayer()
    {
        currentDepth = (currentDepth + 1) % Size.y;
        gridField.image = layerTextures[currentDepth];
        SelectTile(new Vector3Int(selectedTile.x, currentDepth, selectedTile.z), false);

        RefreshTileProperties(selectedTile);
        UpdateNextLayerButton();
        rootElement.MarkDirtyRepaint();
    }
    private void UpdateNextLayerButton()
    {
        if (Size.y > 1)
        {
            nextLayerButton.SetEnabled(true);
            nextLayerButton.text = $"Show Layer {((currentDepth + 1) % Size.y) + 1}";
        }
        else
        {
            nextLayerButton.SetEnabled(false);
            nextLayerButton.text = "Show Layer 1";
        }


    }

    private void UpdateGridSize(ChangeEvent<Vector3Int> changeEvent)
    {

        if (changeEvent.newValue.x >= 0 && changeEvent.newValue.x <= MAX_TILES &&
        changeEvent.newValue.y >= 0 && changeEvent.newValue.y <= MAX_TILES &&
        changeEvent.newValue.z >= 0 && changeEvent.newValue.z <= MAX_TILES
        )
        {
            gameBoard.size = changeEvent.newValue;
            UpdateGrid(changeEvent.newValue);

        }
    }
    private void SelectTile(Vector3Int coordinates, bool updatePath)
    {
        Vector3Int oldSelectedTile = selectedTile;
        selectedTile = coordinates;
        UpdateGridTileTexture(oldSelectedTile);
        if (updatePath)
        {
            UpdateGridTilePathType(selectedTile, (GameBoardPathType)currentPathType, false);
        }
        UpdateGridTileTexture(selectedTile);
        RefreshTileProperties(selectedTile);

        rootElement.MarkDirtyRepaint();
    }
    private void SelectTile(float x, float y, bool updatePath)
    {

        SelectTile(new Vector3Int(
                (int)x / tilemapTileSize.x,
                currentDepth,
                (int)((gridField.style.height.value.value - y) / tilemapTileSize.y)
                ), updatePath);
    }

    private void UpdateGridTileTexture(Vector3Int coordinates)
    {
        UpdateGridTileTexture(new Vector2Int(coordinates.x, coordinates.z));
    }
    private void UpdateGridTileTexture(Vector2Int coordinates)
    {
        Texture2D texture = layerTextures[currentDepth];

        int isSelected = selectedTile.x == coordinates.x && selectedTile.z == coordinates.y ? (tilemapTileSize.x * 4) : 0;
        int type = (int)(gameBoard.tiles[currentDepth * (Width * Length) + coordinates.y * (Width) + coordinates.x]?.PathType ?? GameBoardPathType.BLOCKED);
        Graphics.CopyTexture(tilemap, 0, 0, type % 4 * tilemapTileSize.x + isSelected, type / 4 * tilemapTileSize.y, tilemapTileSize.x, tilemapTileSize.y, texture, 0, 0, coordinates.x * tilemapTileSize.x, coordinates.y * tilemapTileSize.y);

    }

    private void UpdateSelectedPath(int x, int y)
    {
        int oldPath = currentPathType;
        currentPathType = ((tilemap.height - y) / tilemapTileSize.y * 4) + (x / tilemapTileSize.x);
        Graphics.CopyTexture(tilemap, 0, 0, oldPath % 4 * tilemapTileSize.x, oldPath / 4 * tilemapTileSize.y, tilemapTileSize.x, tilemapTileSize.y, tileSelectionTexture, 0, 0, oldPath % 4 * tilemapTileSize.x, oldPath / 4 * tilemapTileSize.y);
        Graphics.CopyTexture(tilemap, 0, 0, currentPathType % 4 * tilemapTileSize.x + (tilemapTileSize.x * 4), currentPathType / 4 * tilemapTileSize.y, tilemapTileSize.x, tilemapTileSize.y, tileSelectionTexture, 0, 0, currentPathType % 4 * tilemapTileSize.x, currentPathType / 4 * tilemapTileSize.y);
        rootElement.MarkDirtyRepaint();
    }

    private void UpdateGridTilePathType(Vector3Int coordinates, GameBoardPathType pathType, bool update = true)
    {
        int index = coordinates.y * (Width * Length) + coordinates.z * (Width) + coordinates.x;
        if (index >= 0 && index < gameBoard.tiles.Length)
        {

            GameBoardTile tile = gameBoard.tiles[index];
            if (tile == null)
            {
                tile = new GameBoardTile();

                gameBoard.tiles[index] = tile;
            }
            tile.PathType = pathType;

            if (coordinates.y == currentDepth && update)
            {
                UpdateGridTileTexture(coordinates);
            }


            EditorUtility.SetDirty(gameBoard);
        }
    }

    private void UpdateGridTileRender(Vector3Int coordinates, Object render)
    {
        UpdateGridTileProperties(coordinates, render, null, true, false);
    }
    private void UpdateGridTileEffect(Vector3Int coordinates, Object effect)
    {
        UpdateGridTileProperties(coordinates, null, effect, false, true);
    }
    private void UpdateGridTileProperties(Vector3Int coordinates, GameObject render, Object effect)
    {
        UpdateGridTileProperties(coordinates, render, effect, true, true);
    }

    private void RefreshTileProperties(Vector3Int coordinates)
    {
        int index = coordinates.y * (Width * Length) + coordinates.z * (Width) + coordinates.x;
        GameBoardTile tile;
        if (index > 0 && index < gameBoard.tiles.Length) 
        tile = gameBoard.tiles[index];
        else
        tile = null;

        isRefreshingTileProperties = true;
        if (tile == null)
        {
            effectSelector.value = null;
            renderSelector.value = null;
        }
        else
        {
            effectSelector.value = tile.Effect;
            renderSelector.value = tile.Render;
        }
        isRefreshingTileProperties = false;
    }
    private void UpdateGridTileProperties(Vector3Int coordinates, Object render, Object effect, bool updateRender, bool updateEffect)
    {
        if (!isRefreshingTileProperties)
        {
            int index = coordinates.y * (Width * Length) + coordinates.z * (Width) + coordinates.x;
            GameBoardTile tile = gameBoard.tiles[index];
            if (tile == null)
            {
                tile = new GameBoardTile();

                gameBoard.tiles[index] = tile;
            }
            if (updateRender)
                tile.Render = render as GameObject;
            if (updateEffect)
                tile.Effect = effect as GameBoardEffect;
            EditorUtility.SetDirty(gameBoard);
        }
    }

}