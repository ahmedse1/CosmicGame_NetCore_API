using Models.Request.Book;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBookRepo
    {

        Task<(int, bool)> AddEditBook(bookRequest obj);
        Task<(int, bool)> AddEditChapter(chapterRequest obj);
        Task<(int, bool)> AddEditContent(ContentRequest obj);
        Task<IEnumerable<bookResponse>> BookList(int iBookID);
        Task<IEnumerable<chapterResponse>> ChapterList(int iChapterID);
        Task<IEnumerable<ContentResponse>> ContentList(int iContentID);
        Task<IEnumerable<ContentResponse>> ContentListByChapterID(int iContentID);
        Task<dynamic> GetBookViewData();
        Task<IEnumerable<chapterResponse>> ChapterListByBook(int iBookID);

    }
}
