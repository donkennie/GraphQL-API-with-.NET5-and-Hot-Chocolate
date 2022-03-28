using GraphQLAuth.InputType;
using GraphQLAuth.Logic;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLAuth.Resolver
{
    public class MutationResolver
    {

        public string Register([Service] RegisterInputType registerInput, IAuthLogic authLogic)
        {
            return authLogic.Register(registerInput);


        }

    }
}
 