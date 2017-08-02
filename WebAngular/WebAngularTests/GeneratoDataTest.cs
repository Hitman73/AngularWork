using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAngular.Generator;
using static System.Net.Mime.MediaTypeNames;
using WebAngular.Models;
using System.Collections.Generic;

namespace WebAngularTests
{
    [TestClass]
    public class GeneratoDataTest
    {
        [TestMethod]
        public void getOneRecordTest()
        {
            //Arrange
            string appDataPath = Environment.CurrentDirectory;
            GeneratorData gd = new GeneratorData(appDataPath);

            //Act
            Addres adr = gd.getOneRecord();

            //Assert
            Assert.IsNotNull(adr);
        }
        [TestMethod]
        public void getRecordsTest()
        {
            //Arrange
            string appDataPath = Environment.CurrentDirectory;
            GeneratorData gd = new GeneratorData(appDataPath);

            //Act
            List<Addres> adr = gd.getRecords(56);

            //Assert
            Assert.AreEqual(adr.Count, 56);
        }
        [TestMethod]
        public void getRecordsTestToCountry()
        {
            //Arrange
            string appDataPath = Environment.CurrentDirectory;
            GeneratorData gd = new GeneratorData(appDataPath);

            //Act
            List<Addres> adr = gd.getRecords(2);

            //Assert
            Assert.AreNotEqual(adr[0].Country, "");
        }
        [TestMethod]
        public void getRecordsTestToStreet()
        {
            //Arrange
            string appDataPath = Environment.CurrentDirectory;
            GeneratorData gd = new GeneratorData(appDataPath);

            //Act
            List<Addres> adr = gd.getRecords(2);

            //Assert
            Assert.AreNotEqual(adr[1].Street, "");
        }
    }
}
