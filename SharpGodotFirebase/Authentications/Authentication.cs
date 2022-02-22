using Godot;

using Newtonsoft.Json;

using SharpGodotFirebase.Utilities;

using SignalStringProvider;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpGodotFirebase.Authentications
{
    internal class Authentication : FirebaseRequester
    {
        [Signal] internal delegate void OAuthProcessComplete(AuthResultSignalWrapper result);

        private static readonly Dictionary<string, string> googleAuthBody = new Dictionary<string, string>()
        {
            { "scope", "email openid profile" },
            { "response_type","code" },
            { "redirect_uri","" },
            { "client_id","[CLIENT_ID]" }
        };

        private static readonly Dictionary<string, string> googleTokenBody = new Dictionary<string, string>()
        {
            {"code", "" },
            {"client_id", "" },
            {"client_secret", ""},
            {"redirect_uri", "" },
            {"grant_type","authorization_code" }
        };

        internal static FirebaseUser User;

        private static Authentication authenticationNode;
        private static TCP_Server TCPServer;
        private static Timer TCPTimerNode;
        private static HTTPRequest httpRequest;

        private enum OAuthAction { Signin, Link, Unlink }
        private static OAuthAction currentOAuthAction = OAuthAction.Signin;

        internal static void Initialize(HTTPRequest hTTPRequest)
        {
            httpRequest = hTTPRequest;

            authenticationNode = new Authentication
            {
                Name = "Authentication"
            };
            FirebaseClient.Config.ParentNode.AddChild(authenticationNode);

            TCPServer = new TCP_Server();
            TCPTimerNode = new Timer
            {
                Name = "AuthTCPTimer"
            };
            authenticationNode.AddChild(TCPTimerNode);
            TCPTimerNode.Connect(SignalString.Timeout, authenticationNode, nameof(OnTCPTimerTimeout));

            LoadCachedFirebaseUserOrRefresh();
        }

        internal async Task<Userdata> GetUserData(string _idToken)
        {
            string address = UrlBuilder.GetUserdataUrl();
            var body = new
            {
                idToken = _idToken
            };
            string content = JsonConvert.SerializeObject(body);

            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            if (requestResult.EnsureSuccess())
            {
                return JsonConvert.DeserializeObject<Userdata>(requestResult.Body);
            }

            return null;
        }

        internal async Task<AuthResult> SignupWithEmailAndPassword(string email, string password)
        {
            string address = UrlBuilder.GetSignupWithEmailAndPasswordUrl();
            var body = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            AuthResult authResult = new AuthResult(requestResult);
            if (authResult.EnsureSuccess())
            {
                Logger.Log("Signup with email and password success");
                User = JsonConvert.DeserializeObject<FirebaseUser>(authResult.Body);
                authResult.User = User;
                long expiresAt = IdTokenManager.GetExpiresAt(User.ExpiresIn);
                DataPersister.Build().AddData(IdTokenManager.FirebaseUserIdTokenExpiresAt, expiresAt)
                    .AddDataAndSave(nameof(FirebaseUser), User);
            }
            else
            {
                if (string.IsNullOrEmpty(authResult.Body))
                {
                    authResult.AuthError = AuthError.GenerateError();
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                }
                Logger.LogErr("Signup with email and password ", authResult.AuthError.Error.Message);
            }
            return authResult;
        }

        internal async Task<AuthResult> SendEmailVerification(string idToken, string locale = "en")
        {
            string address = UrlBuilder.GetSendEmailVerificationUrl();
            var body = new
            {
                requestType = "VERIFY_EMAIL",
                idToken
            };
            string content = JsonConvert.SerializeObject(body);
            string[] header = new string[3]
            {
                "Content-Type: application/json",
                "Accept: application/json",
                string.Format("X-Firebase-Locale : {0}", locale)
            };
            IRequestResult requestResult = await SendRequest(httpRequest, address, content, header);
            AuthResult authResult = new AuthResult(requestResult);
            if (authResult.EnsureSuccess())
            {
                Logger.Log("Sending email verification success");
                authResult.User = User;
            }
            else
            {
                if (string.IsNullOrEmpty(authResult.Body))
                {
                    authResult.AuthError = AuthError.GenerateError();
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                }
                Logger.LogErr("Sending email verification ", authResult.AuthError.Error.Message);
            }
            return authResult;
        }

        internal async Task<Userdata> ConfirmEmailVerification(string oobCode)
        {
            string address = UrlBuilder.GetConfirmEmailUrl();
            var body = new
            {
                oobCode,
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            if (requestResult.EnsureSuccess())
            {
                return JsonConvert.DeserializeObject<Userdata>(requestResult.Body);
            }
            return null;

        }

        internal async Task<AuthResult> SendPasswordResetEmail(string email, string locale = "en")
        {
            string address = UrlBuilder.GetSendPasswordResetEmailUrl();
            var body = new
            {
                requestType = "PASSWORD_RESET",
                email
            };
            string[] header = new string[3]
            {
                "Content-Type: application/json",
                "Accept: application/json",
                string.Format("X-Firebase-Locale : {0}", locale)
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(httpRequest, address, content, header);
            AuthResult authResult = new AuthResult(requestResult);
            if (authResult.EnsureSuccess())
            {
                Logger.Log("Sending password reset email success");
                authResult.User = User;
            }
            else
            {
                if (string.IsNullOrEmpty(authResult.Body))
                {
                    authResult.AuthError = AuthError.GenerateError();
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                }
                Logger.LogErr("Sending password reset email failed, ", authResult.AuthError.Error.Message);
            }
            return authResult;
        }

        internal async Task<AuthResult> VerifyPasswordResetCode(string oobCode)
        {
            string address = UrlBuilder.GetVerifyPasswordResetCodeUrl();
            var body = new
            {
                oobCode
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            AuthResult authResult = new AuthResult(requestResult);
            if (authResult.EnsureSuccess())
            {
                Logger.Log("Verify password reset email success");
                authResult.User = User;
            }
            else
            {
                if (string.IsNullOrEmpty(authResult.Body))
                {
                    authResult.AuthError = AuthError.GenerateError();
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                }
                Logger.LogErr("Verify password reset email failed, ", authResult.AuthError.Error.Message);
            }
            return authResult;
        }

        internal async Task<AuthResult> ConfirmPasswordReset(string oobCode, string newPassword)
        {
            string address = UrlBuilder.GetConfirmPasswordResetUrl();
            var body = new
            {
                oobCode,
                newPassword
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            AuthResult authResult = new AuthResult(requestResult);
            if (authResult.EnsureSuccess())
            {
                Logger.Log("Confirm update password success");
                authResult.User = User;
            }
            else
            {
                if (string.IsNullOrEmpty(authResult.Body))
                {
                    authResult.AuthError = AuthError.GenerateError();
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                }
                Logger.LogErr("Confirm update password failed, ", authResult.AuthError.Error.Message);
            }
            return authResult;
        }

        internal async Task<AuthResult> SigninWithPassword(string username, string pass)
        {
            string address = UrlBuilder.GetSigninWithPasswordUrl();
            var body = new
            {
                email = username,
                password = pass,
                returnSecureToken = true
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            AuthResult authResult = new AuthResult(requestResult);
            if (authResult.EnsureSuccess())
            {
                Logger.Log("User data received");
                User = JsonConvert.DeserializeObject<FirebaseUser>(authResult.Body);
                authResult.User = User;
                long expiresAt = IdTokenManager.GetExpiresAt(User.ExpiresIn);
                DataPersister.Build().AddData(IdTokenManager.FirebaseUserIdTokenExpiresAt, expiresAt)
                    .AddDataAndSave(nameof(FirebaseUser), User);
            }
            else
            {
                if (string.IsNullOrEmpty(authResult.Body))
                {
                    authResult.AuthError = AuthError.GenerateError();
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                }
                Logger.LogErr("Signin With Password ", authResult.AuthError.Error.Message);
            }
            return authResult;
        }

        internal async Task<AuthResult> SigninAnonymous()
        {
            string address = UrlBuilder.GetSigninAnonymousUrl();
            var body = new
            {
                returnSecureToken = true
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            AuthResult authResult = new AuthResult(requestResult);
            if (authResult.EnsureSuccess())
            {
                Logger.Log("User data received");
                User = JsonConvert.DeserializeObject<FirebaseUser>(authResult.Body);
                authResult.User = User;
                long expiresAt = IdTokenManager.GetExpiresAt(User.ExpiresIn);
                DataPersister.Build().AddData(IdTokenManager.FirebaseUserIdTokenExpiresAt, expiresAt)
                    .AddDataAndSave(nameof(FirebaseUser), User);
            }
            else
            {
                if (string.IsNullOrEmpty(authResult.Body))
                {
                    authResult.AuthError = AuthError.GenerateError();
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                }
                Logger.LogErr("Signin Anonymous ", authResult.AuthError.Error.Message);
            }
            return authResult;
        }

        internal async Task<AuthResult> RefreshUserIdToken(FirebaseUser firebaseUser)
        {
            User = firebaseUser;
            string address = UrlBuilder.GetRefreshTokenUrl();
            var body = new
            {
                grant_type = "refresh_token",
                refresh_token = User.RefreshToken
            };

            string content = JsonConvert.SerializeObject(body);

            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            AuthResult authResult = new AuthResult(requestResult);
            if (authResult.EnsureSuccess())
            {
                Logger.Log("IdToken is refreshed");
                RefreshTokenResponsePayload payload = JsonConvert.DeserializeObject<RefreshTokenResponsePayload>(authResult.Body);
                User.IdToken = payload.IdToken;
                User.RefreshToken = payload.RefreshToken;
                User.ExpiresIn = payload.ExpiresIn;

                authResult.User = User;
                long expiresAt = IdTokenManager.GetExpiresAt(User.ExpiresIn);
                DataPersister.Build().AddData(IdTokenManager.FirebaseUserIdTokenExpiresAt, expiresAt)
                    .AddDataAndSave(nameof(FirebaseUser), User);
            }
            else
            {
                if (string.IsNullOrEmpty(authResult.Body))
                {
                    authResult.AuthError = AuthError.GenerateError();
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                }
                Logger.LogErr("Refresh IdToken ", authResult.AuthError.Error.Message);
            }

            return authResult;
        }

        internal string GetIdToken()
        {
            LoadCachedFirebaseUserOrRefresh();
            return User.IdToken;
        }

        internal async Task<AuthResult> DeleteAccount(FirebaseUser user)
        {
            string address = UrlBuilder.GetDeleteAccountUrl();
            var body = new
            {
                idToken = user.IdToken
            };
            string content = JsonConvert.SerializeObject(body);

            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            AuthResult authResult = new AuthResult(requestResult);
            return authResult;
        }

        internal async Task<bool> Signout()
        {
            User = null;
            return await Task.FromResult<bool>(true);
        }

        internal async Task<AuthResult> SigninOAuth(string _providerId, string successUrl)
        {
            currentOAuthAction = OAuthAction.Signin;
            OpenOAuthPage(_providerId, "http://127.0.0.1:[PORT]/", 4195);
            object[] signalResult = await ToSignal(authenticationNode, nameof(OAuthProcessComplete));
            OS.ShellOpen(successUrl);
            if (signalResult[0] is AuthResultSignalWrapper s)
            {
                return s.AuthResult;
            }
            return new AuthResult()
            {
                Result = 4,
                ResponseCode = -1,
                AuthError = AuthError.GenerateError(4)
            };
        }

        internal async Task<AuthResult> LinkAccountWithOAuth(FirebaseUser user, string providerId, string successUrl)
        {
            User = user;
            currentOAuthAction = OAuthAction.Link;
            OpenOAuthPage(providerId, "http://127.0.0.1:[PORT]/", 4195);
            object[] signalResult = await ToSignal(authenticationNode, nameof(OAuthProcessComplete));
            OS.ShellOpen(successUrl);
            if (signalResult[0] is AuthResultSignalWrapper s)
            {
                return s.AuthResult;
            }
            return new AuthResult()
            {
                Result = 4,
                ResponseCode = -1,
                AuthError = AuthError.GenerateError(4)
            };
        }

        protected override Task<IRequestResult> SendRequest(HTTPRequest httpRequest, string address, string content = "", string[] header = null, HTTPClient.Method method = HTTPClient.Method.Post)
        {
            string[] customHeader = header;
            if (customHeader == null)
            {
                customHeader = new string[2]
                {
                    "Content-Type: application/json",
                    "Accept: application/json"
                };
            }

            return base.SendRequest(httpRequest, address, content, customHeader, method);
        }

        private void OpenOAuthPage(string _providerId, string _redirectUri, ushort _listenToPort = 4195)
        {
            string redirectUri = _redirectUri.Replace("[PORT]", _listenToPort.ToString());
            string urlEndpoint = UrlBuilder.GetOAuthRequestUrl(_providerId);

            googleAuthBody["redirect_uri"] = redirectUri;
            foreach (KeyValuePair<string, string> pair in googleAuthBody)
            {
                urlEndpoint += pair.Key + "=" + pair.Value + "&";
            }

            if (string.IsNullOrEmpty(FirebaseClient.Config.ClientId))
            {
                Logger.LogErr("ClientId is empty. Please set it with Firebase ClientId on initialization process");
                return;
            }
            urlEndpoint = urlEndpoint.Replace("[CLIENT_ID]&", FirebaseClient.Config.ClientId);

            OS.ShellOpen(urlEndpoint);
            TCPTimerNode.Start();
            _ = TCPServer.Listen(_listenToPort, "*");
        }

        private void OnTCPTimerTimeout()
        {
            StreamPeerTCP peer = TCPServer.TakeConnection();
            if (peer != null)
            {
                string rawResult = peer.GetUtf8String(100);
                if (rawResult != "" & rawResult.BeginsWith("GET"))
                {
                    string code = rawResult.Split("=")[1].RStrip("&scope");
                    TCPServer.Stop();
                    peer.DisconnectFromHost();
                    TCPTimerNode.Stop();
                    SigninWithOAuth(code, googleAuthBody["redirect_uri"]);
                }
            }
        }

        private async void SigninWithOAuth(string signinCode, string requestUri = "urn:ietf:wg:oauth:2.0:oob", string providerId = ProviderId.GOOGLE)
        {
            string code = signinCode.PercentDecode();
            AuthResult authResult = await ExchangeGoogleToken(code, requestUri);

            if (authResult.EnsureSuccess())
            {
                GoogleToken googleToken = JsonConvert.DeserializeObject<GoogleToken>(authResult.Body);
                Dictionary<string, object> body = new Dictionary<string, object>()
                {
                    { "postBody", "id_token=" + googleToken.IdToken + "&providerId=" + providerId },
                    { "requestUri", requestUri },
                    { "returnIdpCredential", true },
                    { "returnSecureToken", true }
                };

                if (providerId == ProviderId.FACEBOOK)
                {
                    // TODO Facebook
                    body["postBody"] = "access_token=" + "" + "&providerId=" + providerId;
                }

                if (currentOAuthAction == OAuthAction.Link)
                {
                    body.Add("idToken", User.IdToken);
                }

                string address = UrlBuilder.GetSigninOAuthUrl(providerId);
                string content = JsonConvert.SerializeObject(body);
                IRequestResult requestResult = await SendRequest(httpRequest, address, content);
                authResult = new AuthResult(requestResult);
                if (authResult.EnsureSuccess())
                {
                    Logger.Log("User data received");
                    OAuthFirebaseUser oAuthuser = JsonConvert.DeserializeObject<OAuthFirebaseUser>(authResult.Body);
                    User = oAuthuser;
                    authResult.User = User;
                    long expiresAt = IdTokenManager.GetExpiresAt(User.ExpiresIn);
                    DataPersister.Build().AddData(IdTokenManager.FirebaseUserIdTokenExpiresAt, expiresAt)
                        .AddDataAndSave(nameof(FirebaseUser), User);
                }
                else
                {
                    authResult.AuthError = JsonConvert.DeserializeObject<AuthError>(authResult.Body);
                    Logger.LogErr("Signin With OAuth ", authResult.AuthError.Error.Message);
                }
                EmitSignal(nameof(OAuthProcessComplete), new AuthResultSignalWrapper() { AuthResult = authResult });
            }
        }

        private async Task<AuthResult> ExchangeGoogleToken(string signinCode, string redirectUri = "urn:ietf:wg:oauth:2.0:oob")
        {
            googleTokenBody["code"] = signinCode;
            googleTokenBody["client_id"] = FirebaseClient.Config.ClientId;
            googleTokenBody["client_secret"] = FirebaseClient.Config.ClientSecret;
            googleTokenBody["redirect_uri"] = redirectUri;
            string content = JsonConvert.SerializeObject(googleTokenBody);
            string address = UrlBuilder.GetExchangeGoogleTokenRequestUrl();
            IRequestResult requestResult = await SendRequest(httpRequest, address, content);
            return new AuthResult(requestResult);
        }

        private static async void LoadCachedFirebaseUserOrRefresh()
        {
            FirebaseUser user = DataPersister.Build().GetValue<FirebaseUser>(nameof(FirebaseUser));
            if (IdTokenManager.IsExpired())
            {
                await new Authentication().RefreshUserIdToken(user);
            }
            else
            {
                User = user;
            }
        }
    }
}
