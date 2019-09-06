using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NewsSystem.ViewModels;
using Newtonsoft.Json.Linq;
using NewsSystem.Common;

namespace NewsSystem.Services.Data
{
    public class FacebookPage : IFacebookPage
    {
        readonly string _accessToken;
        readonly string _pageID;
        readonly string _facebookAPI = "https://graph.facebook.com/";
        readonly string _pageEdgeFeed = "feed";
        readonly string _pageEdgePhotos = "photos";
        readonly string _postToPageURL;
        readonly string _postToPagePhotosURL;

        public FacebookPage()
        {
            _accessToken = "EAAeGhHPkizUBALO0kiJYZCXsNaToZAafchDRXZCFozfOxW7V6ZAJq8ZBSTYvxRydfVBEnFiV2ceAIBdCekQy8CHVxPSRn3vCQ57gRcs5fC9P3dy1wm8aOhFlN2zpr0ctEkhaZAqVERrPzXlqHXD9hr9ZCtQqXhrqWVSVg6qiPYHmX5neHF1LoDV";
            _pageID = "105628697487098";
            _postToPageURL = $"{_facebookAPI}{_pageID}/{_pageEdgeFeed}";
            _postToPagePhotosURL = $"{_facebookAPI}{_pageID}/{_pageEdgePhotos}";
        }

        public async Task<Tuple<int, string>> PublishSimplePost(string postText)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                    { "access_token", _accessToken },
                    { "message", postText }//,
                    // { "formatting", "MARKDOWN" } // doesn't work
                };

                var httpResponse = await http.PostAsync(
                    _postToPageURL,
                    new FormUrlEncodedContent(postData)
                );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
            
                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                );
            }
        }

        public string PublishToFacebook(NewsToFacebookViewMode model)
    {
        try
        {
            // upload picture first
            var rezImage = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    return await UploadPhoto(model.ImageUrl);
                }
            });
            var rezImageJson = JObject.Parse(rezImage.Result.Item2);
            
            if (rezImage.Result.Item1 != 200)
            {
                try // return error from JSON
                {
                    return $"Error uploading photo to Facebook. {rezImageJson["error"]["message"].Value<string>()}";
                }
                catch (Exception ex) // return unknown error
                {
                    // log exception somewhere
                    return $"Unknown error uploading photo to Facebook. {ex.Message}";
                }
            }
            // get post ID from the response
            string postID = rezImageJson["post_id"].Value<string>();
            
            // and update this post (which is actually a photo) with your text
            var rezText = Task.Run(async () =>
            {
                using (var http = new HttpClient())
                {
                    return await UpdatePhotoWithPost(postID, model);
                }
            });
            var rezTextJson = JObject.Parse(rezText.Result.Item2);
            
            if (rezText.Result.Item1 != 200)
            {
                try // return error from JSON
                {
                    return $"Error posting to Facebook. {rezTextJson["error"]["message"].Value<string>()}";
                }
                catch (Exception ex) // return unknown error
                {
                    // log exception somewhere
                    return $"Unknown error posting to Facebook. {ex.Message}";
                }
            }

            return "OK";
        }
        catch (Exception ex)
        {
            // log exception somewhere
            return $"Unknown error publishing post to Facebook. {ex.Message}";
        }
    }

        public async Task<Tuple<int, string>> UploadPhoto(string photoURL)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                    { "access_token", _accessToken },
                    { "url", photoURL }
                };

                var httpResponse = await http.PostAsync(
                    _postToPagePhotosURL,
                    new FormUrlEncodedContent(postData)
                );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
            
                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                );
            }
        }

        public async Task<Tuple<int, string>> UpdatePhotoWithPost(string postID, NewsToFacebookViewMode model)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                    { "access_token", _accessToken },
                    { "message", model.GetShortContent(255) },
                    {"link", GlobalConstants.TempUrl+model.Url}//,
                    // { "formatting", "MARKDOWN" } // doesn't work
                };

                var httpResponse = await http.PostAsync(
                    $"{_facebookAPI}{postID}",
                    new FormUrlEncodedContent(postData)
                );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
            
                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                );
            }
        }
    }
}
