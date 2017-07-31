using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Deployment;
using WebAngular.Models;

namespace WebAngular.Generator
{
    public class GeneratorData
    {
        /// <summary>
        /// Возвращает абсолютный путь к файлу fileName
        /// </summary>
        /// <param name="fileName">имя файла</param>
        /// <returns></returns>
        private string getabsolutePathToFile(string fileName) {
            string appDataPath = System.Web.HttpContext.Current.Server.MapPath(@"~/App/data");
            string absolutePathToFile = Path.Combine(appDataPath, fileName);

            return absolutePathToFile;
        }

        /// <summary>
        /// вернем все города
        /// </summary>
        /// <returns></returns>
        private List<string> getCity()
        {
            List<string> arrCity = new List<string>();
            
            using (StreamReader sr = new StreamReader(getabsolutePathToFile("city.txt")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    arrCity.Add(line);
                }
            }
            return arrCity;
        }

        /// <summary>
        /// вернем все улицы
        /// </summary>
        /// <returns></returns>
        private List<string> getStreet()
        {
            List<string> arrStreet = new List<string>();
            using (StreamReader sr = new StreamReader(getabsolutePathToFile("street.txt")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    arrStreet.Add(line);
                }
            }
            return arrStreet;
        }

        /// <summary>
        /// вернем все Страны
        /// </summary>
        /// <returns></returns>
        private List<string> getCountry()
        {
            List<string> arrStreet = new List<string> { "Росиия" };

            return arrStreet;
        }
        /// <summary>
        /// Генерация запроса, для добавления одной записи
        /// </summary>
        /// <returns></returns>
        public Addres getOneRecord()
        {
            Random rnd = new Random();
            int indCity = 0;    // сгенерированный индекс города
            int indStreet = 0;  // сгенерированный индекс улицы
            int numHouse = 1;   // номер дома 
            int index = 111111; // индекс
            Addres rec = new Addres();

            List<string> lCountry = new List<string>();
            List<string> lCity = new List<string>();
            List<string> lStreet = new List<string>();

            lCountry = getCountry();     //получим список стран
            lCity = getCity();        //получим список городов
            lStreet = getStreet();      //получим список улиц

            indCity = rnd.Next(0, lCity.Count - 1);
            indStreet = rnd.Next(0, lStreet.Count - 1);
            index = rnd.Next(111111, 1000000);
            numHouse = rnd.Next(1, 301);            

            rec.Id = 0;
            rec.Country = lCountry[0];
            rec.City = lCity[indCity];
            rec.Street = lStreet[indStreet];
            rec.Number = numHouse;
            rec.Index = index;
            rec.Date = DateTime.Now;
            return rec;
        }

        /// <summary>
        /// Генерация запроса, для добавления count записeй
        /// </summary>
        /// <returns></returns>
        public List<Addres> getRecords(int count)
        {
            Random rnd = new Random();
            int indCity = 0;    // сгенерированный индекс города
            int indStreet = 0;  // сгенерированный индекс улицы
            int numHouse = 1;   // номер дома 
            int index = 111111; // индекс

            List<Addres> arrAdr = new List<Addres>();

            List<string> lCountry = new List<string>();
            List<string> lCity = new List<string>();
            List<string> lStreet = new List<string>();

            lCountry = getCountry();     //получим список стран
            lCity = getCity();        //получим список городов
            lStreet = getStreet();      //получим список улиц           

            for (int i = 0; i < count; i++)
            {
                Addres rec = new Addres();
                //генерируем индексы и данные
                indCity = rnd.Next(0, lCity.Count - 1);
                indStreet = rnd.Next(0, lStreet.Count - 1);
                index = rnd.Next(111111, 1000000);
                numHouse = rnd.Next(1, 301);

                rec.Id = 0;
                rec.Country = lCountry[0];
                rec.City = lCity[indCity];
                rec.Street = lStreet[indStreet];
                rec.Number = numHouse;
                rec.Index = index;
                rec.Date = DateTime.Now;

                arrAdr.Add(rec);
            }
            return arrAdr;
        }
    }
}