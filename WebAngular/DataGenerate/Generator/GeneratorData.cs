using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WebAngular.Models;

namespace DataGenerate.Generator
{
    class GeneratorData
    {
        /// <summary>
        /// возвращает строку insert для формирования запроса
        /// </summary>
        /// <returns></returns>
        private string getStartLine()
        {
            string str = "INSERT INTO Addres (Country, City, Street, Number, [Index], [Date]) VALUES";
            return str;
        }
        /// <summary>
        /// вернем все города
        /// </summary>
        /// <returns></returns>
        private List<string> getCity()
        {
            List<string> arrCity = new List<string>();
            using (StreamReader sr = new StreamReader(@"..\..\data\city.txt"))
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
            using (StreamReader sr = new StreamReader(@"..\..\data\street.txt"))
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
            List<string> arrStreet = new List<string> {"Росиия" };
            
            return arrStreet;
        }
        /// <summary>
        /// Генерация запроса, для добавления одной записи
        /// </summary>
        /// <returns></returns>
        public string getOneRecord() {
            Random rnd = new Random();
            int indCity = 0;    // сгенерированный индекс города
            int indStreet = 0;  // сгенерированный индекс улицы
            int numHouse = 1;   // номер дома 
            int index = 111111; // индекс

            string str = "";

            List<string> lCountry = new List<string>();
            List<string> lCity = new List<string>();
            List<string> lStreet = new List<string>();

            lCountry    = getCountry();     //получим список стран
            lCity       = getCity();        //получим список городов
            lStreet     = getStreet();      //получим список улиц

            indCity     = rnd.Next(0, lCity.Count - 1);
            indStreet   = rnd.Next(0, lStreet.Count - 1);
            index       = rnd.Next(111111, 1000000);
            numHouse    = rnd.Next(1, 301);

            //"MM.dd.yyyy hh:mm:ss" сначало месяц потои день, иначе sql запрос не выполняется
            // 31.07.2017 01:41:52 - не выполнится
            str = string.Format("{0} ('{1}', '{2}', '{3}', {4}, {5}, '{6}')",
                                    getStartLine(), lCountry[0], lCity[indCity], lStreet[indStreet], numHouse, index, DateTime.Now.ToString("MM.dd.yyyy hh:mm:ss"));
            return str;
        }

        /// <summary>
        /// Генерация запроса, для добавления count записeй
        /// </summary>
        /// <returns></returns>
        public string getRecords(int count)
        {
            Random rnd = new Random();
            int indCity = 0;    // сгенерированный индекс города
            int indStreet = 0;  // сгенерированный индекс улицы
            int numHouse = 1;   // номер дома 
            int index = 111111; // индекс

            List<string> listRecords = new List<string>();
            string str = "";

            List<string> lCountry = new List<string>();
            List<string> lCity = new List<string>();
            List<string> lStreet = new List<string>();

            lCountry = getCountry();     //получим список стран
            lCity = getCity();        //получим список городов
            lStreet = getStreet();      //получим список улиц


            listRecords.Add(getStartLine());    //запишем начало запроса

            for (int i = 0; i < count; i++)
            {
                indCity = rnd.Next(0, lCity.Count - 1);
                indStreet = rnd.Next(0, lStreet.Count - 1);
                index = rnd.Next(111111, 1000000);
                numHouse = rnd.Next(1, 301);

                //"MM.dd.yyyy hh:mm:ss" сначало месяц потои день, иначе sql запрос не выполняется
                // 31.07.2017 01:41:52 - не выполнится
                str = string.Format("{0} ('{1}', '{2}', '{3}', {4}, {5}, '{6}')",
                                        (i == 0 ? " " : ","), lCountry[0], lCity[indCity], lStreet[indStreet], numHouse, index, DateTime.Now.ToString("MM.dd.yyyy hh:mm:ss"));
                listRecords.Add(str);
            }
            str = string.Join(" ", listRecords.ToArray<string>());
            return str;
        }

    }
}
