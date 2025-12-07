using Joseco.DDD.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticaService.Domain.Users
{
    public class User : AggregateRoot
    {
        public string FullName { get; private set; }

        internal User(Guid id, string fullname) : base(id)
        {
            FullName = fullname;
        }
        public static User Create(Guid id, string fullname)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id cannot be empty", nameof(id));
            }
            if (string.IsNullOrWhiteSpace(fullname))
            {
                throw new ArgumentNullException("fullname cannot be null or  empty", nameof(fullname));
            }
            return new User(id, fullname);
        }

        public void update(string fullname)
        {
            if (string.IsNullOrEmpty(fullname))
            {
                throw new ArgumentNullException("fullname cannot be null or  empty", nameof(fullname));
            }
            fullname = fullname.Trim();
        }


        private User() : base()
        {

        }
    }
}
