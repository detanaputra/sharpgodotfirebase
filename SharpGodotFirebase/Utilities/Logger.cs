using Godot;

namespace SharpGodotFirebase.Utilities
{
    internal static class Logger
    {
        internal static void Log(params object[] msg)
        {
            Convert(msg, out object[] newMsg);
            GD.Print(newMsg);
        }

        internal static void LogErr(params object[] msg)
        {
            Convert(msg, out object[] newMsg);
            GD.PrintErr(newMsg);
        }

        private static void Convert(object[] msg, out object[] newMsg)
        {
            newMsg = new object[msg.Length + 1];
            newMsg[0] = "[GodotSharpFirebase]--> ";
            for (int i = 0; i < msg.Length; i++)
            {
                newMsg[i + 1] = msg[i];
            }
        }
    }
}
