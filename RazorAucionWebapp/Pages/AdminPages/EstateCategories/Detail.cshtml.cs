using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.RealEstate;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.AdminPages.EstateCategories
{
    public class DetailModel : PageModel
    {
        private readonly EstateCategoryDetailServices _estateCategoryDetailRepository;
        private readonly EstateCategoriesServices _estateCategoriesRepository;
        public DetailModel(EstateCategoryDetailServices estateCategoryDetailRepository, EstateCategoriesServices estateCategoriesRepository)
        {
            _estateCategoryDetailRepository = estateCategoryDetailRepository;
            _estateCategoriesRepository = estateCategoriesRepository;
        }
        
        public EstateCategoryDetail EstateCategoryDetail { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id == null)
            {
                return RedirectToPage("./Index");
            }
            EstateCategoryDetail = await _estateCategoryDetailRepository.GetById(id.Value);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int delId)
        {
            var flag = await DeleteCategory(delId);
            return RedirectToPage("./Index");
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var flag1 = await _estateCategoriesRepository.CheckForCategoryDetailInUsed(id);
            if (!flag1)
            {
                var item = await _estateCategoryDetailRepository.GetById(id);
                var flag2 = await _estateCategoryDetailRepository.DeleteCategoryDetail(item);
                if (flag2)
                {
                    TempData["DelSuccess"] = String.Format("Category {0} removed!", item.CategoryName);
                    return true;
                }
                else
                {
                    TempData["DelFail"] = "Can't remove category due to database error!";
                }
            }
            else
            {
                TempData["DelFail"] = "Can't remove category due to it is still in use!";
            }
            return false;
        }
    }
}
