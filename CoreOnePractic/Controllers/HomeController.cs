using CoreOnePractic.Entities;
using CoreOnePractic.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreOnePractic.Controllers
{
    public class HomeController : Controller
    {
        static List<TaskEntity> _tasks = new List<TaskEntity>()
        {
            new TaskEntity{ Id=1,Name="Deneme",Status=false, IsDeleted=false},
            new TaskEntity{ Id=2,Name="Test",Status=false, IsDeleted=false},
            new TaskEntity{ Id=3,Name="Yeni",Status=false, IsDeleted=false},
            new TaskEntity{ Id=4,Name="Dort",Status=false, IsDeleted=false},
            new TaskEntity{ Id=5,Name="Bes",Status=true, IsDeleted=true},
        };
        public IActionResult Index(string search, string statusFilter)
        {

            // Silinmemiş görevleri alıyoruz
            var listTask = _tasks.Where(x => x.IsDeleted == false);

            // Eğer arama terimi boş değilse, arama terimine göre filtrele
            if (!string.IsNullOrEmpty(search))
            {
                listTask = listTask.Where(x => x.Name.ToLower().Contains(search.ToLower()));
            }
            // Tamamlanma durumuna göre filtreleme
            if (!string.IsNullOrEmpty(statusFilter))
            {
                if (statusFilter == "completed")
                {
                    listTask = listTask.Where(x => x.Status == true);
                }
                else if (statusFilter == "incomplete")
                {
                    listTask = listTask.Where(x => x.Status == false);
                }
            }

            // Görevleri ViewModel'e dönüştürüyoruz
            var viewModel = listTask.Select(x => new TaskListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(int id) 
        {
            var detailsTask = _tasks.FirstOrDefault(x => x.Id == id);
            return View(detailsTask);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TaskAddViewModel addTask) 
        {
            var MaxId = _tasks.Max(x=> x.Id);   
            if (ModelState.IsValid)
            {
                var newTask = new TaskEntity
                {
                    Id =MaxId + 1,
                    Name = addTask.Name,
                };
                _tasks.Add(newTask);
                TempData["SuccessMessage"] = "Görev başarıyla oluşturuldu!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Görev oluşturulurken bir hata oluştu.";
            return View(addTask);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //Id ile eş olan değeri buldum
            var editTask = _tasks.FirstOrDefault(x=>x.Id == id);
            //null ise NotFound Döndüm
            if(editTask == null)
            {
                return NotFound();
            }
            //buradaki veriyi Entity üzerinden aldığım için öncelikle gelen veriyi bir ViewModel e dönüştürdüm.
            //Id eşleşen değeri entityden alıp viewmodel e aktardım
            var userEditModel = new TaskListViewModel
            {
                Id = editTask.Id,
                Name = editTask.Name,
                Status = editTask.Status,
            };
            return View(userEditModel);
        }
        [HttpPost]
        public IActionResult Edit(TaskListViewModel editList)
        {
            var edit = _tasks.FirstOrDefault(x=>x.Id ==editList.Id);
           if(ModelState.IsValid)
            {
               edit.Name = editList.Name;
               edit.Status = editList.Status;
                TempData["SuccessMessage"] = "Görev başarıyla Güncellendi!";
                return RedirectToAction("Index");   
            }
           return View(editList);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var deleteTask = _tasks.FirstOrDefault(x=>x.Id==id);
            if(deleteTask == null)
            {
                return NotFound();
            }
            deleteTask.IsDeleted = true;
            TempData["SuccessMessage"] = "Görev başarıyla silindi!";
            return RedirectToAction("Index");
        }
        
    }
}
