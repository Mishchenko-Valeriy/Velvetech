using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class Group
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name - required attribute")]
        [StringLength(25, ErrorMessage = "The group name exceeds the maximum character length of 25")]
        public string Name { get; set; }
        public int StudentsCount { get; set; }

        public List<ValidationResult> Add(string Name) // Добавление новой группы
        {
            Group group = new Group { Name = Name };
            List<ValidationResult> validationResult = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(group);

            if (!Validator.TryValidateObject(group, context, validationResult, true))
            {
                return validationResult;
            }

            // Если валидация прошла без ошибок, то продолжаем
            DBConnect DbConnect = new DBConnect(true);

            DbConnect.DBExecute("INSERT INTO Groups (Name) VALUES (@0)", Name);

            return validationResult;
        }
        public List<ValidationResult> Update(int Id, string Name) // Обновление группы
        {
            Group group = new Group { Id = Id, Name = Name };
            List<ValidationResult> validationResult = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(group);

            if (!Validator.TryValidateObject(group, context, validationResult, true))
            {
                return validationResult;
            }
            if (Id < 1)
            {
                validationResult.Add(new ValidationResult("Id - is not an identifier"));
                return validationResult;
            }
            // Если валидация прошла без ошибок, то продолжаем
            DBConnect DbConnect = new DBConnect(true);

            DbConnect.DBExecute("UPDATE Groups SET Name=@0 WHERE Id=@1", Name, Id);

            return validationResult;
        }
        public List<ValidationResult> Delete(int Id) // Удаление группы
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (Id < 1)
            {
                validationResult.Add(new ValidationResult("Id - is not an identifier"));
                return validationResult;
            }
            // Если валидация прошла без ошибок, то продолжаем
            DBConnect DbConnect = new DBConnect(true);

            // Сначала - удаляем связи с студентами
            DbConnect.DBExecute("DELETE Relationship WHERE GroupID=@0", Id);

            DbConnect.DBExecute("DELETE Groups WHERE Id=@0", Id);

            return validationResult;
        }

        public Group Get(int Id)
        {
            Group group = new Group();
            if (Id < 1)
            {
                return group;
            }
            // Если валидация прошла без ошибок, то продолжаем
            DBConnect DbConnect = new DBConnect(true);

            dynamic result = DbConnect.DBQuerySingle("SELECT * FROM Groups WHERE Id=@0", Id);

            if(result != null)
            {
                group.Id = result.Id;
                group.Name = result.Name;

                IEnumerable<Relationship> relationshipList = new Relationship().Get(true, result.Id);
                group.StudentsCount = relationshipList.Count();
            }

            return group;
        }

        public IEnumerable<Group> GetList(string name = "")
        {
            DBConnect DbConnect = new DBConnect(true);
            IEnumerable<Group> groupList = Enumerable.Empty<Group>();

            // Осуществим фильтр по имени прямо в SQL запросе
            foreach (var row in DbConnect.DBQuery("SELECT * FROM Groups WHERE Name LIKE N'%" + name +  "%'"))
            {
                Group Item = new Group();
                Item.Id = row.Id;
                Item.Name = row.Name;

                IEnumerable<Relationship> relationshipList = new Relationship().Get(true, row.Id);
                Item.StudentsCount = relationshipList.Count();

                groupList = groupList.Concat(new[] { Item });
            }

            return groupList;
        }
    }
}
