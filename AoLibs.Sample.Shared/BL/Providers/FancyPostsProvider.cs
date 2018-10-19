using System;
using System.Linq;
using System.Threading.Tasks;
using AoLibs.Sample.Shared.Interfaces;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.Models.HttpCommunication;

namespace AoLibs.Sample.Shared.BL.Providers
{
    public class FancyPostsProvider : IFancyPostsProvider
    {
        private readonly IFancyPostsController _postsController;

        public FancyPostsProvider(IFancyPostsController controller)
        {
            _postsController = controller;
        }

        public async Task<FancyCommentedPost> GetCommentedPost()
        {
            var posts = await _postsController.GetAll();
            var post = posts.First();
            var comments = _postsController.GetComments(post.Id);
            var comment = posts.First();

            return new FancyCommentedPost()
            {
                Title = post.Title,
                Body = post.Body,
                Comment = comment.Body
            };
        }
    }
}
