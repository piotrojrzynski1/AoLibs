using System;
using System.Threading.Tasks;
using AoLibs.Sample.Shared.Models;

namespace AoLibs.Sample.Shared.Interfaces
{
    public interface IFancyPostsProvider
    {
        Task<FancyCommentedPost> GetCommentedPost();
    }
}
