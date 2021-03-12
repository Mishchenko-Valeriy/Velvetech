using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ItMan - required attribute")]
        public bool ItMan { get; set; } // Не лаконично, но быстро (я не сексист ))) )

        [Required(ErrorMessage = "Surname - required attribute")]
        [StringLength(40, ErrorMessage = "The student surname exceeds the maximum character length of 40")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Name - required attribute")]
        [StringLength(40, ErrorMessage = "The student name exceeds the maximum character length of 40")]
        public string Name { get; set; }


        [StringLength(60, ErrorMessage = "The student patronymic exceeds the maximum character length of 60")]
        public string Patronymic { get; set; }

        [StringLength(16, ErrorMessage = "The student callsign exceeds the maximum character length of 6 - 16")]
        public string Callsign { get; set; }
        public string Groups { get; set; }
        public string PageInfo { get; set; }

        // По мимо атрибутов с валидацией - добавляем конструктор класса с 3 перегрузками (присвоение входных данных)
        public Student() { }
        private Student(StudentStringAdd StudentString)
        {
            ItMan = StudentString.ItMan;
            Surname = StudentString.Surname;
            Name = StudentString.Name;
            Patronymic = StudentString.Patronymic;
            Callsign = StudentString.Callsign;
        }
        private Student(StudentStringPut StudentString)
        {
            Id = StudentString.Id;
            ItMan = StudentString.ItMan;
            Surname = StudentString.Surname;
            Name = StudentString.Name;
            Patronymic = StudentString.Patronymic;
            Callsign = StudentString.Callsign;
        }

        public List<ValidationResult> Add(StudentStringAdd StudentString) // Добавление новго студента
        {
            Student student = new Student (StudentString);
            List<ValidationResult> validationResult = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(student);

            if (!Validator.TryValidateObject(student, context, validationResult, true))
            {
                return validationResult;
            }
            if(student.Callsign != "" && student.Callsign.Length < 6)
            {
                validationResult.Add(new ValidationResult("Callsign is not unique"));
                return validationResult;
            }

            // Если валидация прошла без ошибок, то продолжаем
            DBConnect DbConnect = new DBConnect(true);

            // Если есть позывной - проверяем его на уникальность
            if (student.Callsign != "" && DBConnect.Query("SELECT Id FROM Students WHERE Callsign=@0", student.Callsign).Count() > 0)
            {
                validationResult.Add(new ValidationResult("Callsign is not unique"));
                return validationResult;
            }

            // Добавляем нового студента
            DbConnect.DBExecute("INSERT INTO Students (ItMan, Surname, Name, Patronymic, Callsign)" + 
                " VALUES (@0, @1, @2, @3, @4)", student.ItMan, student.Surname, student.Name, student.Patronymic, student.Callsign);

            return validationResult;
        }

        public List<ValidationResult> Update(StudentStringPut StudentString) // Обновление студента
        {
            Student student = new Student(StudentString);
            List<ValidationResult> validationResult = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(student);

            if (!Validator.TryValidateObject(student, context, validationResult, true))
            {
                return validationResult;
            }
            if (student.Id < 1)
            {
                validationResult.Add(new ValidationResult("Id - is not an identifier"));
                return validationResult;
            }
            // Если валидация прошла без ошибок, то продолжаем
            DBConnect DbConnect = new DBConnect(true);

            // Проверяем позывной на уникальность
            if(student.Callsign != "")
            {
                dynamic result = DbConnect.DBQuerySingle("SELECT Id FROM Students WHERE Callsign=@0 AND Id <> @1", student.Callsign, student.Id); // Возможно, этот позывной пренадлежит текущему Id?

                if (result != null)
                {
                    validationResult.Add(new ValidationResult("Id - is not an identifier"));
                    return validationResult;
                }
            }

            // Обновляем студента
            DbConnect.DBExecute("UPDATE Students SET ItMan=@0, Surname=@1, Name=@2, Patronymic=@3, Callsign=@4" +
                " WHERE Id=@5", student.ItMan, student.Surname, student.Name, student.Patronymic, student.Callsign, student.Id);

            return validationResult;
        }

        public List<ValidationResult> Delete(int Id) // Удаление студента
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (Id < 1)
            {
                validationResult.Add(new ValidationResult("Id - is not an identifier"));
                return validationResult;
            }
            // Если валидация прошла без ошибок, то продолжаем
            DBConnect DbConnect = new DBConnect(true);

            // Сначала - удаляем связи с группами
            DbConnect.DBExecute("DELETE Relationship WHERE StudentID=@0", Id);

            DbConnect.DBExecute("DELETE Students WHERE Id=@0", Id);

            return validationResult;
        }

        public Student Get(int Id)
        {
            Student student = new Student();
            if (Id < 1)
            {
                return student;
            }
            // Если валидация прошла без ошибок, то продолжаем
            DBConnect DbConnect = new DBConnect(true);

            dynamic result = DbConnect.DBQuerySingle("SELECT * FROM Students WHERE Id=@0", Id);

            if (result != null)
            {
                student.Id = result.Id;
                student.ItMan = result.ItMan;
                student.Surname = result.Surname;
                student.Name = result.Name;
                student.Patronymic = result.Patronymic;
                student.Callsign = result.Callsign;
            }

            return student;
        }
        public IEnumerable<Student> GetList(StudentStringGet message)
        {
            DBConnect DbConnect = new DBConnect(true);
            IEnumerable<Student> studentList = Enumerable.Empty<Student>();
            IEnumerable<Group> groupList = new Group().GetList(); // Заранее получаем список всех групп
            int pageSize = 5;
            int pageNow = 1;
            if(message.Page > 0)
            {
                pageNow = message.Page;
            }

            // Получаем полный источник
            foreach (var row in DbConnect.DBQuery("SELECT * FROM Students"))
            {
                Student Item = new Student();
                Item.Id = row.Id;
                Item.ItMan = row.ItMan;
                Item.Surname = row.Surname;
                Item.Name = row.Name;
                Item.Patronymic = row.Patronymic;
                Item.Callsign = row.Callsign;

                // Заполняем список групп для студента
                IEnumerable<Relationship> relationshipList = new Relationship().Get(false, Item.Id);
                StringBuilder studentGroups = new StringBuilder();
                foreach(var relationship in relationshipList)
                {
                    // Ищем название группы из общего списка групп и конкатенируем через StringBuilder
                    foreach (var groupRow in groupList)
                    {
                        if(relationship.GroupId == groupRow.Id)
                        {
                            studentGroups.Append(groupRow.Name + ", ");
                            break;
                        }
                    }
                }

                string studentGroupsSTR = studentGroups.ToString();
                // Удаляем лишнюю запятую в группах
                if(studentGroupsSTR.Length > 0)
                {
                    studentGroupsSTR = studentGroupsSTR.Remove(studentGroupsSTR.Length - 2);
                }
                Item.Groups = studentGroupsSTR;

                studentList = studentList.Concat(new[] { Item });
            }

            // Формируем отфильтрованный список
            IEnumerable<Student> studentListFilter = studentList.Where(i => i.Surname.Contains(message.Surname) &&
                                                                            i.Name.Contains(message.Name) &&
                                                                            i.Patronymic.Contains(message.Patronymic) &&
                                                                            i.Callsign.Contains(message.Callsign) &&
                                                                            i.Groups.Contains(message.Group) &&
                                                                            i.ItMan == message.ItMan);
            // Пагинация
            int count = 0;
            foreach (var row in studentListFilter)
            {
                count++;
                row.PageInfo = count + " of " + studentListFilter.Count();
            }
            IEnumerable<Student> studentListPag = studentListFilter.Skip(pageSize * (pageNow - 1)).Take(pageSize);

            return studentListPag;
        }
    }
}
