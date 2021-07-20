using Binding;
using Nucleus.Game;
using Nucleus.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ElementFOVConverter : MultiValueConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is SquareCellMap<int> map && values[1] is int cellIndex)
        {
            if (map[cellIndex] >= MapAwareness.Visible) return true;
            else
            {
                int maxAdjacent = map.AdjacencyCount(cellIndex);
                for (int i = 0; i < maxAdjacent; i++)
                {
                    int adjacentIndex = map.AdjacentCellIndex(cellIndex, i);
                    if (map.Exists(adjacentIndex) && map[adjacentIndex] >= MapAwareness.Visible) return true;
                }
            }

        }
        return false;
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
