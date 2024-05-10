    using UnityEngine;

public class SwingLineVisualizer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SwingingController swingingController;
    
    [SerializeField] private TrailRenderer trailRenderer;
    
    private void LateUpdate()
    {
        if (swingingController.IsSwinging && swingingController.hookTransform)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, swingingController.hookTransform.position);
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
            lineRenderer.enabled = false;
        }
    }
}