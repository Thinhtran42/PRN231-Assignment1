using System;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.Repositories;

namespace PRN231_Group5.Assignment1.Repo.Interfaces
{
    public interface IUnitOfWork
    {
        GenericRepository<Member> MemberRepository { get; }
        GenericRepository<Product> ProductRepository { get; }
        GenericRepository<Category> CategoryRepository { get; }
        GenericRepository<Order> OrderRepository { get; }
        GenericRepository<OrderDetail> OrderDetailRepository { get; }
        void Save();
    }
}

