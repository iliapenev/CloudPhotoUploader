using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
//using System.Diagnostics;
using Spring.Social.OAuth1;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.IO;
using System.Diagnostics;

namespace CloudPhotoUpload
{
    public partial class _Default : System.Web.UI.Page
    {
        private const string DropboxAppKey = "YourKey";
        private const string DropboxAppSecret = "YourSecret";


        private DropboxServiceProvider dropboxServiceProvider =
            new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);


        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Request.Cookies["requestTokenValue"] == null) || (Request.Cookies["requestTokenSecret"] == null))
            {
                DropboxAuth();
            }

            if ((Request.Cookies["TokenValue"] == null) || (Request.Cookies["TokenSecret"] == null))
            {
                OAuthToken requestToken = new OAuthToken(Request.Cookies["requestTokenValue"].Value, Request.Cookies["requestTokenSecret"].Value);
                AuthorizedRequestToken authorizedRequestToken = new AuthorizedRequestToken(requestToken, null);
                OAuthToken token = dropboxServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(authorizedRequestToken, null).Result;
                Response.Cookies["TokenValue"].Value = token.Value;
                Response.Cookies["TokenSecret"].Value = token.Secret;
            }
        }

        protected void UploadPhotos_Click(object sender, EventArgs e)
        {
            string myDirectory = FolderPath.Text.ToString();
            string[] directories = Directory.GetFiles(myDirectory);

            IDropbox dropbox = dropboxServiceProvider.GetApi(Request.Cookies["TokenValue"].Value, Request.Cookies["TokenSecret"].Value);
            string newFolderName = AlbumName.Text;
            try
            {
                Entry createFolderEntry = dropbox.CreateFolderAsync(newFolderName).Result;
               
                foreach (var file in directories)
                {
                    int fileNamePosition = file.LastIndexOf("\\") + 1;
                    string path = file.Substring(fileNamePosition, file.Length - fileNamePosition);
                    Entry uploadFileEntry = dropbox.UploadFileAsync(
                      new FileResource(file), "/" + newFolderName + "/" + path).Result;
                }
                DropboxLink link = dropbox.GetShareableLinkAsync(newFolderName).Result;
                Response.Redirect(link.Url);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex is DropboxApiException)
                    {
                        ErrorLabel.Text = ex.Message;
                        return true;
                    }
                    return false;
                });
            }
        }

        private void DropboxAuth()
        {
            try
            {
                OAuthToken requestToken = dropboxServiceProvider.OAuthOperations.FetchRequestTokenAsync(null, null).Result;
                Response.Cookies["requestTokenValue"].Value = requestToken.Value;
                Response.Cookies["requestTokenSecret"].Value = requestToken.Secret;
                OAuth1Parameters parameters = new OAuth1Parameters();
                parameters.CallbackUrl = Request.Url.ToString();
                string authorizeUrl = dropboxServiceProvider.OAuthOperations.BuildAuthenticateUrl(requestToken.Value, parameters);
                Response.Redirect(authorizeUrl);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                    {
                        if (ex is DropboxApiException)
                        {
                            ErrorLabel.Text = ex.Message;
                            return true;
                        }
                        return false;
                    });
            }
        }

        protected void FolderPath_TextChanged(object sender, EventArgs e)
        {
            string myDirectory = FolderPath.Text.ToString();
            string[] directories = Directory.GetFiles(myDirectory);
            FileList.DataSource = directories;
            FileList.DataBind();
            FileList.Height = 250;
        }
    }
}


