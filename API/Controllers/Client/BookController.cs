using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Request.Book;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Client
{
    [Route("Client/[controller]")]
    [ApiController]
    public class BookController : BaseController
    {
        IBookService _IBSercice;
        public BookController(IHttpContextAccessor httpContextAccessor, IBookService IBSercice) : base(httpContextAccessor)
        {
            _IBSercice = IBSercice;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("AddEditBook")]
        public async Task<IActionResult> AddEditBook([Microsoft.AspNetCore.Mvc.FromBody] bookRequest obj)
        {
            var Response = await _IBSercice.AddEditBook(obj, UserId);
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("AddEditChapter")]
        public async Task<IActionResult> AddEditChapter([Microsoft.AspNetCore.Mvc.FromBody] chapterRequest obj)
        {
            var Response = await _IBSercice.AddEditChapter(obj, UserId);
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("AddEditContent")]
        public async Task<IActionResult> AddEditContent([Microsoft.AspNetCore.Mvc.FromBody] ContentRequest obj)
        {
            var Response = await _IBSercice.AddEditContent(obj, UserId);
            return Ok(Response);
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("BookList")]
        public async Task<IActionResult> BookList()
        {
            var Response = await _IBSercice.BookList(0);
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("ChapterList")]
        public async Task<IActionResult> ChapterList([Microsoft.AspNetCore.Mvc.FromBody] int strBookID)
        {
            var Response = await _IBSercice.ChapterList(strBookID);
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("ChapterListByBook")]
        public async Task<IActionResult> ChapterListByBook([Microsoft.AspNetCore.Mvc.FromBody] int strBookID)
        {
            var Response = await _IBSercice.ChapterListByBook(strBookID);
            return Ok(Response);
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("ContentList")]
        public async Task<IActionResult> ContentList([Microsoft.AspNetCore.Mvc.FromBody] object strChapterID)
        {
            var Response = await _IBSercice.ContentList(Convert.ToInt32(strChapterID.ToString()));
            return Ok(Response);
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("ContentListByChapterID")]
        public async Task<IActionResult> ContentListByChapterID([Microsoft.AspNetCore.Mvc.FromBody] object strChapterID)
        {
            var Response = await _IBSercice.ContentListByChapterID(Convert.ToInt32(strChapterID.ToString()));
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("LoadBook")]
        public async Task<IActionResult> GetBookViewData()
        {
            var Response = await _IBSercice.GetBookViewData();
            return Ok(Response);
        }
    }
}
