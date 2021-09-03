using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IDbContext
    {
        Task<IEnumerable<T>> GetDataList<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
        Task<T> GetDataFirstDefault<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
        Task<int> ExecutableData(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
        Task<(int, List<(string, object)>)> ExecutableDataWithOutput(string Query, DynamicParameters DynamicParameters, List<SpOutput> OutputParams, CommandType CommandType);
        Task<dynamic> GetMultipleData(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
        IEnumerable<T> GetDataList2<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
    }
}
