using SciencePortalMVC.Models;
using System;
using System.Linq;

namespace SciencePortalMVC.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SciencePortalDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Departments.Any()) { return; }

            var random = new Random();

            // --- 1. Кафедры (10 штук) ---
            var departments = Enumerable.Range(1, 10).Select(i => new Department
            {
                Name = $"Кафедра {i}",
                Profile = $"Профиль {i}"
            }).ToList();
            context.Departments.AddRange(departments);
            context.SaveChanges();

            // --- 2. Преподаватели (50 штук) ---
            var teachers = Enumerable.Range(1, 50).Select(i => new Teacher
            {
                FullName = $"Преподаватель {i}",
                Position = "Должность " + (i % 5 + 1),
                Degree = "Степень " + (i % 3 + 1),
                DepartmentId = departments[random.Next(departments.Count)].DepartmentId
            }).ToList();
            context.Teachers.AddRange(teachers);
            context.SaveChanges();

            // --- 3. Проекты (30 штук) ---
            var projects = Enumerable.Range(1, 30).Select(i => new Project
            {
                Name = $"Проект {i}",
                Number = $"P-{i}",
                FundingOrg = "Фонд " + (i % 4 + 1),
                StartDate = DateTime.Now.AddDays(-i * 15),
                LeaderId = teachers[random.Next(teachers.Count)].TeacherId
            }).ToList();
            context.Projects.AddRange(projects);
            context.SaveChanges();

            // --- 4. Публикации (100 штук) ---
            var publications = Enumerable.Range(1, 100).Select(i => new Publication
            {
                Title = $"Публикация {i}",
                Type = (i % 3 == 0) ? "Статья" : "Тезисы",
                Year = 2020 + (i % 6)
            }).ToList();

            // Добавляем случайных авторов к публикациям
            foreach (var pub in publications)
            {
                var authorsCount = random.Next(1, 4); // От 1 до 3 авторов
                for (int i = 0; i < authorsCount; i++)
                {
                    var randomTeacher = teachers[random.Next(teachers.Count)];
                    if (!pub.Teachers.Contains(randomTeacher))
                    {
                        pub.Teachers.Add(randomTeacher);
                    }
                }
            }
            context.Publications.AddRange(publications);
            context.SaveChanges();
        }
    }
}