using Models.Request.Book;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IBookService
    {
        Task<ServiceResponse> AddEditBook(bookRequest obj, int iUserID);
        Task<ServiceResponse> AddEditChapter(chapterRequest obj, int iUserID);
        Task<ServiceResponse> AddEditContent(ContentRequest obj, int iUserID);
        Task<ServiceResponse> BookList(int iBookID);
        Task<ServiceResponse> ChapterList(int iChapterID);
        Task<ServiceResponse> ContentList(int iContentID);
        Task<ServiceResponse> ContentListByChapterID(int iContentID);
        Task<ServiceResponse> GetBookViewData();
        Task<ServiceResponse> ChapterListByBook(int iBookID);
    }
}
