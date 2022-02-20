namespace SharpGodotFirebase.Utilities
{
    internal static class UrlBuilder
    {
        #region Authentication
        private const string IdentityBaseUrl = "https://identitytoolkit.googleapis.com/v1/";
        private const string UserdataEndpoint = "accounts:lookup?key=";
        private const string SigninEndpoint = "accounts:signInWithPassword?key=";
        private const string SigninGoogleOAuthEndpoint = "accounts:signInWithIdp?key=";
        private const string AnonymousSigninEndpoint = "accounts:signUp?key=";
        private const string DeleteAccountEndpoint = "accounts:delete?key=";
        #endregion

        #region Authentication using Emulator        
        private const string LocalhostAddress = "http://127.0.0.1:";
        private const string EmulatedIdentityBaseUrl = "/identitytoolkit.googleapis.com/v1/";
        #endregion

        #region OAuth
        private const string RefreshTokenEndpoint = "https://securetoken.googleapis.com/v1/token?key=";
        private const string EmulatedRefreshTokenEndpoint = "/securetoken.googleapis.com/v1/token?key=";
        private const string GoogleAuthRequestUrl = "https://accounts.google.com/o/oauth2/v2/auth?";
        private const string ExchangeGoogleTokenRequestUrl = "https://oauth2.googleapis.com/token?";
        #endregion

        #region Firestore
        private const string FirestoreBaseUrl = "https://firestore.googleapis.com/v1/";
        private const string FirestoreEndpoint = "projects/[PROJECT_ID]/databases/";
        private const string EmulatedFirestoreBaseUrl = "/v1/";
        #endregion

        #region Authentication Methods
        internal static string GetSigninWithPasswordUrl()
        {
            string address = IdentityBaseUrl + SigninEndpoint + FirebaseClient.Config.WebAPIKey;
            if (FirebaseClient.Config.UseEmulator)
            {
                address = LocalhostAddress + FirebaseClient.Config.AuthEmulatorPort + EmulatedIdentityBaseUrl + SigninEndpoint + FirebaseClient.Config.WebAPIKey;
            }
            return address;
        }

        internal static string GetSigninAnonymousUrl()
        {
            string address = IdentityBaseUrl + AnonymousSigninEndpoint + FirebaseClient.Config.WebAPIKey;
            if (FirebaseClient.Config.UseEmulator)
            {
                address = LocalhostAddress + FirebaseClient.Config.AuthEmulatorPort + EmulatedIdentityBaseUrl + AnonymousSigninEndpoint + FirebaseClient.Config.WebAPIKey;
            }
            return address;
        }

        internal static string GetSigninOAuthUrl()
        {
            string address = IdentityBaseUrl + SigninGoogleOAuthEndpoint + FirebaseClient.Config.WebAPIKey;
            if (FirebaseClient.Config.UseEmulator)
            {
                address = LocalhostAddress + FirebaseClient.Config.AuthEmulatorPort + EmulatedIdentityBaseUrl + SigninGoogleOAuthEndpoint + FirebaseClient.Config.WebAPIKey;
            }
            return address;
        }

        internal static string GetUserdataUrl()
        {
            string address = IdentityBaseUrl + UserdataEndpoint + FirebaseClient.Config.WebAPIKey;
            if (FirebaseClient.Config.UseEmulator)
            {
                address = LocalhostAddress + FirebaseClient.Config.AuthEmulatorPort + EmulatedIdentityBaseUrl + UserdataEndpoint + FirebaseClient.Config.WebAPIKey;
            }
            return address;
        }

        internal static string GetDeleteAccountUrl()
        {
            string address = IdentityBaseUrl + DeleteAccountEndpoint + FirebaseClient.Config.WebAPIKey;
            if (FirebaseClient.Config.UseEmulator)
            {
                address = LocalhostAddress + FirebaseClient.Config.AuthEmulatorPort + EmulatedIdentityBaseUrl + DeleteAccountEndpoint + FirebaseClient.Config.WebAPIKey;
            }
            return address;
        }

        internal static string GetRefreshTokenUrl()
        {
            string address = RefreshTokenEndpoint + FirebaseClient.Config.WebAPIKey;
            if (FirebaseClient.Config.UseEmulator)
            {
                address = LocalhostAddress + FirebaseClient.Config.AuthEmulatorPort + EmulatedRefreshTokenEndpoint + FirebaseClient.Config.WebAPIKey;
            }
            return address;
        }

        internal static string GetGoogleAuthRequestUrl()
        {
            return GoogleAuthRequestUrl;
        }

        internal static string GetExchangeGoogleTokenRequestUrl()
        {
            return ExchangeGoogleTokenRequestUrl;
        }

        internal static string GetOAuthRequestUrl(string providerId)
        {
            switch (providerId)
            {
                // TODO create facebook oauth request and other provider
                case Authentications.ProviderId.GOOGLE:
                    return GoogleAuthRequestUrl;
                default:
                    return GoogleAuthRequestUrl;
            }
        }

        internal static string GetSigninOAuthUrl(string providerId)
        {
            switch (providerId)
            {
                // TODO create facebook oauth request and other provider
                case Authentications.ProviderId.GOOGLE:
                    return IdentityBaseUrl + SigninGoogleOAuthEndpoint + FirebaseClient.Config.WebAPIKey;
                default:
                    return IdentityBaseUrl + SigninGoogleOAuthEndpoint + FirebaseClient.Config.WebAPIKey;
            }
        }
        #endregion

        internal static string GetFirestoreUrl(string path, string database = "%28default%29")
        {
            string address = FirestoreBaseUrl + FirestoreEndpoint + database + "/documents/" + path;
            if (FirebaseClient.Config.UseEmulator)
            {
                address = LocalhostAddress + FirebaseClient.Config.FirestoreEmulatorPort + EmulatedFirestoreBaseUrl + FirestoreEndpoint + database + "/documents/" + path;
            }
            address = address.Replace("[PROJECT_ID]", FirebaseClient.Config.ProjectID);

            return address;
        }

        internal static string GetRealtimeUrl(string path, string database = "default")
        {
            string address = string.Format("https://{0}-{1}-rtdb.{2}.firebasedatabase.app/{3}.json?auth={4}", FirebaseClient.Config.ProjectID, database, FirebaseClient.Config.RealtimeDBRegion, path, FirebaseClient.User.IdToken);
            if (FirebaseClient.Config.UseEmulator)
            {
                address = string.Format("{0}{1}/{2}.json?ns={3}-{4}-rtdb&auth={5}", LocalhostAddress, FirebaseClient.Config.RealtimeDBEmultorport, path, FirebaseClient.Config.ProjectID, database, FirebaseClient.User.IdToken);
            }

            return address;
        }
    }
}
