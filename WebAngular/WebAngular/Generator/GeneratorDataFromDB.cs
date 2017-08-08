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
        //// <summary>
        /// Генерируем данные для БД
        /// </summary>
        ////<summary>
        /// <param name="count">количество генерируемых записей</param>
        public static void generationData(int count)
        {
               
            string appDataPath = System.Web.HttpContext.Current.Server.MapPath(@"~/App/data");
            GeneratorData gd = new GeneratorData(appDataPath);
            List<Addres> arrAdr = new List<Addres>();
            //Генерируем записи
            try
            {
                using (DBAddressEntities db = new DBAddressEntities())
                {   //контекст данных
                    arrAdr = gd.getRecords(count);
                    
                    foreach (Addres adr in arrAdr)
                    {
                        //вызываем хранимую процедуру для добавления записей
                        db.sp_InsertAddress(adr.Country, adr.City, adr.Street, adr.Number, adr.Index, adr.Date);
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
    }
}