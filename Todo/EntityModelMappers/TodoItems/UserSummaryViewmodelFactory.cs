using Microsoft.AspNetCore.Identity;
using Todo.Models.TodoItems;

namespace Todo.EntityModelMappers.TodoItems
{
    public class UserSummaryViewmodelFactory
    {
        public static UserSummaryViewmodel Create(IdentityUser identityUser)
        {
            var nameFromEmail = identityUser.Email?.Split('@')[0]; // Short-term name
            return new UserSummaryViewmodel(nameFromEmail, identityUser.Email);
        }
    }
}