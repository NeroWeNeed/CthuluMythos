using UnityEngine;
using UnityEditor;
using CMythos;

[CustomEditor(typeof(DiceShooter))]
public class DiceShooterEditor : Editor
{
    void OnSceneGUI()
    {
        DiceShooter diceShooter = target as DiceShooter;
        Handles.matrix = diceShooter.transform.localToWorldMatrix;
        Handles.color = Color.yellow;
        Vector3 half = (diceShooter.GetComponent<BoxCollider>().size / 2);
        
        Handles.DrawSphere(0,half,Quaternion.identity,diceShooter.spawnRadius);

        
        Handles.DrawLine(half,-half*diceShooter.spawnRadius*0.05f);


/*         Handles.DrawPolyLine(
new Vector3(diceShooter.transform.position.x - diceShooter.size.x, diceShooter.transform.position.y - diceShooter.size.y, diceShooter.transform.position.z),
new Vector3(diceShooter.transform.position.x + diceShooter.size.x, diceShooter.transform.position.y - diceShooter.size.y, diceShooter.transform.position.z),
new Vector3(diceShooter.transform.position.x + diceShooter.size.x, diceShooter.transform.position.y + diceShooter.size.y, diceShooter.transform.position.z),
new Vector3(diceShooter.transform.position.x - diceShooter.size.x, diceShooter.transform.position.y + diceShooter.size.y, diceShooter.transform.position.z),
new Vector3(diceShooter.transform.position.x - diceShooter.size.x, diceShooter.transform.position.y - diceShooter.size.y, diceShooter.transform.position.z)
        ); */


    }
}