using SharpGodotFirebase.Utilities;

namespace SharpGodotFirebase.Firestore
{
    public sealed class CollectionReference : Query
    {
        /*public string Id { get; }
        public DocumentReference Parent { get; }*/
        public string Path { get; private set; }

        public CollectionReference(string path)
        {
            Path = PathEvaluator.Evaluate(path);
        }
    }
}