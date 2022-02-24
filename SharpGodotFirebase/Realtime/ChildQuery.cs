using SharpGodotFirebase.Utilities;

namespace SharpGodotFirebase.Realtime
{
    public class ChildQuery
    {
        public string Path { get; set; }
        internal string LastSection { get; set; }

        public ChildQuery Child(string path)
        {
            string cleanPath = PathEvaluator.Evaluate(path);
            Path += "/" + cleanPath;
            LastSection = cleanPath;
            return this;
        }

        public ChildQuery(string path)
        {
            Path = PathEvaluator.Evaluate(path);
        }
    }
}
