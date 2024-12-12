using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class InteractiveOutline : MonoBehaviour
{
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    public enum Mode
    {
        OutlineAll,
        OutlineVisible,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly
    }

    public Mode OutlineMode
    {
        get { return outlineMode; }
        set
        {
            outlineMode = value;
            needsUpdate = true;
        }
    }

    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            outlineColor = value;
            needsUpdate = true;
        }
    }

    public float OutlineWidth
    {
        get { return outlineWidth; }
        set
        {
            outlineWidth = value;
            needsUpdate = true;
        }
    }

    [SerializeField]
    private Mode outlineMode;

    [SerializeField]
    private Color outlineColor = Color.white;

    [SerializeField, Range(0f, 10f)]
    private float outlineWidth = 2f;

    [Header("Distance Settings")]
    [SerializeField]
    private Transform player; // Reference to the player's transform

    [SerializeField]
    private float activationDistance = 5f; // Maximum distance to enable outline

    [Header("Optional")]

    [SerializeField]
    private bool precomputeOutline;

    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material outlineMaskMaterial;
    [SerializeField] private Material outlineFillMaterial;

    private bool needsUpdate;
    private bool isMouseOver = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>();
        Debug.LogError("InteractiveOutLine Awake1");
        // Instantiate outline materials
        outlineMaskMaterial = Instantiate(Resources.Load<Material>("Materials/OutlineMask"));
        outlineFillMaterial = Instantiate(Resources.Load<Material>("Materials/OutlineFill"));
        Debug.LogError("InteractiveOutLine Awake2");

        outlineMaskMaterial.name = "OutlineMask (Instance)";
        outlineFillMaterial.name = "OutlineFill (Instance)";

        // Retrieve or generate smooth normals
        LoadSmoothNormals();
 

        // Apply material properties immediately
        needsUpdate = true;

        
        //Debug.LogError(GameObject.FindWithTag("Player"));


    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
        CheckAndEnableOutline();
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        DisableOutline();
    }

    private void CheckAndEnableOutline()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set.");
            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= activationDistance && isMouseOver)
        {
            EnableOutline();
        }
        else
        {
            DisableOutline();
        }
    }

    private void EnableOutline()
    {
        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();
            if (!materials.Contains(outlineMaskMaterial))
            {
                materials.Add(outlineMaskMaterial);
                materials.Add(outlineFillMaterial);
                renderer.materials = materials.ToArray();
            }
        }
    }

    private void DisableOutline()
    {
        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();
            if (materials.Contains(outlineMaskMaterial))
            {
                materials.Remove(outlineMaskMaterial);
                materials.Remove(outlineFillMaterial);
                renderer.materials = materials.ToArray();
            }
        }
    }

    void Update()
    {
        if (isMouseOver)
        {
            CheckAndEnableOutline();
        }

        if (needsUpdate)
        {
            needsUpdate = false;
            UpdateMaterialProperties();
        }
    }

    private void OnDestroy()
    {
        Destroy(outlineMaskMaterial);
        Destroy(outlineFillMaterial);
    }

    private void LoadSmoothNormals()
    {
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);
        }
    }

    private List<Vector3> SmoothNormals(Mesh mesh)
    {
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
        var smoothNormals = new List<Vector3>(mesh.normals);

        foreach (var group in groups)
        {
            if (group.Count() == 1)
            {
                continue;
            }

            var smoothNormal = Vector3.zero;
            foreach (var pair in group)
            {
                smoothNormal += smoothNormals[pair.Value];
            }

            smoothNormal.Normalize();
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    private void UpdateMaterialProperties()
    {
        outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

        switch (outlineMode)
        {
            case Mode.OutlineAll:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
            case Mode.OutlineVisible:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
            case Mode.OutlineHidden:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
            case Mode.OutlineAndSilhouette:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
            case Mode.SilhouetteOnly:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
                break;
        }
    }
}