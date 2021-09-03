using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Request.Book;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Repository.Implementation
{
    public class DbContext : IDbContext
    {
        private readonly IConfiguration _config;
        public DbContext(IConfiguration config)
        {
            _config = config;
            var strcon = _config["ConnectionStrings:DefaultConnection"];
        }
        private SqlConnection connection => new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

        public async Task<int> ExecutableData(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            try
            {
                int Response = 0;
                using (var conn = connection)
                {
                    Response = await conn.ExecuteAsync(Query, DynamicParameters, commandType: CommandType);
                }
                return Response;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
        }

        public async Task<(int, List<(string, object)>)> ExecutableDataWithOutput(string Query, DynamicParameters DynamicParameters, List<SpOutput> OutputParams, CommandType CommandType)
        {
            try
            {
                int Response = 0;
                List<(string, object)> objOutPutData = new List<(string, object)>();
                using (var conn = connection)
                {
                    Response = await conn.ExecuteAsync(Query, DynamicParameters, commandType: CommandType);
                    if (OutputParams != null)
                    {
                        for (int i = 0; i < OutputParams.Count; i++)
                        {
                            (string, object) vData = (OutputParams[i].ParameterName, DynamicParameters.Get<object>(OutputParams[i].ParameterName));
                            objOutPutData.Add(vData);
                        }
                    }
                }
                return (Response, objOutPutData);
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        public async Task<T> GetDataFirstDefault<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            try
            {
                T Response = default(T);
                using (var conn = connection)
                {
                    Response = await conn.QueryFirstAsync<T>(Query, DynamicParameters, commandType: CommandType);
                }
                return Response;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        public async Task<IEnumerable<T>> GetDataList<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            try
            {
                IEnumerable<T> Response = default(IEnumerable<T>);
                using (var conn = connection)
                {
                    Response = await conn.QueryAsync<T>(Query, DynamicParameters, commandType: CommandType);
                }
                return Response;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        public IEnumerable<T> GetDataList2<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            try
            {
                IEnumerable<T> Response = default(IEnumerable<T>);
                using (var conn = connection)
                {
                    Response = conn.Query<T>(Query, DynamicParameters, commandType: CommandType);
                }
                return Response;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        public async Task<dynamic> GetMultipleData(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            try
            {
                List<dynamic> Response = new List<dynamic>();
                //dynamic Response = null;
                using (var conn = connection)
                {
                    var multiData = await conn.QueryMultipleAsync(Query, DynamicParameters, commandType: CommandType);
                    try
                    {
                        while (true)
                        {
                            Response.Add(multiData?.ReadAsync<dynamic>().Result);
                        }
                    }
                    catch
                    {

                    }
                }
                return Response;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }


        //public async Task<(IEnumerable<bookResponse>,IEnumerable<chapterResponse>, IEnumerable<ContentResponse>)> GetBookViewData(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        //{
        //    try
        //    {
        //        List<dynamic> Response = new List<dynamic>();
        //        //dynamic Response = null;
        //        using (var conn = connection)
        //        {
        //            var Response = await conn.QueryMultipleAsync(Query, DynamicParameters, commandType: CommandType);
        //            try
        //            {
        //                while (true)
        //                {
        //                    Response.Add(multiData?.ReadAsync<dynamic>().Result);
        //                }
        //            }
        //            catch
        //            {

        //            }
        //        }
        //        return Response;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //    }
        //}
    }
}
