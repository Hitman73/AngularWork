using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAngular.Generator;
using WebAngular.Models;

namespace WebAngular.Controllers
{
    public class HomeController : Controller
    {
        AddressContext db = new AddressContext();   //контекст данных
        List<Addres> arrAdr = new List<Addres>();
        GeneratorData gd = new GeneratorData();
        /// <summary>
        /// Генерируем данные для БД
        /// </summary>
        private void generationDatoFromDB() {
            
            //Генерируем записи
             try
            {
                arrAdr = gd.getRecords(90);
                foreach(Addres adr in arrAdr)
                {
                    // добавляем их в бд
                    db.Address.Add(adr);
                }
                db.SaveChanges();
            } 
             catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                string str = ex.Message;
            }
        }
        
        public ActionResult Index()
        {
            generationDatoFromDB();
            return View();
        }

        [HttpGet]
        public ActionResult GetAddress() //тут мы получим список адресов через аякс запрос
        {
            // получаем объекты из бд
            var address = db.Address;
            JsonResult jr = Json(address, JsonRequestBehavior.AllowGet); // и наш объект address сериализован
            
            return jr;
        }

        [HttpPost]
        public ActionResult SortFilterColumn() //сортируем таблицу по столбцу
        {
            string sortType  = Request.Params["sortType"];
            string sortReverse = Request.Params["sortReverse"];
            string country = Request.Params["f_country"];
            string city = Request.Params["f_city"];
            string street = Request.Params["f_street"];
            string f_house = Request.Params["f_house"];            
            int f_number_min = Int32.Parse(Request.Params["f_number_min"]);
            int f_number_max = Int32.Parse(Request.Params["f_number_max"]);
            string index = Request.Params["f_index"];
            string date = Request.Params["f_date"];

            string f_StartDate = Request.Params["f_StartDate"];
            string f_EndDate = Request.Params["f_EndDate"];

            // получаем объекты из бд
            var address = db.Address;
            //Отфильтруем по Стране, Городу и улице
            var f_date = db.Address.Where(p => p.Country.Contains(country) && p.City.Contains(city) && p.Street.Contains(street));

            //Фильтруем по номеру дома
            if (f_house != "")
            {
                f_date = f_date.Where(p => (p.Number >= f_number_min) && ((p.Number <= f_number_max)));
            }

            //Фильтруем по индексу
            if (index != "")
            {
                int ind = Int32.Parse(index);
                f_date = f_date.Where(p => p.Index == ind);
            }

            //Фильтруем по дате
            if (date != "")
            {
                try
                {
                    DateTime dt = DateTime.Parse(f_StartDate);
                    //f_date = f_date.Where(p => (p.Date.Ticks >= DateTime.Parse(f_StartDate).Ticks) && ((p.Date.Ticks <= DateTime.Parse(f_EndDate).Ticks)));
                }
                catch (ArgumentNullException ex) { }
            }

            switch (sortType) {
                case "Id":
                    f_date = (sortReverse == "false") ? f_date.OrderByDescending(p => p.Id) : f_date.OrderBy(p => p.Id);
                    break;
                case "Country":
                    f_date = (sortReverse == "false") ? f_date.OrderByDescending(p => p.Country) : f_date.OrderBy(p => p.Country);
                    break;
                case "City":
                    f_date = (sortReverse == "false") ? f_date.OrderByDescending(p => p.City) : f_date.OrderBy(p => p.City);
                    break;
                case "Street":
                    f_date = (sortReverse == "false") ? f_date.OrderByDescending(p => p.Street) : f_date.OrderBy(p => p.Street);
                    break;
                case "Num":
                    f_date = (sortReverse == "false") ? f_date.OrderByDescending(p => p.Number) : f_date.OrderBy(p => p.Number);
                    break;
                case "Index":
                    f_date = (sortReverse == "false") ? f_date.OrderByDescending(p => p.Index) : f_date.OrderBy(p => p.Index);
                    break;
                case "Date":
                    f_date = (sortReverse == "false") ? f_date.OrderByDescending(p => p.Date) : f_date.OrderBy(p => p.Date);
                    break;
                default:
                    f_date = (sortReverse == "false") ? f_date.OrderByDescending(p => p.Id) : f_date.OrderBy(p => p.Id);
                    break;
            }

            return Json(f_date, JsonRequestBehavior.AllowGet); // и наш объект address сериализован
        }

        [HttpPost]
        public ActionResult FilterDateTable() //фильтрация данных
        {
            string country  = Request.Params["f_country"];
            string city     = Request.Params["f_city"];
            string street   = Request.Params["f_street"];
            int f_number_min = Int32.Parse(Request.Params["f_number_min"]);
            int f_number_max = Int32.Parse(Request.Params["f_number_max"]);
            string index = Request.Params["f_index"];
            string date = Request.Params["f_date"];

            // получаем объекты из бд
            var address = db.Address;
            //Отфильтруем по Стране, Городу и улице
            var f_date = db.Address.Where(p => p.Country.Contains(country) && p.City.Contains(city) && p.Street.Contains(street));

            //Фильтруем по номеру
            f_date = f_date.Where(p => (p.Number >= f_number_min) && ((p.Number <= f_number_max)));

            //Фильтруем по индексу
            if (index != "")
            {
                int ind = Int32.Parse(index);
                f_date = f_date.Where(p => p.Index == ind);
            }
            JsonResult jr = Json(f_date, JsonRequestBehavior.AllowGet); // и наш объект address сериализован

            return jr;
        }
    }
    
}