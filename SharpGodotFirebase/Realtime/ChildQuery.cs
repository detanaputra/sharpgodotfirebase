using SharpGodotFirebase.Utilities;

namespace SharpGodotFirebase.Realtime
{
    public class ChildQuery
    {
        public string Path { get; set; }

        public ChildQuery Child(string path)
        {
            Path += "/" + path;
            return this;
        }

        public ChildQuery(string path)
        {
            Path = PathEvaluator.Evaluate(path);
        }
    }
}
