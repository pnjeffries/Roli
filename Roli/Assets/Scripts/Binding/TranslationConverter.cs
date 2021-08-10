using Binding;
using Nucleus.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Converter to automatically translate bound text via the current language pack
/// </summary>
[Serializable]
public class TranslationConverter : ValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return GameEngine.Instance.LanguagePack?.GetText(value.ToString()) ?? value.ToString();
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
