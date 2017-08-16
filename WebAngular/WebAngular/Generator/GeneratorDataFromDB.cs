using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAngular.Generator
{
    /// <summary>
    /// Генератор случайных данных в БД
    /// </summary>
    public static class GeneratorDataFromDB
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();  //создаем лог
        //// <summary>
        /// Генерируем данные для БД
        /// </summary>
        ////<summary>
        /// <param name="count">количество генерируемых записей</param>
        public static void generationData()
        {
               
            string appDataPath = System.Web.HttpContext.Current.Server.MapPath(@"~/App/data");
            GeneratorData gd = new GeneratorData(appDataPath);
            List<Cities> city = new List<Cities>();
            List<Street> street = new List<Street>();
            //Генерируем записи
            try
            {
                using (DBAddressEntities db = new DBAddressEntities())
                {   //добавим города
                    if (db.Cities.Count() == 0)
                    {
                        city = gd.getCityList();
                        foreach (Cities c in city) { db.Cities.Add(c); }
                        db.SaveChanges();
                    }

                    //добавим улицы
                    if (db.Street.Count() == 0)
                    {
                        street = gd.getStreetList();
                        foreach (Street c in street) { db.Street.Add(c); }
                        db.SaveChanges();
                    }
                    //изменяем таймаут, запрос на добавление 100000 записей выполняется примерно 6 мин 30 сек
                    db.Database.CommandTimeout = 460;
                    //генерируем записи в таблицу Addres
                    db.genRecord();                 
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);
            }
        }
    }
}