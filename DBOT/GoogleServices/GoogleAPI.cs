using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace DBOT.GoogleServices
{
    public static class GoogleAPI
    {
        /// <summary>
        /// Used to auth in google services.
        /// </summary>
        public static void Auth(string credentialPath, string userDataPath, string[] scopes, out UserCredential userCredential)
        {
            using (FileStream fileStream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromStream(fileStream).Secrets, scopes, "user", CancellationToken.None, new FileDataStore(userDataPath, true)).Result;
            }
        }
    }
}
