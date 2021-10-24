﻿using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var affected = await GetConnection().ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount VALUES (@ProductName, @Description, @Amount",
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var affected = await GetConnection().ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName = @ProductName",
                    new { ProductName = productName });

            return affected != 0;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await GetConnection().QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName});

            if(coupon is null)
            {
                coupon = new Coupon { ProductName = "NoDiscount", Amount = 0, Description = "No Discount Desc" };
            }

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var affected = await GetConnection().ExecuteAsync
                ("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            return affected != 0;
        }

        private NpgsqlConnection GetConnection()
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            return connection;
        }
    }
}