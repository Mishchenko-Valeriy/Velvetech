using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Velvetech
{
    public class Facade
    {
        protected  Group _group;
        protected  Student _student;

        public Facade(Group group, Student student)
        {
            this._group = group;
            this._student = student;
        }
        public Facade(Group group)
        {
            this._group = group;
        }
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
        public IEnumerable<Group> GroupGet()
        {
            return _group.GetList();
        }
    }
}