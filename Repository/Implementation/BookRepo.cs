using Dapper;
using Models.Request.Book;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class BookRepo : IBookRepo
    {
        private readonly IDbContext _DbContext;
        private readonly ICommonRepo _CommonRepo;


        public BookRepo(IDbContext DbContext, ICommonRepo CommonRepo)
        {
            _DbContext = DbContext;
            _CommonRepo = CommonRepo;

        }
        public async Task<(int, bool)> AddEditBook(bookRequest obj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppBookID", Convert.ToInt32(obj.AppBookID));
                param.Add("@AppBookName", obj.AppBookName);
                param.Add("@AppAuther", obj.AppAuther);
                param.Add("@AppOutBookID", DbType.Int32, direction: ParameterDirection.Output);

                List<SpOutput> OutputParameter = new List<SpOutput>();
                OutputParameter.Add(new SpOutput { ParameterName = "AppOutBookID", ParamterType = typeof(int) });
                // OutputParameter.Add(new SpOutput { ParameterName = "isExist", ParamterType = typeof(int) });

                var Response = await _DbContext.ExecutableDataWithOutput("Book__AddEditBook", param, OutputParameter, CommandType.StoredProcedure);
                int iBookId = Convert.ToInt32(Response.Item2.Where(x => x.Item1 == "AppOutBookID").Select(x => x.Item2).FirstOrDefault().ToString());
                return (iBookId, false);
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> AddEditBook : " + ex.Message + "::" + ex.StackTrace);
                return (0, false);
            }
        }


        public async Task<(int, bool)> AddEditChapter(chapterRequest obj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppChapterID", Convert.ToInt32(obj.AppChapterID));
                param.Add("@AppBookID", Convert.ToInt32(obj.AppBookID));
                param.Add("@AppChapterNo", obj.AppChapterNo);
                param.Add("@AppChapterTitle", obj.AppChapterTitle);
                param.Add("@AppOutChapterID", DbType.Int32, direction: ParameterDirection.Output);

                List<SpOutput> OutputParameter = new List<SpOutput>();
                OutputParameter.Add(new SpOutput { ParameterName = "@AppOutChapterID", ParamterType = typeof(int) });
                // OutputParameter.Add(new SpOutput { ParameterName = "isExist", ParamterType = typeof(int) });

                var Response = await _DbContext.ExecutableDataWithOutput("Book__AddEditChapter", param, OutputParameter, CommandType.StoredProcedure);
                int iBookId = Convert.ToInt32(Response.Item2.Where(x => x.Item1 == "@AppOutChapterID").Select(x => x.Item2).FirstOrDefault().ToString());
                return (iBookId, false);
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> AddEditChapter : " + ex.Message + "::" + ex.StackTrace);
                return (0, false);
            }
        }


        public async Task<(int, bool)> AddEditContent(ContentRequest obj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppContentID", Convert.ToInt32(obj.AppContentID));
                param.Add("@AppBookID", Convert.ToInt32(obj.AppBookID));
                param.Add("@AppChapterId", Convert.ToInt32(obj.AppChapterId));
                param.Add("@AppParentID", Convert.ToInt32(obj.AppParentID));
                param.Add("@AppHeader", obj.AppHeader);
                param.Add("@AppContent", obj.AppContent);
                param.Add("@AppOutContentID", DbType.Int32, direction: ParameterDirection.Output);

                List<SpOutput> OutputParameter = new List<SpOutput>();
                OutputParameter.Add(new SpOutput { ParameterName = "@AppOutContentID", ParamterType = typeof(int) });
                // OutputParameter.Add(new SpOutput { ParameterName = "isExist", ParamterType = typeof(int) });

                var Response = await _DbContext.ExecutableDataWithOutput("Book__AddEditContent", param, OutputParameter, CommandType.StoredProcedure);
                int iBookId = Convert.ToInt32(Response.Item2.Where(x => x.Item1 == "@AppOutContentID").Select(x => x.Item2).FirstOrDefault().ToString());
                return (iBookId, false);
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> AddEditContent : " + ex.Message + "::" + ex.StackTrace);
                return (0, false);
            }
        }


        public async Task<IEnumerable<bookResponse>> BookList(int iBookID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppBookID", iBookID);
                var Response = await _DbContext.GetDataList<bookResponse>("Book_LoadBooks", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> BookList : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }


        public async Task<IEnumerable<chapterResponse>> ChapterList(int iChapterID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppChapterID", iChapterID);
                var Response = await _DbContext.GetDataList<chapterResponse>("Book_LoadChapter", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> ChapterList : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }


        public async Task<IEnumerable<chapterResponse>> ChapterListByBook(int iBookID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppBookID", iBookID);
                var Response = await _DbContext.GetDataList<chapterResponse>("Book_LoadChapterbyBook", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> ChapterListByBook : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ContentResponse>> ContentList(int iContentID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppContentID", iContentID);
                var Response = await _DbContext.GetDataList<ContentResponse>("Book_LoadContent", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> ContentList : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ContentResponse>> ContentListByChapterID(int iChapterID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AppChapterID", iChapterID);
                var Response = await _DbContext.GetDataList<ContentResponse>("Book_LoadContentbyChapterId", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> ContentListByChapterID : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }

        public async Task<dynamic> GetBookViewData()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();

                var Response = await _DbContext.GetMultipleData("Book_LoadBookView", param, CommandType.StoredProcedure); 
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> BookRepo -> GetBookViewData : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }
    }
}
