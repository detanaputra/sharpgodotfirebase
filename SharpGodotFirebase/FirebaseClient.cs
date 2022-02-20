using Godot;

using SharpGodotFirebase.Authentications;
using SharpGodotFirebase.Firestore;
using SharpGodotFirebase.Realtime;
using SharpGodotFirebase.Utilities;

using System;
using System.Threading.Tasks;

namespace SharpGodotFirebase
{
    /// <summary>
    /// SharpGodotFirebase entry client
    /// </summary>
    public class FirebaseClient
    {
        public static void Initialize(SetupConfiguration config)
        {
            Config = config;
            
            HttpRequest = new HTTPRequest();
            HttpRequest.Name = "HttpRequest";
            Config.ParentNode.AddChild(HttpRequest);
            
            DataPersister.Build().Load();
            
            Authentication.Initialize(HttpRequest);
            FirestoreDB.Initialize(HttpRequest);
            RealtimeDB.Initialize(HttpRequest);

            
            IsInitialized = true;
        }

        public static FirebaseUser User
        {
            get => Authentication.User;
        }

        internal static SetupConfiguration Config;
        private static HTTPRequest HttpRequest;
        private static bool IsInitialized = false;

        /// <summary>
        /// Signin with email and password already registered in Firebase project.
        /// </summary>
        /// <param name="email">user email. This email is not validated internally, Your app should validate it before passing into this method</param>
        /// <param name="pass">user password. This is literal string</param>
        /// <returns>AuthResult object that hold Result, Response Code, User, and AuthError object.</returns>
        public static async Task<AuthResult> SigninWithPassword(string email, string pass)
        {
            EnsureInit();
            return await new Authentication().SigninWithPassword(email, pass);
        }

        public static async Task<AuthResult> RefreshUserToken(FirebaseUser user)
        {
            EnsureInit();
            return await new Authentication().RefreshUserIdToken(user);
        }

        public static string GetIdToken()
        {
            return new Authentication().GetIdToken();
        }

        /*public static async Task<string> GetUserIdToken(bool forceRefresh)
        {
            EnsureInit();

        }*/

        /// <summary>
        /// Signin Anonymously to Firebase Project. Please note that although it is named Signin, You should treat it like Signup process<br /> because Firebase generate different User UID whenever User invoke this method, even it's the same user.
        /// </summary>
        /// <returns>AuthResult object that hold Result, Response Code, User, and AuthError object.</returns>
        public static async Task<AuthResult> SigninAnonymous()
        {
            EnsureInit();
            return await new Authentication().SigninAnonymous();
        }

        /// <summary>
        /// Get all user data information. Return null if operation failed.
        /// </summary>
        /// <param name="idToken">Firebase id token</param>
        /// <returns>Userdata object</returns>
        public static async Task<Userdata> GetUserData(string idToken)
        {
            EnsureInit();
            return await new Authentication().GetUserData(idToken);
        }

        public static async Task<AuthResult> SigninOAuth(string providerId = ProviderId.GOOGLE, string successUrl = "https://google.com")
        {
            EnsureInit();
            return await new Authentication().SigninOAuth(providerId, successUrl);
        }

        public static async Task<AuthResult> LinkAccountWithOAuth(FirebaseUser user, string providerId = ProviderId.GOOGLE, string successUrl = "https://google.com")
        {
            EnsureInit();
            return await new Authentication().LinkAccountWithOAuth(user, providerId, successUrl);
        }

        /// <summary>
        /// Signout the user from this application.
        /// </summary>
        /// <returns>Return true if the operation is complete</returns>
        public static async Task<bool> Signout()
        {
            EnsureInit();
            return await new Authentication().Signout();
        }

        /// <summary>
        /// Delete user account on Firebase Project. This method delete user immediately so You have to ask confirmation from user first.
        /// </summary>
        /// <param name="user">Firebase user</param>
        /// <returns>AuthResult object that hold Result, Response Code, User, and AuthError object.</returns>
        public static async Task<AuthResult> DeleteAccount(FirebaseUser user)
        {
            return await new Authentication().DeleteAccount(user);
        }

        public static async Task<RealtimeResult<T>> GetRealtimeDocument<T>(ChildQuery query, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDB().GetDocument<T>(query, database);
        }

        public static async Task<RealtimeResult<T>> PutRealtimeDocument<T>(ChildQuery query, T data, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDB().PutDocument<T>(query, data, database);
        }

        public static async Task<RealtimeResult<T>> PostRealtimeDocument<T>(ChildQuery query, T data, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDB().PostDocument<T>(query, data, database);
        }

        public static async Task<FirestoreResult<T>> GetDocument<T>(DocumentReference documentReference)
        {
            EnsureInit();
            return await new FirestoreDB().GetDocument<T>(documentReference);
        }

        private static void EnsureInit()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Firebase Wrapper hasn't initialized yet. Call Initialize(string webApiKey, string projectId) first");
            }
        }


    }
}
