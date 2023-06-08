using Newtonsoft.Json.Linq;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class TextureAtlas
{
    private const float inverseGridSize = 1 / 64f;

    public enum BlockType { DIRT, STONE, SAND, OAK_LOG_SIDE, OAK_LOG_TOP, OAK_PLANK }

    public enum ItemType { }

    /* "gridCoordinates" refers to the grid position of the desired block in the 64 by 64 texture atlas
     * The returned array then refers to the set of actual values that Unity will use to render specific textures
     */
    public static Vector2[] GetUVCoordinates(Vector2Int gridCoordinates)
    {
        return new Vector2[]
        {
            new Vector2(inverseGridSize * gridCoordinates.x, inverseGridSize * gridCoordinates.y),             // (0, 0) -- bottom left
            new Vector2(inverseGridSize * gridCoordinates.x, inverseGridSize * (gridCoordinates.y + 1)),       // (0, 1) -- top left
            new Vector2(inverseGridSize * (gridCoordinates.x + 1), inverseGridSize * (gridCoordinates.y + 1)), // (1, 1) -- top right
            new Vector2(inverseGridSize * (gridCoordinates.x + 1), inverseGridSize * gridCoordinates.y)        // (1, 0) -- bottom right
        };
    }
}