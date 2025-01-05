using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCApp01DbContext _dbContext;

        public EmployeeRepository(MVCApp01DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
          return  _dbContext.Employees.Where(E=>E.Address==address);
        }

        public IQueryable<Employee> GetEmployeesByName(string searchValue)
        {
              return _dbContext.Employees.Include(X => X.Department).Where(E=>E.Name.ToLower().Contains(searchValue.ToLower())); 
        }
    }
}
