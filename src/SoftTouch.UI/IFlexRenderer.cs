using SoftTouch.UI.Flexbox;

namespace SoftTouch.UI;

public interface IFlexRenderer
{
    public Tree RenderTree { get; set; }
    public void Render<T>(Tree<T> node)
        where T : FixedView;
}
