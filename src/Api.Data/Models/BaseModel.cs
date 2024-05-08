using System;

namespace Api.Domain.Models
{
    public class BaseModel
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _createAt;
        public DateTime CreateAt
        {
            get { return _createAt; }
            set
            {
                _createAt = DateTime.UtcNow;
            }
        }

        private DateTime _updateAt;
        public DateTime UpdateAt
        {
            get { return _updateAt; }
            set { _updateAt = value; }
        }
    }
}
