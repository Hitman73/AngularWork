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

        /// <summary>
        /// Генерируем данные для БД
        /// </summary>
        private void generationDatoFromDB() {
            List<Addres> arrAdr = new List<Addres>();
            GeneratorData gd = new GeneratorData();
            //Генер
            using (AddressContext db = new AddressContext())
            {
                try
                {
                    arrAdr = gd.getRecords(80);
                    // добавляем их в бд
                    db.Address.AddRange(arrAdr);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                }
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
        public string SetPages() //вернем кол-во страниц удовлетворяющих запросу
        {
            int a = Int32.Parse(Request.Params["id"]);
            
            return "<h2>Площадь треугольника с основанием " + a + "</h2>";
        }

        [HttpPost]
        public ActionResult SortColumn(string sortType, string sortReverse) //сортируем таблицу по столбцу
        {
            //string sortType  = Request.Params["sortType"];
            //string sortReverse = Request.Params["sortReverse"];
            IOrderedQueryable<Addres> sortDate;

            switch (sortType) {
                case "Id":
                    sortDate = (sortReverse == "false") ? db.Address.OrderByDescending(p => p.Id) : db.Address.OrderBy(p => p.Id);
                    break;
                case "Country":
                    sortDate = (sortReverse == "false") ? db.Address.OrderByDescending(p => p.Country) : db.Address.OrderBy(p => p.Country);
                    break;
                case "City":
                    sortDate = (sortReverse == "false") ? db.Address.OrderByDescending(p => p.City) : db.Address.OrderBy(p => p.City);
                    break;
                case "Street":
                    sortDate = (sortReverse == "false") ? db.Address.OrderByDescending(p => p.Street) : db.Address.OrderBy(p => p.Street);
                    break;
                case "Num":
                    sortDate = (sortReverse == "false") ? db.Address.OrderByDescending(p => p.Number) : db.Address.OrderBy(p => p.Number);
                    break;
                case "Index":
                    sortDate = (sortReverse == "false") ? db.Address.OrderByDescending(p => p.Index) : db.Address.OrderBy(p => p.Index);
                    break;
                case "Date":
                    sortDate = (sortReverse == "false") ? db.Address.OrderByDescending(p => p.Date) : db.Address.OrderBy(p => p.Date);
                    break;
                default:
                    sortDate = (sortReverse == "false") ? db.Address.OrderByDescending(p => p.Id) : db.Address.OrderBy(p => p.Id);
                    break;
            }

            return Json(sortDate, JsonRequestBehavior.AllowGet); // и наш объект address сериализован
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
            /*if (number != "")
            {
                int num = Int32.Parse(number);
                f_date = f_date.Where(p => p.Number == num);
            }*/
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