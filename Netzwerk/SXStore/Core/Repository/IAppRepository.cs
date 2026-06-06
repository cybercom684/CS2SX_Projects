using SXStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXStore.Core.Repository
{
    public interface IAppRepository
    {
        IReadOnlyList<AppEntry> GetAll();
        IReadOnlyList<AppEntry> GetByCategory(string category);
        IReadOnlyList<AppEntry> GetFeatured();
        IReadOnlyList<AppEntry> Search(string query);
        AppEntry GetById(string id);
    }
}
