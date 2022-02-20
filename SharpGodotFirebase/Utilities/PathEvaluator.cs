namespace SharpGodotFirebase.Utilities
{
    internal static class PathEvaluator
    {
        internal static string Evaluate(string path)
        {
            string Path = path;
            if (Path.StartsWith("/"))
            {
                Path = Path.Remove(0, 1);
            }

            if (Path.EndsWith("/"))
            {
                Path = Path.Remove(Path.Length - 1);
            }
            return Path;
        }
    }
}
