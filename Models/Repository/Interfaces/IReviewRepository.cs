using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IReviewRepository: IRepository<Review>
    {
        public bool Exists(string UserId, int ProductId);
    }
}
