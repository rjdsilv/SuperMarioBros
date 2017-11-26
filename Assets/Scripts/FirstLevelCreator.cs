using UnityEditor;
using UnityEngine;

/// <summary>
/// Creates the Super Mario Bros 1st Level World.
/// </summary>
public class FirstLevelCreator : MonoBehaviour {
    // Constant Declaration.
    const float BLOCK_SIDE = 0.32f;
    const int FLOOR_LINES = 2;

    // Attribute Declaration.
    private Vector3 world;

    // Use this for initialization
    void Start ()
    {
        world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        CreateFloor();
	}

    private void CreateFloor()
    {
        // Calculates the number of stones will be on the floor.
        GameObject floorStonePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/FloorStone.prefab");
        float worldCurrentPositionY = -world.y;

        // Spawns the floor vertically.
        for (int i = 0; i < FLOOR_LINES; i++)
        {
            float worldCurrentPositionX = -world.x + BLOCK_SIDE / 2;

            // Spawns the floor horizontally.
            while (worldCurrentPositionX < world.x)
            {
                Instantiate(floorStonePrefab, new Vector3(worldCurrentPositionX, worldCurrentPositionY, 0f), Quaternion.identity);
                worldCurrentPositionX += BLOCK_SIDE;
            }

            worldCurrentPositionY += BLOCK_SIDE;
        }
    }
}
