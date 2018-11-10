using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Walkable,
        Layer.Enemy,
        Layer.NPC,
        Layer.Buildings
    };

   [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit rayHit;
    public RaycastHit hit
    {
        get { return rayHit; }
    }

    Layer layerRaycastHit;
    public Layer layerHit
    {
        get { return layerRaycastHit; }
    }

    public delegate void OnLayerChange(Layer newLayer);
    public event OnLayerChange onLayerChange;

    void Start()
    {
        viewCamera = Camera.main;
    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                rayHit = hit.Value;
                if(layerRaycastHit != layer)
                {
                    layerRaycastHit = layer;
                    onLayerChange(layer);
                }
                layerRaycastHit = layer;
                return;
            }
        }

        // Otherwise return background hit
        rayHit.distance = distanceToBackground;
        layerRaycastHit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
