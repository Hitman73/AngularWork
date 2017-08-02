using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAngular.Generator;
using WebAngular.Logirovanie;
using WebAngular.Models;

namespace WebAngular.Controllers
{
    public class HomeController : Controller
    {
        AddressContext db = new AddressContext();   //контекст данных
        List<Addres> arrAdr = new List<Addres>();
        
        /// <summary>
        /// Генерируем данные для БД
        /// </summary>
        private void generationDatoFromDB() {
            string appDataPath = System.Web.HttpContext.Current.Server.MapPath(@"~/App/data");
            GeneratorData gd = new GeneratorData(appDataPath);
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

        /// <summary>
        /// Запись в лог-файл
        /// </summary>
        /// <param name="operation">название операции</param>
        /// <param name="text">текст</param>
        [HttpGet]
        public void SaveLog(string operation, string text) //фильтрация данных
        {
            string appDataPath = System.Web.HttpContext.Current.Server.MapPath(@"~/App/data");
            string absolutePathToFile = Path.Combine(appDataPath, "log.txt");
            //сохраним в файл
            LogicSave.saveToFile(absolutePathToFile, operation, text);
        }
        public ActionResult Index()
        {
            generationDatoFromDB();
            return View();
        }
        /// <summary>
        /// Контроллер для сортировки и фильтрации данных
        /// </summary>
        /// <param name="f_country"></param>
        /// <param name="f_city"></param>
        /// <param name="f_street"></param>
        /// <param name="f_house"></param>
        /// <param name="f_number_min"></param>
        /// <param name="f_number_max"></param>
        /// <param name="f_index"></param>
        /// <param name="f_date"></param>
        /// <param name="f_StartDate"></param>
        /// <param name="f_EndDate"></param>
        /// <param name="sortType"></param>
        /// <param name="sortReverse"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SortFilterColumn(string f_country, string f_city, string f_street, string f_house,
                int? f_number_min, int? f_number_max, int? f_index, string f_date,
                DateTime? f_StartDate, DateTime? f_EndDate, string sortType, bool? sortReverse) //сортируем таблицу по столбцу
        {
            // получаем объекты из бд
            var address = db.Address;
            //Отфильтруем по Стране, Городу и улице
            var f_data = db.Address.Where(p => p.Country.Contains(f_country) && p.City.Contains(f_city) && p.Street.Contains(f_street));

            //Фильтруем по номеру дома
            if (f_house != "")
            {
                if (f_number_min != null)
                    f_data = f_data.Where(p => (p.Number >= f_number_min));
                if (f_number_max != null)
                    f_data = f_data.Where(p => (p.Number <= f_number_max));
            }

            //Фильтруем по индексу
            if (f_index != null)
            {
                f_data = f_data.Where(p => p.Index == f_index);
            }

            //Фильтруем по дате
            if (f_date != "")
            {
                if (f_StartDate != null)
                    f_data = f_data.Where(p => (p.Date >= f_StartDate));
                if (f_EndDate != null)
                    f_data = f_data.Where(p => (p.Date <= f_EndDate));
            }

            switch (sortType) {
                case "Id":
                    f_data = (sortReverse == false) ? f_data.OrderByDescending(p => p.Id) : f_data.OrderBy(p => p.Id);
                    break;
                case "Country":
                    f_data = (sortReverse == false) ? f_data.OrderByDescending(p => p.Country) : f_data.OrderBy(p => p.Country);
                    break;
                case "City":
                    f_data = (sortReverse == false) ? f_data.OrderByDescending(p => p.City) : f_data.OrderBy(p => p.City);
                    break;
                case "Street":
                    f_data = (sortReverse == false) ? f_data.OrderByDescending(p => p.Street) : f_data.OrderBy(p => p.Street);
                    break;
                case "Num":
                    f_data = (sortReverse == false) ? f_data.OrderByDescending(p => p.Number) : f_data.OrderBy(p => p.Number);
                    break;
                case "Index":
                    f_data = (sortReverse == false) ? f_data.OrderByDescending(p => p.Index) : f_data.OrderBy(p => p.Index);
                    break;
                case "Date":
                    f_data = (sortReverse == false) ? f_data.OrderByDescending(p => p.Date) : f_data.OrderBy(p => p.Date);
                    break;
                default:
                    f_data = (sortReverse == false) ? f_data.OrderByDescending(p => p.Id) : f_data.OrderBy(p => p.Id);
                    break;
            }

            return Json(f_data, JsonRequestBehavior.AllowGet); // и наш объект address сериализован
        }        
    }
    
}