using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DataGenerate.DataGeneration
{
    public class DataGenerations
    {
        /// <summary>
        /// возвращает строку insert для формирования запроса
        /// </summary>
        /// <returns></returns>
        public string getStartLine() {
            return "";
        }
        /// <summary>
        /// вернем случайный город
        /// </summary>
        /// <returns></returns>
        public string[] getCity()
        {
            List<string> arrCity = new List<string>();
            using (StreamReader sr = new StreamReader("city.txt")) {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    arrCity.Add(line);
                }
            }
                return arrCity.ToArray();
        }
    }
}