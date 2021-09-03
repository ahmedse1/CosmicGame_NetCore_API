using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Request.Book
{
    public class bookResponse
    {
        public string AppBookID { get; set; }
        public string AppBookName { get; set; }
        public string AppAuther { get; set; }
        public bool isAdd { get; set; }
    }

    public class chapterResponse
    {
        public string AppChapterID { get; set; }
        public string AppChapterNo { get; set; }
        public string AppBookID { get; set; }
        public string AppBookName { get; set; }
        public string AppChapterTitle { get; set; }
        public bool isAdd { get; set; }
    }

    public class ContentResponse
    {
        public string AppContentID { get; set; }
        public string AppChapterId { get; set; }
        public string AppBookID { get; set; }
        public string AppBookName { get; set; }
        public string AppParentID { get; set; }
        public string AppHeaderNo { get; set; }
        public string AppHeader { get; set; }
        public string AppContent { get; set; }
        public string AppChapterTitle { get; set; }

        public bool isAdd { get; set; }
    }
}
