namespace SoftTouch.UI.Flexbox;

public enum FlexContentAlignment
{
    /// <summary>
    /// Align wrapped lines to the start of the container's cross axis.
    /// </summary>
    FlexStart,
    /// <summary>
    /// Align wrapped lines to the end of the container's cross axis.
    /// </summary>
    FlexEnd,
    /// <summary>
    /// Stretch wrapped lines to match the height of the container's cross axis.
    /// </summary>
    Stretch,
    /// <summary>
    /// Align wrapped lines in the center of the container's cross axis.
    /// </summary>
    Center,
    /// <summary>
    /// Evenly space wrapped links across container's main axis distributing remaining space
    /// </summary>
    SpaceBetween,
    /// <summary>
    /// Evenly space wrapped links across the contianer's main axis, distributing remaining space around the lines. 
    /// Compared to space between using space around will result in 
    /// space being distributed to the beginning of the first lines and end of the last line.
    /// </summary>
    SpaceAround


}
