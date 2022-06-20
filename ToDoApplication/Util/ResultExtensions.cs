using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Util
{
    static class ResultExtensions
    {
        public static Result<TResult> Map<TResult, TArg>(
           this Result<TArg> result,
           Func<TArg, TResult> mapperFunction)
        {
            if (result.WasSuccessful)
            {
                return Result.CreateSuccess(mapperFunction(result.Value));
            }
            else
            {
                return Result.CreateError<TResult>(result.Message);
            }
        }
    }
}
