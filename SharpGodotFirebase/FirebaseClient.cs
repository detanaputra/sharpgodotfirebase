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
    /// SharpGodotFirebase entry point for all API. You have to initialize it first or it will throw invalid operation exception.
    /// </summary>
    public class FirebaseClient
    {
        internal static SetupConfiguration Config;
        private static HTTPRequest HttpRequest;
        private static bool IsInitialized = false;

        /// <summary>
        /// Initializing SharpGodotFirebase. It is recomended to initialize it once before any user interface come to screen like in game splash screen.
        /// </summary>
        /// <param name="config"></param>
        public static void Initialize(SetupConfiguration config)
        {
            Config = config;

            HttpRequest = new HTTPRequest
            {
                Name = "HttpRequest"
            };
            Config.ParentNode.AddChild(HttpRequest);
            
            DataPersister.Build().Load();
            
            Authentication.Initialize(HttpRequest);
            FirestoreDB.Initialize(HttpRequest);
            RealtimeDB.Initialize(HttpRequest);

            
            IsInitialized = true;
        }

        /// <summary>
        /// Firebase User cached in SharpGodotFirebase.
        /// </summary>
        public static FirebaseUser User
        {
            get => Authentication.User;
        }

        public static async Task<AuthResult> SignupWithEmailAndPassword(string email, string password)
        {
            EnsureInit();
            return await new Authentication().SignupWithEmailAndPassword(email, password);
        }

        public static async Task<AuthResult> SendEmailVerification(string idToken, string locale = "en")
        {
            EnsureInit();
            return await new Authentication().SendEmailVerification(idToken, locale);
        }

        public static async Task<Userdata> ConfirmEmailVerification(string oobCode)
        {
            EnsureInit();
            return await new Authentication().ConfirmEmailVerification(oobCode);
        }

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

        /// <summary>
        /// Method to manually Refresh Firebase User Token.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Return AuthResult object that contain newly refreshed FirebaseUser object</returns>
        public static async Task<AuthResult> RefreshUserToken(FirebaseUser user)
        {
            EnsureInit();
            return await new Authentication().RefreshUserIdToken(user);
        }

        /// <summary>
        /// Method to manually get Firebase IdToken. It will return the cached one if it is not expired yet. It will automatically call RefreshUserToken <br />
        /// and return new refresh token when it is already expired. Although FirebaseUser already contains IdToken, it is recomended to call this method to automatically get the newly refresh one.
        /// </summary>
        /// <returns>Firebase Id Token</returns>
        public static string GetIdToken()
        {
            EnsureInit();
            return new Authentication().GetIdToken();
        }

        /// <summary>
        /// Signin Anonymously to Firebase Project. Please note that although it is named Signin, You should treat it like Signup process<br /> because Firebase generate new User LocalId whenever User invoke this method, even it's the same user.
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

        /// <summary>
        /// Signin with OAuth request. Currently only support google account. <br />
        /// You should provide your own success Url to tell the Signin process is complete and tell user to close the browser.
        /// </summary>
        /// <param name="providerId">Firebase provider id, currently it only support "google.com"</param>
        /// <param name="successUrl">Url redirect browsed when OAuth process already completed. You should provide your own Url.</param>
        /// <returns></returns>
        public static async Task<AuthResult> SigninOAuth(string providerId = ProviderId.GOOGLE, string successUrl = "https://detanaputra.github.io/sharpgodotfirebase/")
        {
            EnsureInit();
            return await new Authentication().SigninOAuth(providerId, successUrl);
        }

        /// <summary>
        /// Method for linking account to OAuth signin process. Currently support account only. Usually it is used to link user anonymous account to OAuth account.
        /// </summary>
        /// <param name="user">FirebaseUser object. usually this is an anonymous FirebaseUser</param>
        /// <param name="providerId">Firebase provider id, currently it only support "google.com"</param>
        /// <param name="successUrl">Url redirect browsed when OAuth process already completed. You should provide your own Url.</param>
        /// <returns></returns>
        public static async Task<AuthResult> LinkAccountWithOAuth(FirebaseUser user, string providerId = ProviderId.GOOGLE, string successUrl = "https://detanaputra.github.io/sharpgodotfirebase/")
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

        /// <summary>
        /// No Documentation yet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static async Task<RealtimeResult<T>> GetRealtimeDocument<T>(ChildQuery query, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDB().GetDocument<T>(query, database);
        }

        /// <summary>
        /// No Documentation yet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="data"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static async Task<RealtimeResult<T>> PutRealtimeDocument<T>(ChildQuery query, T data, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDB().PutDocument<T>(query, data, database);
        }

        /// <summary>
        /// No Documentation yet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="data"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static async Task<RealtimeResult<T>> PostRealtimeDocument<T>(ChildQuery query, T data, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDB().PostDocument<T>(query, data, database);
        }

        private static void EnsureInit()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("SharpGodotFirebase hasn't initialized yet.");
            }
        }


    }
}
