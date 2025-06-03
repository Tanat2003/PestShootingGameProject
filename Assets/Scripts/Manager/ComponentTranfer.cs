using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentTranfer : MonoBehaviour
{
    [ContextMenu("Tranfer Component To Child")]

    public void TransferComponentToChild()
    {
        GameObject selectGameObject = transform.gameObject;

        if (selectGameObject == null)
            return;

        //���ҧOBJ������Child
        GameObject childGameObject = new GameObject("Car_Wheel");
        childGameObject.transform.SetParent(selectGameObject.transform);
        childGameObject.transform.localPosition = Vector3.zero;
        childGameObject.transform.localRotation = Quaternion.identity;

        //Duplicate mesh Filter �parent�����child
        MeshFilter meshFilter = selectGameObject.GetComponent<MeshFilter>();
        if(meshFilter != null )
        {
            MeshFilter childMeshFilter = childGameObject.AddComponent<MeshFilter>();
            childMeshFilter.sharedMesh = meshFilter.sharedMesh;
            DestroyImmediate(meshFilter);

        }

        //Duplicate mesh renderer �parent�����child
        MeshRenderer meshRenderer = selectGameObject.GetComponent<MeshRenderer>();
        if(meshRenderer != null)
        {
            MeshRenderer childMeshRenderer = childGameObject.AddComponent<MeshRenderer>();
            childMeshRenderer.sharedMaterial = meshRenderer.sharedMaterial;
            DestroyImmediate(meshRenderer);

        }
    }
}
