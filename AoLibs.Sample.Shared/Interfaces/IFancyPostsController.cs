using System.Threading.Tasks;
using System.Collections.Generic;
using AoLibs.Sample.Shared.Models.HttpCommunication;

namespace AoLibs.Sample.Shared.Interfaces
{
    public interface IFancyPostsController
    {
        Task<List<FancyPostModel>> GetAll();
        Task<FancyPostModel> GetById(long id);
        Task<FancyPostCommentModel> GetComments(long id);
    }
}
