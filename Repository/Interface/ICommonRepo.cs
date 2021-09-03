using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
  public  interface ICommonRepo
    {
        Task setErrorData(string strDescription);
    }
}
