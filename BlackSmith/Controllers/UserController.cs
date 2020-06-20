using BlackSmith;
using BlackSmithCore;
using Microsoft.AspNetCore.Mvc;
using System;
using ViewModels;

namespace BlackSmithAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/User/[action]")]
    public class UserController : BaseController
    {
        private IOperation<User> _userOpp;

        public UserController(IOperation<User> userOpp)
        {
            _userOpp = userOpp;
        }

        [HttpPost]
        public bool Validate([FromBody]User user)
        {
            bool result = false;
            try
            {
                var predicate = PredicateBuilder.True<User>();
                predicate = predicate.And(x => !x.IsDeleted);
                predicate = predicate.And(x => x.UserName == user.UserName);
                predicate = predicate.And(x => x.Password == user.Password);

                var usr = _userOpp.GetAllUsingExpression(out int totalCount, 1, 0, predicate);

                if (usr != null && usr.Count == 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return result;
        }
    }
}