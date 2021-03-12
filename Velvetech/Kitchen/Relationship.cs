using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class Relationship
    {
        public int id { get; set; }
        public int GroupId { get; set; }
        public int StudentId { get; set; }

        public List<ValidationResult> Set(bool IsCreate, int GroupId, int StudentId)
        {
            // Проверка на существование группы и студента
            Group group = new Group().Get(GroupId);
            Student student = new Student().Get(StudentId);

            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (group.Id != 0 && student.Id != 0)
            {
                // Если входящие данные верны, то сначала всегда очищаем эту связь из БД
                DBConnect DbConnect = new DBConnect(true);

                DbConnect.DBExecute("DELETE Relationship WHERE GroupID=@0 AND StudentID=@1", GroupId, StudentId);

                if (IsCreate)
                {
                    DbConnect.DBExecute("INSERT INTO Relationship (GroupID, StudentID) VALUES (@0, @1)", GroupId, StudentId);
                }

                return validationResult;
            }
            else
            {
                validationResult.Add(new ValidationResult("The group or student does not exist"));
                return validationResult;
            }
        }

        public IEnumerable<Relationship> Get(bool isGroup, int Id)
        {
            DBConnect DbConnect = new DBConnect(true);
            IEnumerable<Relationship> relationshipList = Enumerable.Empty<Relationship>();
            IEnumerable<dynamic> result = null;

            if (isGroup)
            {
                // Если запрос по грппу - берем группу
                result = DbConnect.DBQuery("SELECT * FROM Relationship WHERE GroupID=@0", Id);
            }
            else
            {
                // Иначе - ищем связи по студенту
                result = DbConnect.DBQuery("SELECT * FROM Relationship WHERE StudentID=@0", Id);
            }

            foreach (var row in result)
            {
                Relationship relationship = new Relationship();
                relationship.GroupId = row.GroupID;
                relationship.StudentId = row.StudentID;

                relationshipList = relationshipList.Concat(new[] { relationship });
            }

            return relationshipList;
        }
    }
}
