using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class Facade
    {
        protected Group _group;
        protected Student _student;
        protected Relationship _relationship;

        public Facade(Group group)
        {
            this._group = group;
        }
        public Facade(Student student)
        {
            this._student = student;
        }
        public Facade(Relationship relationship)
        {
            this._relationship = relationship;
        }

        #region Groups methods
        public List<ValidationResult> GroupSet(GroupStringAdd message)
        {
            return _group.Add(message.Name);
        }
        public List<ValidationResult> GroupSet(GroupStringPut message)
        {
            return _group.Update(message.Id, message.Name);
        }
        public List<ValidationResult> GroupDel(int Id)
        {
            return _group.Delete(Id);
        }
        public Group GroupGet(int Id)
        {
            return _group.Get(Id);
        }
        public IEnumerable<Group> GroupGet(string name = "")
        {
            return _group.GetList(name);
        }
        #endregion

        #region Students methods
        public List<ValidationResult> StudentSet(StudentStringAdd message)
        {
            return _student.Add(message);
        }
        public List<ValidationResult> StudentSet(StudentStringPut message)
        {
            return _student.Update(message);
        }
        public List<ValidationResult> StudentDel(int Id)
        {
            return _student.Delete(Id);
        }
        public IEnumerable<Student> StudentGet(StudentStringGet message)
        {
            return _student.GetList(message);
        }
        #endregion

        #region Relationship methods
        public List<ValidationResult> RelationshipSet(RelationshipString message)
        {
            return _relationship.Set(true, message.GroupID, message.StudentID);
        }
        public List<ValidationResult> RelationshipDel(int GroupId, int StudentId)
        {
            return _relationship.Set(false, GroupId, StudentId);
        }
        #endregion
    }
}