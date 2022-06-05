using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnObj;
    [SerializeField] private Material firstMat;
    [SerializeField] private Material secondMat;
    [SerializeField] private Vector2Int size = Vector2Int.one;
    [ContextMenu("Spawn")]
    private void Spawn()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int y = 0, i = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                Transform basePlate = Instantiate(spawnObj, transform).transform;
                basePlate.gameObject.name = spawnObj.name + "_" + i;
                Renderer renderer = basePlate.GetComponent<Renderer>();
                if ((x + y) % 2 == 0)
                    renderer.material = firstMat;
                else
                    renderer.material = secondMat;

                basePlate.position = transform.position + new Vector3(x, 0, (-1) * y);
            }
        }
    }
    //private void OnDrawGizmos()
    //{
    //    for (int y = 0, i = 0; y < size.y; y++)
    //    {
    //        for (int x = 0; x < size.x; x++, i++)
    //        {
    //            if ((x + y) % 2 == 0)
    //                Gizmos.color = Color.red;
    //            else
    //                Gizmos.color = Color.blue;
    //            Gizmos.DrawSphere(transform.position + new Vector3(x, 0, (-1) * y), i * 0.01f);
    //        }
    //    }
    //}
}
