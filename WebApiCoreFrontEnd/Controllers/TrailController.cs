using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCoreFrontEnd.Models;
using WebApiCoreFrontEnd.Models.ViewModels;
using WebApiCoreFrontEnd.Repository.IRepository;

namespace WebApiCoreFrontEnd.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        public TrailController(ITrailRepository trailRepository, INationalParkRepository nationalParkRepository)
        {
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> nationalParks = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
            TrailVM trailVM = new TrailVM()
            {
                Trail = new Trail(),
                nationalParkList = nationalParks.Select(np => new SelectListItem()
                {
                    Text = np.Name,
                    Value = np.Id.ToString()
                }
            )
            };
            if (id == null) return View(trailVM);
            trailVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            if (trailVM.Trail == null) return NotFound();
            return View(trailVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Upsert(TrailVM trailVM)
        {
            if (ModelState.IsValid)
            {
                if (trailVM.Trail.Id == 0)
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
                else
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath, trailVM.Trail);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> nationalParks = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
                trailVM = new TrailVM()
                {
                    Trail = new Trail(),
                    nationalParkList = nationalParks.Select(np => new SelectListItem()
                    {
                        Text = np.Name,
                        Value = np.Id.ToString()
                    }
               )
                };
                return View(trailVM);

            }
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
                return Json(new { successs = true, message = "Data Deleted Succesffully " });
            return Json(new { success = false, message = "Something went wrong while deleting data!!!" });
        }
        #endregion
    }
}
