using System;
using Microsoft.EntityFrameworkCore;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.Repositories;

namespace PRN231_Group5.Assignment1.Repo
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FstoreDbContext _context;
        private GenericRepository<Member> memberRepository;
        private GenericRepository<Product> productRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<OrderDetail> orderDetailRepository;

        public UnitOfWork(FstoreDbContext context)
        {
            _context = context;
        }

        private bool disposed = false;

        public GenericRepository<Member> MemberRepository
        {
            get
            {
                if (memberRepository == null)
                {
                    memberRepository = new GenericRepository<Member>(_context);
                }
                return memberRepository;
            }
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new GenericRepository<Product>(_context);
                }
                return productRepository;
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new GenericRepository<Order>(_context);
                }
                return orderRepository;
            }
        }

        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new GenericRepository<Category>(_context);
                }
                return categoryRepository;
            }
        }

        public GenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {
                if (orderDetailRepository == null)
                {
                    orderDetailRepository = new GenericRepository<OrderDetail>(_context);
                }
                return orderDetailRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

