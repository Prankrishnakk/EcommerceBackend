using EcommerceBackend.Context;
using EcommerceBackend.Dto;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceBackend.Services.OrderServices
{
    public class OrderServices : IOrderServices
    {
        public AppDbContext _context { get; set; }
        public OrderServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Indidvidual_ProductBuy(int userId, int productId, CreateOrderDto order_Dto)
        {
            try
            {
                var pro = await _context.products.FirstOrDefaultAsync(a => a.Id == productId);
                if (pro == null)
                {
                    throw new Exception("Product not found");
                }

                if (pro.StockId <= 0)
                {
                    throw new Exception("Product is out of stock");
                }

          
                var address = await _context.userAddresses
                    .FirstOrDefaultAsync(a => a.Id == order_Dto.AddId && a.UserId == userId);

                if (address == null)
                {
                    throw new Exception("Invalid address. You can only use your own address.");
                }
                var new_order = new Models.Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    AddressId = order_Dto.AddId,
                    //Total = order_Dto.Total ?? 0m,
                    //OrderString = order_Dto.OrderString,
                    //TransactionId = order_Dto.TransactionId,
                    _Items = new List<OrderItems>()
                };

                var orderItem = new OrderItems
                {
                    ProductId = pro.Id,
                    Quantity = 1,
                    TotalPrice = pro.offerPrize * 1
                };

                new_order._Items?.Add(orderItem);
                _context.orders.Add(new_order);

                pro.StockId -= 1;
                _context.products.Update(pro);

                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving changes: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        public async Task<bool> CrateOrder_CheckOut(int userId, CreateOrderDto createOrderDto)
        {
            try
            {
                var cart = await _context.cart
                    .Include(b => b._Items)
                    .ThenInclude(c => c._Product)
                    .FirstOrDefaultAsync(z => z.UserId == userId);

                if (cart == null || cart._Items.Count == 0)
                {
                    throw new Exception("Cart is empty");
                }

                decimal total = cart._Items.Sum(item => item._Product.offerPrize * item.ProductQty);
                string orderString = string.Join(", ", cart._Items.Select(item => item._Product.ProductName));
                string transactionId = $"TXN{DateTime.Now.Ticks}"; // Or use Guid.NewGuid().ToString()


                var order = new Models.Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    AddressId = createOrderDto.AddId,
                    Total = total,
                    OrderString = orderString,
                    TransactionId = transactionId,
                    _Items = cart._Items.Select(a => new OrderItems
                    {
                        ProductId = a._Product.Id,
                        Quantity = a.ProductQty,
                        TotalPrice = a._Product.offerPrize * a.ProductQty

                    }).ToList()
                };

                foreach (var item in cart._Items)
                {
                    var pro = await _context.products.FirstOrDefaultAsync(a => a.Id == item.ProductId);
                    if (pro != null)
                    {
                        if (pro.StockId <= item.ProductQty)
                        {
                            return false;
                        }

                        pro.StockId -= item.ProductQty;
                    }
                }

                await _context.orders.AddAsync(order);
                _context.cart.Remove(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<OrderAdminViewDto>> GetOrderDetailsAdmin()
        {
            try
            {
                var orders = await _context.orders
                    .Include(z => z._UserAd)
                    .Include(a => a._Items).ToListAsync();
                if (orders.Count > 0)
                {
                    var details = orders.Select(a => new OrderAdminViewDto
                    {
                        OrderId = a.Id,
                        OrderDate = a.OrderDate,
                        OrderString = a.OrderString,
                        OrderStatus = a.OrderStatus,
                        TransactionId = a.TransactionId,
                        UserName = a._UserAd.CustomerName,
                        Phone = a._UserAd.CustomerPhone,
                        UserAddress = a._UserAd.HomeAddress,


                    }).ToList();

                    return details;
                }

                return new List<OrderAdminViewDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<OrderviewDto>> GetOrderDetails(int userId)
        {
            try
            {
                var orders = await _context.orders
                     .Include(a => a._Items)
                     .ThenInclude(b => b.Product)
                     .Where(c => c.UserId == userId)
                     .ToListAsync();

                var orderDetails = orders
                    .Where(r => r._Items?.Count > 0)
                    .Select(a => new OrderviewDto
                    {
                        OrderId = a.Id,
                        OrderDate = a.OrderDate,
                        OrderStatus = a.OrderStatus,
                        OrderString = a.OrderString,
                        TransactionId = a.TransactionId,
                        Items = a._Items.Select(b => new OrderItemDto
                        {
                            OrderItemId = b.OrderId,
                            OrderId = b.OrderId,
                            ProductId = b.ProductId,
                            ProductName = b.Product.ProductName,
                            Quantity = b.Quantity,
                            TotalPrice = b.TotalPrice,
                        }).ToList(),
                    }).ToList();



                return orderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> TotalRevenue()
        {
            try
            {
                var Total = await _context.orderItems.SumAsync(i => i.TotalPrice);
                return Convert.ToDecimal(Total);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> TotalProductsPurchased()
        {
            try
            {
                var total_pro = await _context.orderItems.SumAsync(i => i.Quantity);
                return Convert.ToInt32(total_pro);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<OrderviewDto>> GetOrderDetailsAdmin_byuserId(int userId)
        {
            try
            {
                var orders = await _context.orders.Include(a => a._Items)
                   .ThenInclude(b => b.Product)
                   .Where(c => c.UserId == userId)
                   .ToListAsync();

                var orderDetails = orders.Select(a => new OrderviewDto
                {
                    OrderId = a.Id,
                    OrderDate = a.OrderDate,
                    OrderStatus = a.OrderStatus,
                    OrderString = a.OrderString,
                    TransactionId = a.TransactionId,
                    Items = a._Items.Select(b => new OrderItemDto
                    {
                        OrderItemId = b.OrderId,
                        OrderId = b._Order.Id,
                        ProductId = b.ProductId,
                        ProductImage = b.Product.ImageUrl,
                        ProductName = b.Product.ProductName,
                        Quantity = b.Quantity,
                        TotalPrice = b.TotalPrice,
                    }).ToList(),
                }).ToList();

                return orderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> UpdateOrderStatus(int oId)
        {
            try
            {
                var order = await _context.orders.FirstOrDefaultAsync(a => a.Id == oId);

                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                //const string? OrderPlaced = "OrderPlaced";
                //const string? Delivered = "Delivered";


                order.OrderStatus = "Delivered";



                await _context.SaveChangesAsync();
                return order.OrderStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }



    }
}
