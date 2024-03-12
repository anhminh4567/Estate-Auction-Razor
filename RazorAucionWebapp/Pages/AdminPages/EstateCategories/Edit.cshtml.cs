using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.RealEstate;
using Repository.Implementation.RealEstate;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.AdminPages.EstateCategories
{
    public class EditModel : PageModel
    {
        private readonly EstateCategoryDetailServices _estateCategoryDetailRepository;
        [BindProperty]
        public EstateCategoryDetail EstateCategoryDetail { get; set; } = default!;
        public EditModel(EstateCategoryDetailServices estateCategoryDetailRepository)
        {
            _estateCategoryDetailRepository = estateCategoryDetailRepository;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id is null)
            {
                return RedirectToPage("./Index");
            }
            EstateCategoryDetail = await _estateCategoryDetailRepository.GetById(id.Value);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                //error at this line
                var flag = await _estateCategoryDetailRepository.CheckForDuplicateName(EstateCategoryDetail);
                if (flag)
                {
                    try
                    {

                        await _estateCategoryDetailRepository.UpdateCategoryDetail(EstateCategoryDetail);
                        TempData["UpdateSuccess"] = String.Format("Category {0} updated", EstateCategoryDetail.CategoryName);
                        return RedirectToPage("./Index");
                    }
                    catch(Exception ex)
                    {
                        TempData["UpdateFail"] = "Can't update detail due to unexpected reason!" + ex.ToString();
                        return Page();
                    }
                }
                else
                {
                    TempData["UpdateFail"] = "Can't update detail due to duplicate category name!";
                    return Page();
                }
            }
        }
    }
}

