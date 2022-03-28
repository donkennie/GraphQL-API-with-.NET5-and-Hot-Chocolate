using GraphQLAuth.InputType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLAuth.Logic
{
    public interface IAuthLogic
    {
        string Register(RegisterInputType registerInput);
    }
}
