using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParcelDeliveryCo.Net.Models;
using ParcelDeliveryCo.Net.Models.HelperMethods;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnitTestProject2
{
    [TestClass]
    public class GetContainerTest
    {
        


        [TestMethod]
        [Timeout(2000)]
        public void GetPathTest()
        {
            //Arrange
            string id = "Test.xml";
            string expectedResult = Directory.GetCurrentDirectory() + "\\" +id;

            var containerManager = new ContainerManager();
            var path = containerManager.GetPath(id);

            Assert.AreEqual(path, expectedResult);
        }


        [TestMethod]
        [Timeout(2000)]

        public void GetContainersTest()
        {
            //Arrange
            string id = "Container_68465468.xml";
            string receipientName = "Vinny Gankema";
            string lastReceipientName = "Zeki Soekhai";
            string finalPath = "C:\\Users\\Paulo\\source\\repos\\ParcelDeliveryCo.Net\\" + id;


            var containerManager = new ContainerManager();
            var container = containerManager.ParseRequest(finalPath);

            Assert.AreEqual(container.ParcelItems[0].Receipient.Name, receipientName, "Issues Parsing Name of the XML file.");
            Assert.AreEqual(container.ParcelItems.Last().Receipient.Name, lastReceipientName, "Issues Parsing Name of the XML file.");

            
            //Could also check if the fields are populated.
        }

        [TestMethod]
        [Timeout(2000)]
        public void SortTest()
        {
            string id = "Container_68465468.xml";
            string finalPath = "C:\\Users\\Paulo\\source\\repos\\ParcelDeliveryCo.Net\\" + id;
            List<Department> departments = new List<Department>()
            {
                new Department {Id=1, IsActive =1, Name = "Heavy"},
                new Department {Id=2, IsActive =0, Name = "Mail"},
                new Department {Id=2, IsActive =0, Name = "Regular"},
            };

            //Format of Postal Code
            string regex = "[0-9]{4}[A-Z]{2}";
            
            var containerManager = new ContainerManager();
            var container = containerManager.ParseRequest(finalPath);
            containerManager.Sort(container, departments);

            //Assert:

            foreach (var parcel in container.ParcelItems)
            {
                //Checks if the Postal codes matches the Regex.
                Assert.IsTrue(Regex.IsMatch(parcel.Receipient.Address.PostalCode, regex));
                //Checks if the department was assigned.
                Assert.IsTrue(parcel.CurrentDepartment != null);

            }
            


        }
    }
}
