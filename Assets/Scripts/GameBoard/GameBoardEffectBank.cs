namespace CMythos
{
    using UnityEngine;


    public class GameBoardEffectBank : MonoBehaviour
    {
        public void ElevatorLandEffect(GameBoardEntity entity, Vector3Int coordinates)
        {
            GameBoardPlayer player = entity.GetComponent<GameBoardPlayer>();
            GameBoardManager manager = player.GetGameBoardManager();
            Vector3Int newCoords = new Vector3Int(coordinates.x, coordinates.y == 0 ? 1 : 0, coordinates.z);
            if (manager.GetTile(newCoords).PathType != GameBoardPathType.BLOCKED)
            {
                player.GetComponent<GameBoardPlayer>().SetCoordinates(newCoords);
            }



        }
    }
}