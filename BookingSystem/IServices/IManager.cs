using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.IServices
{
    public interface IManager
    {
        Task<SearchRes> Search(SearchReq searchReq);
        Task<BookRes> Book(BookReq bookReq);
        Task<CheckStatusRes> CheckStatus(CheckStatusReq checkStatusReq);
    }
}
