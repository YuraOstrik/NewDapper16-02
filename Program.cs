using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Data;
using Z.Dapper.Plus;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewDapperFramework14._02
{
    internal class Program
    {
        static string? connectionString;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            string path = Directory.GetCurrentDirectory();
            builder.SetBasePath(path);
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");

            try
            {
                while (true)
                {
                    Console.Clear();
                    //Console.WriteLine("1. Вставка информации о новых покупателях;");
                    //Console.WriteLine("2. Вставка новых стран;");
                    //Console.WriteLine("3. Вставка новых городов;");
                    //Console.WriteLine("4. Вставка информации о новых разделах;");
                    //Console.WriteLine("5. Вставка информации о новых акционных товарах.");
                    
                    //Console.WriteLine(" -- -- -- -- -- -- -- -- -- -- ");

                    //Console.WriteLine("11. Обновление информации о покупателях;");
                    //Console.WriteLine("12. Обновление информации о странах;");
                    //Console.WriteLine("13. Обновление информации о городах;");
                    //Console.WriteLine("14. Обновление информации о разделах;");
                    //Console.WriteLine("15. Обновление информации об акционных товарах");
                    
                    //Console.WriteLine(" -- -- -- -- -- -- -- -- -- -- ");

                    //Console.WriteLine("21. Удаление информации о покупателях;");
                    //Console.WriteLine("22. Удаление информации о странах;");
                    //Console.WriteLine("23. Удаление информации о городах;");
                    //Console.WriteLine("24. Удаление информации о разделах;");
                    //Console.WriteLine("25. Удаление информации об акционных товарах");

                    Console.WriteLine(" -- -- -- -- -- -- -- -- -- -- ");

                    Console.WriteLine("31. Отображение информации о покупателях;");
                    Console.WriteLine("32. Отображение информации о странах;");
                    Console.WriteLine("33. Отображение информации о городах;");

                    Console.WriteLine("0. Выход");
                    int result = int.Parse(Console.ReadLine()!);
                    switch (result)
                    {
                        //case 1:
                        //    AddNewInfoCust();
                        //    break;
                        //case 2:
                        //    AddNewCounty();
                        //    break;
                        //case 3:
                        //    AddNewCity();
                        //    break;
                        //case 4:
                        //    AddNewInfoSect();
                        //    break;
                        //case 5:
                        //    AddNewPromotion();
                        //    break;
                        //case 0:
                        //    return;


                        //case 11:
                        //    BulkUpdateCust();
                        //    break;
                        //case 12:
                        //    BulkUpdateCountry();
                        //    break;
                        //case 13:
                        //    BulkUpdateCity();
                        //    break;
                        //case 14:
                        //    BulkUpdateSection();
                        //    break;
                        //case 15:
                        //    BulkUpdatePromo();
                        //    break;

                        //case 21:
                        //    DeleteCust();
                        //    break;
                        //case 22:
                        //    BulkDeleteCountry();
                        //    break;
                        //case 23:
                        //    BulkDeleteCity();
                        //    break;
                        //case 24:
                        //    BulkDeleteSection();
                        //    break;
                        //case 25:
                        //    BulkDeletePromo();
                        //    break;

                        case 31:
                            CityCountry();
                            break;
                        case 32:
                            CustSection();
                            break;
                        case 33:
                            CustSections();
                            break;
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        static void AddNewInfoCust()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string fullName, gender, email;
                DateTime birthDate;
                int cityIndex;

                do
                {
                    Console.Write("Введите ФИО: ");
                    fullName = Console.ReadLine()!;
                }
                while (string.IsNullOrWhiteSpace(fullName));


                do
                {
                    Console.Write("Введите дату рождения (гггг-мм-дд): ");
                }
                while (!DateTime.TryParse(Console.ReadLine(), out birthDate));

                do
                {
                    Console.Write("Введите пол (М/Ж): ");
                    gender = Console.ReadLine()!.Trim().ToUpper();
                }
                while (gender != "М" && gender != "Ж");

                do
                {
                    Console.Write("Введите email: ");
                    email = Console.ReadLine()!;
                }
                while (string.IsNullOrWhiteSpace(email) || !email.Contains("@"));

                var cities = db.Query<City>("SELECT * FROM Cities").ToList();
                if (cities.Count == 0)
                {
                    Console.WriteLine("Нет доступных городов. Добавьте город перед созданием покупателя.");
                    return;
                }

                Console.WriteLine("Выберите номер города из списка:");
                for (int i = 0; i < cities.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {cities[i].Name}");
                }

                do
                {
                    Console.Write("Введите номер города: ");
                }
                while (!int.TryParse(Console.ReadLine(), out cityIndex) || cityIndex < 1 || cityIndex > cities.Count);

                int cityId = cities[cityIndex - 1].Id;

                var customer = new Customer()
                {
                    FullName = fullName,
                    BirthDate = birthDate,
                    Gender = gender,
                    Email = email,
                    CityId = cityId
                };

                string sqlQuery = "INSERT INTO Customers (FullName, BirthDate, Gender, Email, CityId) " +
                                  "VALUES (@FullName, @BirthDate, @Gender, @Email, @CityId)";

                int rowsAffected = db.Execute(sqlQuery, customer);
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Покупатель был успешно добавлен.");
                }
                else
                {
                    Console.WriteLine("Ошибка при добавлении покупателя.");
                }
            }
            Console.ReadLine();
        }

        static void AddNewCounty() {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string name;
                Console.WriteLine("Введите название Страны");
                name = Console.ReadLine();

                var country = new Country { Name = name };
                var sqlQuery = "Insert into Countries (Name) Values(@Name)";
                int num = db.Execute(sqlQuery, country);
                if(num != 0)
                {
                    Console.WriteLine(" Страна добавлена");
                }


            }
            Console.ReadLine();
        }

        static void AddNewCity()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string name;
                int countryIndex;
                var counrties = db.Query<Country>("Select * from Countries").ToList();
                Console.WriteLine("Все страны -");
                for (int i = 0; i < counrties.Count; i++)
                {
                    Console.WriteLine($"{i + 1}, {counrties[i].Name}");
                }
                do
                {
                    Console.WriteLine("Введите номер страны");
                }
                while (!int.TryParse(Console.ReadLine(), out countryIndex));
                int countryId = counrties[countryIndex - 1].Id;

                Console.WriteLine("Введите название Города которое вы хотите добавить");
                name = Console.ReadLine();
                


                var city = new City { Name = name, CountryId = countryId};
                var sqlQuery = "Insert into Cities (Name, CountryId) Values(@Name,@CountryId)";
                int num = db.Execute(sqlQuery, city);
                if (num != 0)
                {
                    Console.WriteLine(" Город добавлена");
                }
            }
            Console.ReadLine();
        }
        static void AddNewInfoSect()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string name;
                Console.WriteLine("Введите название Отдела");
                name = Console.ReadLine();

                var country = new Section { Name = name };
                var sqlQuery = "Insert into Sections (Name) Values(@Name)";
                int num = db.Execute(sqlQuery, country);
                if (num != 0)
                {
                    Console.WriteLine(" Отдел добавлена");                                 
                }                          
            }                                         
            Console.ReadLine();                  
        }

        static void AddNewPromotion()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {

                var sections = db.Query<Section>("SELECT * FROM Sections").ToList();
                if (sections.Count == 0)
                {
                    Console.WriteLine("Нет доступных разделов. Добавьте хотя бы один.");
                    return;
                }

                Console.WriteLine("Выберите раздел для акции:");
                for (int i = 0; i < sections.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {sections[i].Name}");
                }

                int sectionIndex;
                do
                {
                    Console.Write("Введите номер раздела: ");
                }
                while (!int.TryParse(Console.ReadLine(), out sectionIndex) || sectionIndex < 1 || sectionIndex > sections.Count);

                int sectionId = sections[sectionIndex - 1].Id;

                var countries = db.Query<Country>("SELECT * FROM Countries").ToList();
                if (countries.Count == 0)
                {
                    Console.WriteLine("Нет доступных стран. Добавьте хотя бы одну.");
                    return;
                }

                Console.WriteLine("Выберите страну для акции:");
                for (int i = 0; i < countries.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {countries[i].Name}");
                }

                int countryIndex;
                do
                {
                    Console.Write("Введите номер страны: ");
                }
                while (!int.TryParse(Console.ReadLine(), out countryIndex) || countryIndex < 1 || countryIndex > countries.Count);

                int countryId = countries[countryIndex - 1].Id;

                DateTime startDate, endDate;

                do
                {
                    Console.Write("Введите дату начала акции (ГГГГ-ММ-ДД): ");
                }
                while (!DateTime.TryParse(Console.ReadLine(), out startDate));

                do
                {
                    Console.Write("Введите дату окончания акции (ГГГГ-ММ-ДД): ");
                }
                while (!DateTime.TryParse(Console.ReadLine(), out endDate) || endDate < startDate);


                var promotion = new
                {
                    SectionId = sectionId,
                    CountryId = countryId,
                    StartDate = startDate,  
                    EndDate = endDate       
                };

                var sqlQuery = "INSERT INTO Promotions (SectionId, CountryId, StartDate, EndDate) " +
                               "VALUES (@SectionId, @CountryId, @StartDate, @EndDate)";

                int rowsAffected = db.Execute(sqlQuery, promotion);
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Акция успешно добавлена.");
                }
                else
                {
                    Console.WriteLine("Ошибка при добавлении акции.");
                }
            }
            Console.ReadLine();
        }


        static void BulkUpdateCust()
        {
            Console.Clear();

            using(IDbConnection db = new SqlConnection(connectionString))
            {

                var cust = db.Query<Customer>("Select * from Customers").ToList();
                cust[1].FullName = "Влад Игоревич";
                cust[2].FullName = "Игорь Владиславович";

                db.BulkUpdate(cust);
                Console.WriteLine("Успешно добавлено");
            }
            Console.ReadLine();
        }
        static void BulkUpdateCountry()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                var cust = db.Query<Country>("Select * from Countries").ToList();
                cust[1].Name = "Вьеинам";
                cust[2].Name = "Индонезия";

                db.BulkUpdate(cust);
                Console.WriteLine("Успешно добавлено");
            }
            Console.ReadLine();
        }
        static void BulkUpdateCity()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                var cust = db.Query<City>("Select * from Cities").ToList();
                cust[1].Name = "Сеул";
                cust[2].Name = "Бангкок";

                db.BulkUpdate(cust);
                Console.WriteLine("Успешно добавлено");
            }
            Console.ReadLine();
        }
        static void BulkUpdateSection()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                var cust = db.Query<Section>("Select * from Sections").ToList();
                cust[1].Name = "Игрушки";
                cust[2].Name = "Медтовары";

                db.BulkUpdate(cust);
                Console.WriteLine("Успешно добавлено");
            }
            Console.ReadLine();
        }
        static void BulkUpdatePromo()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                var cust = db.Query<PromotionProduct>("Select * from PromotionProducts").ToList();
                cust[1].ProductName = "Духи";
                cust[2].ProductName = "Джинсы";

                db.BulkUpdate(cust);
                Console.WriteLine("Успешно добавлено");
            }
            Console.ReadLine();
        }


        static void DeleteCust()
        {
            Console.Clear();
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                var cust = db.Query<Customer>("Select * from Customers").ToList();
                for(int i = 0; i < cust.Count(); i++)
                {
                    Console.WriteLine($" {i + 1} - {cust[i].FullName}");
                }
                Console.WriteLine("Введите номер покупателя");

                int index = int.Parse(Console.ReadLine()!);
                var delcust = db.Query<Customer>("Select * from Customers").ToList()[index - 1];
                var sqlQuery = "Delete from Customers where id = @id";

                index = db.Execute(sqlQuery, new { delcust.Id });
                if (index != 0)
                    Console.WriteLine("Студент успешно удален!");
            }

            Console.ReadLine();
        }

        static void BulkDeleteCountry()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countrys = db.Query<Country>("select * from Countries");
                db.BulkDelete(countrys.Where(x => x.Name == "Италия"));
                Console.WriteLine("Сделано -_-");
            }

            Console.ReadLine();
        }
        static void BulkDeleteCity()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countrys = db.Query<City>("select * from Cities");
                db.BulkDelete(countrys.Where(x => x.Name == "Индонезия"));
                Console.WriteLine("Сделано -_-");
            }

            Console.ReadLine();
        }
        static void BulkDeleteSection()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countrys = db.Query<Section>("select * from Sections");
                db.BulkDelete(countrys.Where(x => x.Name == "Мед"));
                Console.WriteLine("Сделано -_-");
            }

            Console.ReadLine();
        }
        static void BulkDeletePromo()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countrys = db.Query<PromotionProduct>("select * from PromotionProducts");
                db.BulkDelete(countrys.Where(x => x.ProductName == "Италия"));
                Console.WriteLine("Сделано -_-");
            }

            Console.ReadLine();
        }

        static void CityCountry()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите номер страны");
                int num = int.Parse(Console.ReadLine()!);
                var country = db.Query<Country>("Select * from Countries").ToList()[num - 1];
                var city = db.Query<City>("Select * from Cities where CountryID = @Id", new { country.Id });
                int iter = 0;
                foreach(var a in city)
                {

                    Console.WriteLine($" {++iter} - {a.Name}");
                }
                Console.WriteLine("Сделано -_-");
            }

            Console.ReadLine();
        }
        static void CustSection()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите номер покупателя");
                int num = int.Parse(Console.ReadLine()!);


                var customers = db.Query<Customer>("SELECT * FROM Customers").ToList();


                if (num < 1 || num > customers.Count)
                {
                    Console.WriteLine("Некорректный номер покупателя!");
                    return;
                }

                var customer = customers[num - 1];

                var sections = db.Query<Section>(
                    "SELECT s.* FROM Sections s " +
                    "JOIN CustomerSections cs ON s.Id = cs.SectionID " +
                    "JOIN Customers c ON c.Id = cs.CustomerID " +
                    "WHERE c.Id = @CustomerId",
                    new { CustomerId = customer.Id }
                ).ToList();

                if (sections.Count == 0)
                {
                    Console.WriteLine("У данного покупателя нет разделов.");
                }
                else
                {
                    int iter = 0;
                    foreach (var section in sections)
                    {
                        Console.WriteLine($" {++iter} - {section.Name}");
                    }
                }

                Console.WriteLine("Сделано -_-");
            }

            Console.ReadLine();
        }
        static void CustSections()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите номер покупателя");
                int num = int.Parse(Console.ReadLine()!);


                var customers = db.Query<Customer>("SELECT * FROM Customers").ToList();


                if (num < 1 || num > customers.Count)
                {
                    Console.WriteLine("Некорректный номер покупателя!");
                    return;
                }

                var customer = customers[num - 1];

                var sections = db.Query<Section>(
                    "SELECT s.* FROM Sections s " +
                    "JOIN CustomerSections cs ON s.Id = cs.SectionID " +
                    "JOIN Customers c ON c.Id = cs.CustomerID " +
                    "WHERE c.Id = @CustomerId",
                    new { CustomerId = customer.Id }
                ).ToList();

                if (sections.Count == 0)
                {
                    Console.WriteLine("У данного покупателя нет разделов.");
                }
                else
                {
                    int iter = 0;
                    foreach (var section in sections)
                    {
                        Console.WriteLine($" {++iter} - {section.Name}");
                    }
                }

                Console.WriteLine("Сделано -_-");
            }

            Console.ReadLine();
        }

    }
}
