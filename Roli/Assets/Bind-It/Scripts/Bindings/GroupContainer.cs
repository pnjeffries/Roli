using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that attaches to a a group container template and is used to specify
/// the location to which new items created within the group should be parented
/// </summary>
[AddComponentMenu("Binding/Group Container")]
public class GroupContainer : MonoBehaviour
{
    [Tooltip("The element within the template which will contain the item representations in this group.  " +
        "If null (or this component is not added) the root element will be used.")]
    public GameObject ContentsContainer = null;
}
