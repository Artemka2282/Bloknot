using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfApp6;

namespace UnitTestProject1
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void TestMethod1()


        {
            // Arrange
            MainWindow window = new MainWindow();
            // Act
            window.Show(); // открываем окно
            window.Close(); // закрываем окно
                            // Assert
            Assert.IsFalse(window.IsVisible); // проверяем, что окно больше не отображается

        }
    }
}
