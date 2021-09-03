using Models.Request.Book;
using Models.Response;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
   public class BookService : IBookService
    {
        
        private readonly IBookRepo _IBookRepo;
        public BookService(IBookRepo IBookRepo)
        {
            _IBookRepo = IBookRepo;
           
        }
        public async Task<ServiceResponse> AddEditBook(bookRequest obj, int iUserID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (obj.AppBookID == "")
                {
                    obj.AppBookID = "0";
                    obj.isAdd = true;
                }

                var Response = await _IBookRepo.AddEditBook(obj);

                if (Response.Item2 || Response.Item1 > 0)
                {
                   
                    if (Response.Item2)
                    {
                        objResponse.success = !Response.Item2;
                        objResponse.message = "Record already exist!";
                        objResponse.status = HttpStatusCode.Conflict;
                    }
                    else
                    {
                        
                        objResponse.success = true;
                        if (obj.isAdd)
                            objResponse.message = "Added successfully";
                        else
                            objResponse.message = "Updated successfully";
                        objResponse.status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    objResponse.success = false;
                    if (obj.isAdd)
                        objResponse.message = "Added Failed.!";
                    else
                        objResponse.message = "Updated Failed.!";
                    objResponse.status = HttpStatusCode.InternalServerError;

                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }


        public async Task<ServiceResponse> AddEditChapter(chapterRequest obj, int iUserID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (obj.AppChapterID == "")
                {
                    obj.AppChapterID = "0";
                    obj.isAdd = true;
                }

                var Response = await _IBookRepo.AddEditChapter(obj);

                if (Response.Item2 || Response.Item1 > 0)
                {

                    if (Response.Item2)
                    {
                        objResponse.success = !Response.Item2;
                        objResponse.message = "Record already exist!";
                        objResponse.status = HttpStatusCode.Conflict;
                    }
                    else
                    {

                        objResponse.success = true;
                        if (obj.isAdd)
                            objResponse.message = "Added successfully";
                        else
                            objResponse.message = "Updated successfully";
                        objResponse.status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    objResponse.success = false;
                    if (obj.isAdd)
                        objResponse.message = "Added Failed.!";
                    else
                        objResponse.message = "Updated Failed.!";
                    objResponse.status = HttpStatusCode.InternalServerError;

                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }


        public async Task<ServiceResponse> AddEditContent(ContentRequest obj, int iUserID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (obj.AppContentID == "")
                {
                    obj.AppContentID = "0";
                    obj.isAdd = true;
                }

                var Response = await _IBookRepo.AddEditContent(obj);

                if (Response.Item2 || Response.Item1 > 0)
                {

                    if (Response.Item2)
                    {
                        objResponse.success = !Response.Item2;
                        objResponse.message = "Record already exist!";
                        objResponse.status = HttpStatusCode.Conflict;
                    }
                    else
                    {

                        objResponse.success = true;
                        if (obj.isAdd)
                            objResponse.message = "Added successfully";
                        else
                            objResponse.message = "Updated successfully";
                        objResponse.status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    objResponse.success = false;
                    if (obj.isAdd)
                        objResponse.message = "Added Failed.!";
                    else
                        objResponse.message = "Updated Failed.!";
                    objResponse.status = HttpStatusCode.InternalServerError;

                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }


        public async Task<ServiceResponse> BookList(int iBookID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var obj = await _IBookRepo.BookList(iBookID);
                objResponse.result = obj;
                objResponse.success = true;
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> ChapterList( int iChapterID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var obj = await _IBookRepo.ChapterList(iChapterID);
                objResponse.result = obj;
                objResponse.success = true;
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> ChapterListByBook(int iBookID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var obj = await _IBookRepo.ChapterListByBook(iBookID);
                objResponse.result = obj;
                objResponse.success = true;
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> ContentList(int iContentID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var obj = await _IBookRepo.ContentList(iContentID);
                objResponse.result = obj;
                objResponse.success = true;
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> ContentListByChapterID(int iContentID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var obj = await _IBookRepo.ContentListByChapterID(iContentID);
                objResponse.result = obj;
                objResponse.success = true;
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> GetBookViewData()
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var obj = await _IBookRepo.GetBookViewData();
                objResponse.result = obj;
                objResponse.success = true;
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }
    }
}
