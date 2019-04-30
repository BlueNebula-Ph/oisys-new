using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OisysNew.DTO.Item;
using OisysNew.Helpers;
using System.Linq;

namespace OisysNew.Filters
{
    public class ValidateDuplicateItemCodeAttribute : ActionFilterAttribute
    {
        private readonly IOisysDbContext dbContext;

        public ValidateDuplicateItemCodeAttribute(IOisysDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var input = context.ActionArguments["entity"];
            if (input == null)
            {
                return;
            }

            if (!(input is SaveItemRequest model))
            {
                return;
            }

            var item = dbContext.Items.FirstOrDefault(a => !a.IsDeleted && a.Code == model.Code && a.Id != model.Id);
            if (item != null)
            {
                context.ModelState.AddModelError(Constants.ErrorMessage, "Item code already exists.");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
