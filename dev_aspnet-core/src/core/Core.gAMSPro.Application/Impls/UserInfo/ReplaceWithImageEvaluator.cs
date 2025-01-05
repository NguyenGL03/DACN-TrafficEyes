using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Replacing;

namespace Core.gAMSPro.Impls.UserInfo
{
    public class ReplaceWithImageEvaluator : IReplacingCallback
    {

        private readonly MemoryStream _stream;
        private readonly string _name;
        internal ReplaceWithImageEvaluator(MemoryStream stream, string name)
        {
            _stream = stream;
            _name = name;
        }

        ReplaceAction IReplacingCallback.Replacing(ReplacingArgs args)
        {
            try
            {
                DocumentBuilder builder = new DocumentBuilder((Document)args.MatchNode.Document);
                builder.MoveTo(args.MatchNode);

                _stream.Position = 0;
                builder.InsertImage(_stream,
                    RelativeHorizontalPosition.Default, 50,
                    RelativeVerticalPosition.TopMargin,
                    -200,
                    0, 0,
                    WrapType.TopBottom
                    );
                args.Replacement = "";
                return ReplaceAction.Replace;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ReplaceAction.Skip;
            }
        }
    }
}
