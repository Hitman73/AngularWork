using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Deployment;

namespace WebAngular.Generator
{
    public class GeneratorData
    {
        public string pathToFile { get; }

        public GeneratorData(string path) {
            pathToFile = path;
        }
        /// <summary>
        /// Возвращает абсолютный путь к файлу fileName
        /// </summary>
        /// <param name="path">папка с файлом</param>
        /// /// <param name="fileName">имя файла</param>
        /// <returns></returns>
        private string getabsolutePathToFile(string fileName)
        {
            //string appDataPath = System.Web.HttpContext.Current.Server.MapPath(pathToFile);
            string absolutePathToFile = Path.Combine(pathToFile, fileName);

            return absolutePathToFile;
        }

        /// <summary>
        /// вернем все города
        /// </summary>
        /// <returns></returns>
        private List<string> getCity()
        {
            List<string> arrCity = new List<string>();

            //using (StreamReader sr = new StreamReader(getabsolutePathToFile(@"~/App/data", "city.txt")))
            using (StreamReader sr = new StreamReader(getabsolutePathToFile("city.txt")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > 1)
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
            using(StreamReader sr = new StreamReader(getabsolutePathToFile("street.txt")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if(line.Length > 1)
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
            DateTime dtStart = new DateTime(2008, 1, 1);
            List<Addres> arrAdr = new List<Addres>();

            List<string> lCountry = new List<string>();
            List<string> lCity = new List<string>();
            List<string> lStreet = new List<string>();

            lCountry = getCountry();     //получим список стран
            lCity = getCity();        //получим список городов
            lStreet = getStreet();      //получим список улиц           

            for (int i = 0; i < count; i++)
            {
                //генерируем индексы и данные
                indCity = rnd.Next(0, lCity.Count - 1);
                indStreet = rnd.Next(0, lStreet.Count - 1);
                index = rnd.Next(111111, 1000000);
                numHouse = rnd.Next(1, 301);
                //генерируем дату
                DateTime dtGen = dtStart.AddDays(rnd.Next(100)).AddSeconds(rnd.Next(1000));

                arrAdr.Add(new Addres {Country = lCountry[0] , City = lCity[indCity] , Street = lStreet[indStreet] ,
                            Number = numHouse, Index = index, Date = dtGen
                });
            }
            return arrAdr;
        }

        /// <summary>
        /// Вернем список городов
        /// </summary>
        /// <returns></returns>
        public List<Cities> getCityList()
        {
            List<string> lCity = new List<string>();
            List<Cities> arrCity = new List<Cities>();

            lCity = getCity();        //получим список городов

            foreach (string str in lCity) {
                arrCity.Add(new Cities { id = 1, name = str});
            }

            return arrCity;
        }

        /// <summary>
        /// Вернем список городов
        /// </summary>
        /// <returns></returns>
        public List<Street> getStreetList()
        {
            List<string> lStreet = new List<string>();
            List<Street> arrStreet = new List<Street>();

            lStreet = getStreet();        //получим список городов

            foreach (string str in lStreet)
            {
                arrStreet.Add(new Street { id = 1, name = str });
            }

            return arrStreet;
        }
    }
}