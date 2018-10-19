using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AoLibs.HttpHelper;
using AoLibs.HttpHelper.Interfaces;
using AoLibs.Sample.Shared.Interfaces;
using AoLibs.Sample.Shared.Models.HttpCommunication;

namespace AoLibs.Sample.Shared.BL.HttpCommunication.ApiControllers
{
    public class FancyPostsController : ApiControllerBase, IFancyPostsController
    {
        protected override IApiDefinition ApiDefinition { get; } = new FancyApiDefinition();
        protected override string ControllerPrefix { get; } = "posts";

        public async Task<List<FancyPostModel>> GetAll()
        {
            return await Get<List<FancyPostModel>>("");
        }

        public async Task<FancyPostModel> GetById(long id)
        {
            return await Get<FancyPostModel>(id.ToString());
        }

        public async Task<FancyPostCommentModel> GetComments(long id)
        {
            return await Get<FancyPostCommentModel>($"{id}/comments");
        }
    }
}
