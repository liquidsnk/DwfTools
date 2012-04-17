using DwfTools.Renderer;

namespace DwfTools.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderer = new HtmlImgRenderer();

            var data = W2dParser.GetParsedData(@"C:\Users\Liquidsnk\Documents\Visual Studio 2010\Projects\DwfTools\DwfTools\Sample\FF6A06F4-B452-48B0-A328-74702E883FEA.w2d");
            renderer.Render(data);

            System.Console.ReadKey();
        }
    }
}
