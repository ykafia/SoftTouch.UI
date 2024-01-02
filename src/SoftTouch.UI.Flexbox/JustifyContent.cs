namespace SoftTouch.UI.Flexbox;

public enum JustifyContent
{
    /// <summary>
    /// Align children of a container to the start of the container's main axis.
    /// </summary>
    FlexStart,
    /// <summary>
    /// Align children of a container to the end of the container's main axis.
    /// </summary>
    FlexEnd,
    /// <summary>
    /// Align children of a container in the center of the container's main axis.
    /// </summary>
    Center,
    /// <summary>
    /// Evenly space of children across the container's main axis, distributing remaining space between the children.
    /// </summary>
    SpaceBetween,
    /// <summary>
    /// Evenly space of children across the container's main axis, distributing remaining space around the children. Compared to space between using space around will result in space being distributed to the beginning of the first child and end of the last child.
    /// </summary>
    SpaceAround,
    /// <summary>
    /// Evenly distributed within the alignment container along the main axis. The spacing between each pair of adjacent items, the main-start edge and the first item, and the main-end edge and the last item, are all exactly the same.
    /// </summary>
    SpaceEvenly
}
