﻿using EasyCashIdentityProject.DataAccessLayer.Abstract;
using EasyCashIdentityProject.DataAccessLayer.Repositories;
using EasyCashIdentityProject.EntityLayer.Concrete;

namespace EasyCashIdentityProject.DataAccessLayer.EntityFramework
{
    public class EfCustomerAccountProcessDal:GenericRepository<CustomerAccountProcess>,ICustomerAccountProcessDal
    {
    }
}
