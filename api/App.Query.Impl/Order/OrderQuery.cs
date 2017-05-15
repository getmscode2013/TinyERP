﻿namespace App.Query.Impl.Order
{
    using System;
    using App.Common.Data;
    using App.Common.Data.MongoDB;
    using App.Query.Order;
    using App.Query.Entity.Order;
    using System.Linq;
    using ValueObject.Order;
    using System.Collections.Generic;
    using Common.Mapping;
    using Common;

    public class OrderQuery : BaseQueryRepository<Order>, IOrderQuery
    {
        public OrderQuery() : base(RepositoryType.MongoDb) { }

        public void ActivateOrder(Guid orderId)
        {
            App.Query.Entity.Order.Order order = this.DbSet.AsQueryable().FirstOrDefault(item => item.OrderId == orderId);
            order.IsActivated = true;
            this.DbSet.Update(order);
        }

        public void AddOrderLineItem(Guid orderId,Guid productId,string productName,int quantity, decimal price)
        {
            App.Query.Entity.Order.Order order = this.DbSet.AsQueryable().FirstOrDefault(item => item.OrderId == orderId);
            order.OrderLines.Add(new OrderLine(productId,productName, quantity, price));
            order.TotalItems += quantity;
            order.TotalPrice += price*(decimal)quantity;
            this.DbSet.Update(order);
        }
        public void CreateOrder(Guid orderId)
        {
            this.DbSet.Add(new App.Query.Entity.Order.Order(orderId));
        }

        public TEntity GetOrder<TEntity>(string id) where TEntity : IMappedFrom<Order>
        {
            return this.GetById<TEntity>(id);
        }

        public IList<TEntity> GetOrders<TEntity>() where TEntity : IMappedFrom<Order>
        {
            return this.GetItems<TEntity>();
        }

        public void UpdateCustomerDetail(Guid orderId, string customerName)
        {
            App.Query.Entity.Order.Order order = this.DbSet.AsQueryable().FirstOrDefault(item => item.OrderId == orderId);
            order.Name = customerName;
            this.DbSet.Update(order);
        }
    }
}
