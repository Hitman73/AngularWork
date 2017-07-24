using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAngular.Models;

namespace WebAngular.Controllers
{
    public class HomeController : Controller
    {
        AddressContext db = new AddressContext();
        public ActionResult Index()
        {

            using (AddressContext db = new AddressContext())
            {
                // создаем два объекта User
                Addres adr1 = new Addres {Country="Россия", City="Ульяновск", Street="Гончарова", Number=33, Index=457825, Date=DateTime.Now };

                // добавляем их в бд
                //db.Address.Add(adr1);
                //db.SaveChanges();
            }
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

        [HttpGet]
        public int GetPages() //вернем кол-во страниц удовлетворяющих запросу
        {
            return 18;
        }
    }
}