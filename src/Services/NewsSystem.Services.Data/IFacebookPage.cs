using System;
using System.Threading.Tasks;
using NewsSystem.ViewModels;

namespace NewsSystem.Services.Data
{
    public interface IFacebookPage
    {
        Task<Tuple<int, string>> PublishSimplePost(string postText);

        string PublishToFacebook(NewsToFacebookViewMode model);

        Task<Tuple<int, string>> UploadPhoto(string photoURL);

        Task<Tuple<int, string>> UpdatePhotoWithPost(string postID, NewsToFacebookViewMode model);

    }
}