using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;
using CMythos;
[CustomEditor(typeof(Dice))]
public class DiceEditor : Editor
{

    [SerializeField]
    private Dice dice;
    private MeshFilter diceMeshFilter;
    private MeshCollider diceMeshCollider;
    private VisualElement rootElement;
    private VisualTreeAsset visualTree;

    private List<Tuple<Vector3, Vector3>> faces;

    private Toggle invertOrientation;

    private VisualElement sideValues;

    private Vector3Field orientation;


    private void OnEnable()
    {
        dice = (Dice)target;
        diceMeshFilter = dice.GetComponent<MeshFilter>();
        diceMeshCollider = dice.GetComponent<MeshCollider>();
        rootElement = new VisualElement();
        if (invertOrientation == null)
            invertOrientation = new Toggle
            {
                label = "Invert Orientation"
            };
        sideValues = new VisualElement();
        orientation = new Vector3Field
        {
            label = "Orientation",
            value = dice.MatchDirection
        };
        orientation.RegisterValueChangedCallback(x =>
        {
            dice.matchDirection = x.newValue;

            EditorUtility.SetDirty(dice);
            serializedObject.ApplyModifiedProperties();
        });
        faces = new List<Tuple<Vector3, Vector3>>();
    }
    public override VisualElement CreateInspectorGUI()
    {
        rootElement.Clear();
        sideValues.Clear();
        //visualTree.CloneTree(rootElement);
        rootElement.Add(orientation);
        rootElement.Add(invertOrientation);
        rootElement.Add(sideValues);
        faces = CalculateFaceNormals();
        CreateFaceFields(sideValues, faces);

        return rootElement;
    }

    private List<Tuple<Vector3, Vector3>> CalculateFaceNormals()
    {
        diceMeshCollider.sharedMesh.RecalculateNormals();
        List<Tuple<Vector3, Vector3>> tempFaces = new List<Tuple<Vector3, Vector3>>();

        Tuple<Vector3, Vector3> similar;
        Tuple<Vector3, Vector3> face;
        for (int i = 0; i < diceMeshCollider.sharedMesh.triangles.Length / 3; i++)
        {
            face = Tuple.Create(
                (diceMeshCollider.sharedMesh.vertices[diceMeshCollider.sharedMesh.triangles[i * 3]]
                + diceMeshCollider.sharedMesh.vertices[diceMeshCollider.sharedMesh.triangles[i * 3 + 1]]
                + diceMeshCollider.sharedMesh.vertices[diceMeshCollider.sharedMesh.triangles[i * 3 + 2]]) / 3
                ,
                (diceMeshCollider.sharedMesh.normals[diceMeshCollider.sharedMesh.triangles[i * 3]]
                + diceMeshCollider.sharedMesh.normals[diceMeshCollider.sharedMesh.triangles[i * 3 + 1]]
                + diceMeshCollider.sharedMesh.normals[diceMeshCollider.sharedMesh.triangles[i * 3 + 2]]) / 3
            );
            similar = tempFaces.Find(x => x.Item2 == face.Item2);

            if (similar == null)
            {
                tempFaces.Add(face);
            }
            else
            {
                tempFaces.Remove(similar);
                tempFaces.Add(Tuple.Create(
                    (face.Item1 + similar.Item1) / 2
                    ,
                    face.Item2
                ));
            }

        }
        return tempFaces;
    }

    private void CreateFaceFields(VisualElement root, List<Tuple<Vector3, Vector3>> faceInfo)
    {
        VisualElement element;
        TextField textField;
        Button button;

        Debug.Log(dice.values);
        int index = 0;
        foreach (Tuple<Vector3, Vector3> face in faceInfo)
        {
            element = new VisualElement();
            element.style.flexDirection = FlexDirection.Row;
            textField = new TextField
            {

                value = dice.values.ContainsKey(face.Item2) ? dice.values[face.Item2] : ""
            };
            textField.RegisterValueChangedCallback(x =>
            {
                Debug.Log("value updated");
                dice.values[face.Item2] = x.newValue;
                EditorUtility.SetDirty(dice);
                serializedObject.ApplyModifiedProperties();
            });
            textField.style.flexGrow = 9;
            element.Add(textField);
            button = new Button
            {
                text = "Orientate"

            };
            button.clickable.clicked += () =>
            {
                dice.transform.rotation = Quaternion.FromToRotation(face.Item2, invertOrientation.value ? -dice.MatchDirection : dice.MatchDirection);
            };
            element.Add(button);
            root.Add(element);
            index++;
        }
    }
    void OnSceneGUI()
    {
        if (diceMeshFilter != null)
            foreach (var face in faces)
            {
                Handles.matrix = diceMeshFilter.transform.localToWorldMatrix;
                Handles.color = Color.yellow;
                Handles.DrawLine(
                    face.Item1,
                    face.Item1 + face.Item2);
            }

    }

}