namespace DbScout.Contracts
{
    public delegate void OutputRenderEventHandler(object sender, OutputRenderEventArgs e);

    public interface IOutputRenderer
    {
        string RenderOutput(IDatabaseObject dbObject);

        event OutputRenderEventHandler OutputRenderingEvent;

        event OutputRenderEventHandler OutputRenderedEvent;
    }
}
