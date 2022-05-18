using library.elegalhubs.com.Ticketing;
using Microsoft.AspNetCore.Mvc;
using services.elegalhubs.com.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace services.elegalhubs.com.Repository
{
   public interface ICommanRepository<T>
    {
   
          
          Task<IEnumerable<T>> Index();


       
        Task<T> Details(string id);
        Task<IEnumerable<T>> Search(T ticketComman);


        [HttpPost]
        [ValidateAntiForgeryToken]
        Task<T> Create(T ticketComman);


        [HttpPost]
        [ValidateAntiForgeryToken]
        Task Edit(T ticketComman);

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        Task DeleteConfirmed(string id);
    }

}
