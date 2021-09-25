using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Tooltip("The gridline template")]
    public Shapes.Line Template;

    public Vector2 Origin;

    public Vector2 GridSize;

    public Vector2 CellCount;

    public float ScrollSpeed = 1;

    private List<Shapes.Line> _Horizontals;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        float offset = GridSize.y * ((Time.time * ScrollSpeed) % 1.0f);
        foreach (var line in _Horizontals)
        {
            line.transform.localPosition = new Vector3(0, 0, -offset);
        }
    }

    private void GenerateGrid()
    {
        _Horizontals = new List<Shapes.Line>();

        float maxY = Origin.y + CellCount.y * GridSize.y;
        float maxX = Origin.x + CellCount.x * GridSize.x;

        for (int i = 0; i < CellCount.x; i++)
        {
            var line = Instantiate(Template, transform);
            line.Start = new Vector3(Origin.x + (i * GridSize.x), 0, Origin.y);
            line.End = new Vector3(Origin.x + (i * GridSize.x), 0, maxY);
        }

        for (int i = 0; i < CellCount.x; i++)
        {
            var line = Instantiate(Template, transform);
            line.Start = new Vector3(Origin.x , 0, Origin.y + (i * GridSize.y));
            line.End = new Vector3(maxX, 0, Origin.y + (i * GridSize.y));
            _Horizontals.Add(line);
        }
    }
}
