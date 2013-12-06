using Everis.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everis.Core.Services
{
    public interface ICRUDService<TENTITY, TKEY> where TENTITY : Entity<TKEY>
    {
        TENTITY GetById(TKEY id);

        void CreateOrUpdate(TENTITY product);
        void Delete(TENTITY product);
    }
}
