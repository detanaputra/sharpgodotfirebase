using Godot;

using SharpGodotFirebase.Authentications;
using SharpGodotFirebase.Firestore;
using SharpGodotFirebase.Realtime;
using SharpGodotFirebase.Utilities;

using System;
using System.Collections.Generic;
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
            RealtimeDb.Initialize(HttpRequest);

            
            IsInitialized = true;
        }

        /// <summary>
        /// Firebase User cached in SharpGodotFirebase.
        /// </summary>
        public static FirebaseUser User
        {
            get => Authentication.User;
        }

        /// <summary>
        /// Signup with email and password. After calling this API and get the FirebaseUser object, you should send confirmation email with <see cref="SendEmailVerification(string, string)"/>
        /// </summary>
        /// <param name="email">User's registered email address</param>
        /// <param name="password">User's password</param>
        /// <returns>AuthResult that contain FirebaseUser object. You should treat this user object like anonymous one because the user hasn't verify the email address yet.</returns>
        public static async Task<AuthResult> SignupWithEmailAndPassword(string email, string password)
        {
            EnsureInit();
            return await new Authentication().SignupWithEmailAndPassword(email, password);
        }

        /// <summary>
        /// Send email verification to registered user's email containing out of band code.
        /// </summary>
        /// <param name="idToken">User's generated idToken from FirebaseUser object</param>
        /// <param name="locale">optional parameter that indicate in what languange your Email Confirmation will be. Default is "en"</param>
        /// <returns>AuthResult</returns>
        public static async Task<AuthResult> SendEmailVerification(string idToken, string locale = "en")
        {
            EnsureInit();
            return await new Authentication().SendEmailVerification(idToken, locale);
        }

        /// <summary>
        /// Send the out of band code (oobCode) to Firebase to complete the email verification process. <br />
        /// Mostlikely you don't need to call this API because user usually click the link sent to their email to verify email.
        /// </summary>
        /// <param name="oobCode">Out of band code the user get form verification email</param>
        /// <returns>Userdata object. Please notice that due to response payload of this API from Firebase, the userdata object returned is not complete, <br />
        /// some of the Userdata property will return null or empty string. If you need the complete Userdata object, call <see cref="GetUserData(string)"/> after calling this API.<br/>
        /// This behaviour would likely to change in the future.
        /// </returns>
        public static async Task<Userdata> ConfirmEmailVerification(string oobCode)
        {
            EnsureInit();
            return await new Authentication().ConfirmEmailVerification(oobCode);
        }

        /// <summary>
        /// Send password reset link to user's registered email.
        /// </summary>
        /// <param name="email">User's registered email</param>
        /// <param name="locale">Localization setting, default is en for english</param>
        /// <returns>AuthResult object</returns>
        public static async Task<AuthResult> SendPasswordResetEmail(string email, string locale = "en")
        {
            EnsureInit();
            return await new Authentication().SendPasswordResetEmail(email, locale);
        }

        /// <summary>
        /// Verify password reset code by sending oobcode received by user to Firebase.<br />
        /// Mostlikely you don't need to call this API because usually user click the link sent to their email to update password.
        /// </summary>
        /// <param name="oobCode"></param>
        /// <returns></returns>
        public static async Task<AuthResult> VerifyPasswordResetCode(string oobCode)
        {
            EnsureInit();
            return await new Authentication().VerifyPasswordResetCode(oobCode);
        }

        /// <summary>
        /// Finisih password reset sequence by sending oobcode and new password to Firebase.<br />
        /// Mostlikely you don't need to call this API because usually user click the link sent to their email to update password.
        /// </summary>
        /// <param name="oobCode"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static async Task<AuthResult> ConfirmPasswordReset(string oobCode, string newPassword)
        {
            EnsureInit();
            return await new Authentication().ConfirmPasswordReset(oobCode, newPassword);
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
        /// Delete user account on Firebase Project. This API delete user immediately so your game should handle the confirmation process to User.
        /// </summary>
        /// <param name="user">Firebase user</param>
        /// <returns>AuthResult object that hold Result, Response Code, User, and AuthError object. the FirebaseUser object returned will be null</returns>
        public static async Task<AuthResult> DeleteAccount(FirebaseUser user)
        {
            return await new Authentication().DeleteAccount(user);
        }

        /// <summary>
        /// Get the document from Realtime database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">child query that hold address location in Realtime database json tree</param>
        /// <param name="database">Your database name. Default is "default"</param>
        /// <returns>RealtimeResult object that hold RealtimeDbDocument property which have Value with type of <typeparamref name="T"/>. <br />
        /// Data property is null if the request is succesfully made but no document is found.
        /// </returns>
        public static async Task<RealtimeResult<T>> GetRealtimeDocument<T>(ChildQuery query, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDb().GetDocument<T>(query, database);
        }

        /// <summary>
        /// Get all documents from Realtime database collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="database"></param>
        /// <returns>IEnumerable of RealtimeDbDocument by getting Collection property</returns>
        public static async Task<RealtimeResult<T>> GetRealtimeCollection<T>(ChildQuery query, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDb().GetCollection<T>(query, database);
        }

        /// <summary>
        /// Put or overwrite data into a realtime document. ChildQuery object should refer to a Document path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">ChildQuery object that refer to a Document path</param>
        /// <param name="data"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static async Task<RealtimeResult<T>> PutRealtimeDocument<T>(ChildQuery query, T data, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDb().PutDocument<T>(query, data, database);
        }

        /// <summary>
        /// Post data into a realtime collection and generate random string as a key. ChildQuery object should refer to a Collection path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="data"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static async Task<RealtimeResult<T>> PostRealtimeDocument<T>(ChildQuery query, T data, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDb().PostDocument<T>(query, data, database);
        }

        public static async Task<RealtimeResult<Dictionary<string, object>>> PatchRealtimeDocument(ChildQuery query, Dictionary<string, object> data, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDb().PatchDocument(query, data, database);
        }

        public static async Task<RealtimeResult> DeleteDocument(ChildQuery query, string database = "default")
        {
            EnsureInit();
            return await new RealtimeDb().DeleteDocument(query, database);
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
