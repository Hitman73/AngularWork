using System;
using System.IO;

namespace WebAngular.Logirovanie
{
    /// <summary>
    /// Клас созранения действий пользвателя в файл
    /// </summary>
    static public class LogicSave
    {
        /// <summary>
        /// Запись в лог-файл
        /// </summary>
        /// <param name="operation">название операции</param>
        /// <param name="text">текст</param>
        static public void saveToFile(string fileName, string operation, string text) {
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                string str = string.Format("{0} - {1} : {2}", DateTime.Now.ToString(), operation, text);
                sw.WriteLine(str);
            }
        }
    }
}