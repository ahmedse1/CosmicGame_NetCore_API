using Dapper;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
   public class CommonRepo : ICommonRepo
    {
        private readonly IDbContext _DbContext;
        public CommonRepo(IDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task setErrorData(string strDescription)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Description", strDescription);
                var Response = await _DbContext.ExecutableData("SetError", param, CommandType.StoredProcedure);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
