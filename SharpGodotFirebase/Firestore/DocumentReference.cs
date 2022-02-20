using SharpGodotFirebase.Utilities;

namespace SharpGodotFirebase.Firestore
{
    public sealed class DocumentReference
    {
        /*internal FirestoreDB Database { get; }
        public string Id { get; }
        public CollectionReference Parent { get; }*/
        public string Path { get; }

        public DocumentReference(string path)
        {
            Path = PathEvaluator.Evaluate(path);
        }
    }
}
