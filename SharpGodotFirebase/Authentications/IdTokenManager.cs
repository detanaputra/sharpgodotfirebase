using SharpGodotFirebase.Utilities;

using System;

namespace SharpGodotFirebase.Authentications
{
    internal static class IdTokenManager
    {
        internal const string FirebaseUserIdTokenExpiresAt = "FirebaseUserIdTokenExpiresAt";
        internal static long GetExpiresAt(int expiresIn)
        {
            DateTimeOffset expiresAt = DateTimeOffset.Now.AddSeconds(expiresIn);
            return expiresAt.ToUnixTimeSeconds();
        }

        internal static bool IsExpired()
        {
            if (DataPersister.Build().TryGetValue<long>(FirebaseUserIdTokenExpiresAt, out long value))
            {
                long now = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (now >= value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            Logger.LogErr("can not retrieve idtokenexpiresat value");
            return false;
        }
    }
}