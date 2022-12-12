using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width;
    public int height;
    GridRenderer gRendered;
    void Start()
    {
        this.width = ParametersManager.Instance.grid_width;
        this.height = ParametersManager.Instance.grid_height;
        Terminal terminal = TheGrid.CreateGrid(width, height);
        gRendered = GetComponent<GridRenderer>();
        gRendered.RenderGrid();

        List<Service> services = terminal.GetServices();
        gRendered.RenderServicesWalls(services);
    }

}
