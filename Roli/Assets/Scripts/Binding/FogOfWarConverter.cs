using Binding;
using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Converts a field of view record into mesh vertex colours
/// </summary>
[Serializable]
public class FogOfWarConverter : ValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SquareCellMap<int> squareMap)
        {
            //Vertex colours:
            return VertexColoursFromVisionMap(squareMap);
        }
        else throw new NotSupportedException("Conversion type not supported");
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public static int FogVertexResolution()
    {
        return 1;
    }

    /// <summary>
    /// Set the vertex colours of this mesh based on the specified fieldOfView map.
    /// For this to do anything at all reasonable, the mesh should have been generated
    /// as a regular grid with (mapXSize + 2) * 2 + 1 vertices in the first direction via a function
    /// such as MeshBuilder.AddQuadGridMesh.
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="fieldOfView"></param>
    /// <param name="oldFieldOfView"></param>
    /// <param name="t"></param>
    /// <param name="tweening"></param>
    /// <param name="power">The power to use for interpolating values in-between cells</param>
    public static Color32[] VertexColoursFromVisionMap(SquareCellMap<int> fieldOfView)
    {
        int mapX = fieldOfView.SizeX;
        int mapY = fieldOfView.SizeY;
        int res = FogVertexResolution();
        int vertexCount = ((mapX + 2) * res + 1)* ((mapY + 2) * res + 1);
        Color32[] vertexColours = new Color32[vertexCount];
        int uSizeMesh = (mapX + 2) * res + 1;
        for (int i = 0; i < vertexCount; i++)
        {
            int u = i % uSizeMesh;
            int v = i / uSizeMesh;
            double x = (u - 1.0 - res) / res;
            double y = (v - 1.0 - res) / res;
            double viewValue = 0;
            if (x.IsWholeNumber() && y.IsWholeNumber())
            {
                int iX = (int)x;
                int iY = (int)y;
                viewValue = fieldOfView[iX, iY];
            }
            else
            {
                viewValue = AverageGridValue(x, y, fieldOfView);
            }
            byte alpha = (byte)(255.0 / 10 * (10 - viewValue));
            vertexColours[i] = new Color32(0, 0, 0, alpha);
        }

        return vertexColours;
    }

    private static double AverageGridValue(double x, double y, SquareCellMap<int> fieldOfView)
    {
        double viewValue = 0;
        // Intermediate vertex - take the average of the adjoining cells:
        if (x >= 0)
        {
            if (y >= 0)
                viewValue += fieldOfView[(int)x.Floor(), (int)y.Floor()];
            if (y < fieldOfView.SizeY)
                viewValue += fieldOfView[(int)x.Floor(), (int)y.Ceiling()];
        }
        if (x < fieldOfView.SizeX)
        {
            if (y >= 0)
                viewValue += fieldOfView[(int)x.Ceiling(), (int)y.Floor()];
            if (y < fieldOfView.SizeY)
                viewValue += fieldOfView[(int)x.Ceiling(), (int)y.Ceiling()];
        }
        viewValue /= 4;
        return viewValue;
    }
}
