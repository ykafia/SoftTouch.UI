using SoftTouch.UI.Flexbox;

namespace SoftTouch.UI;

public interface IFlexRenderer
{
    public FlexTree RenderTree { get; set; }
    public void Render();
}
