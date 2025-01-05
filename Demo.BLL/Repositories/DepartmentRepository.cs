using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository :GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MVCApp01DbContext dbContext):base(dbContext)
        {
            
        }
        //private readonly MVCApp01DbContext _dbContext;

        //public DepartmentRepository(MVCApp01DbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        //public int Add(Department department)
        //{
        //    _dbContext.Add(department);
        //    return _dbContext.SaveChanges();
        //}

        //public int Delete(Department department)
        //{
        //    _dbContext.Remove(department);
        //    return _dbContext.SaveChanges();
        //}

        //public IEnumerable<Department> GetAll()
        //{
        //    return _dbContext.Departments.ToList(); 
        //}

        //public Department GetById(int id)
        //{
        //    return _dbContext.Departments.Find(id);
        //}

        //public int Update(Department department)
        //{
        //    _dbContext.Update(department);
        //    return _dbContext.SaveChanges();
        //}
    }
}
