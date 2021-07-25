using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nucleus.IO;
using Nucleus.Base;

/// <summary>
/// Unity resource loader
/// </summary>
public class ResourceLoader : IResourceLoader
{
    public string LoadString(FilePath filePath)
    {
        var resource = Resources.Load<TextAsset>(filePath);
        return resource?.text;
    }
}
