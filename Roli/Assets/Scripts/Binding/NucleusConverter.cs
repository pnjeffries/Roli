using Binding;
using Nucleus.Geometry;
using Nucleus.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public class NucleusConverter : DefaultConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //Debug.Log("Converting " + (value?.ToString() ?? "null"));
        if (value is Vector vector)
        {
            if (targetType == typeof(Vector2)) return ToUnity.Convert2D(vector);
            if (targetType.IsAssignableFrom(typeof(Vector3))) return ToUnity.Convert(vector);
        }
        else if (value is VertexGeometry vG)
        {
            if (targetType.IsAssignableFrom(typeof(List<Vector2>)))
            {
                var result = new List<Vector2>();
                foreach (var v in vG.Vertices)
                {
                    result.Add(ToUnity.Convert(v.Position));
                }
                return result;
            }
            else if (targetType.IsAssignableFrom(typeof(List<Shapes.PolylinePoint>)))
            {
                var result = new List<Shapes.PolylinePoint>();
                foreach (var v in vG.Vertices)
                {
                    result.Add(new Shapes.PolylinePoint(ToUnity.Convert(v.Position)));
                }
                return result;
            }
        }
        else if (value is SquareCellMap<int> squareMap)
        {
            if (targetType.IsAssignableFrom(typeof(UnityEngine.Mesh)))
            {
                UnityMeshBuilder builder = new UnityMeshBuilder();
                int mapX = squareMap.SizeX;
                int mapY = squareMap.SizeY;
                int res = FogOfWarConverter.FogVertexResolution();
                int gridX = (mapX + 2) * res + 1;
                int gridY = (mapY + 2) * res + 1;
                builder.AddQuadGridMesh(new Vector(-2 + 0.5*res, -2 + 0.5*res), new Vector(1.0/res, 0), new Vector(0, 1.0/res),
                    gridX, gridY, 20);
                builder.Finalize();
                var mesh = builder.Mesh;
                mesh.uv = GenerateGridUVs(gridX, gridY);
                mesh.colors32 = FogOfWarConverter.VertexColoursFromVisionMap(squareMap);
                return builder.Mesh;
            }
        }
        return base.Convert(value, targetType, parameter, culture);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private Vector2[] GenerateGridUVs(int gridX, int gridY)
    {
        var result = new Vector2[gridX * gridY];
        for (int i = 0; i < gridY; i++)
        {
            for (int j = 0; j < gridX; j++)
            {
                int index = j + i * gridX;
                result[index] = new Vector2(
                    j * 1f / (gridX - 1), i * 1f / (gridY - 1));
            }
        }
        return result;
    }
}

