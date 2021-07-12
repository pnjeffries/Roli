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
        Debug.Log("Converting " + (value?.ToString() ?? "null"));
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
                builder.AddQuadGridMesh(new Vector(-1, -1), new Vector(0.5, 0), new Vector(0, 0.5),
                    (mapX + 2) * 2 + 1, (mapY + 2) * 2 + 1);
                builder.Finalize();
                UnityEngine.Mesh mesh = builder.Mesh;
            }
        }
        return base.Convert(value, targetType, parameter, culture);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

