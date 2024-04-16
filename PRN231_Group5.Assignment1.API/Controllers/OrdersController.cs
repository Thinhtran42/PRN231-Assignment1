using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.Order;
using PRN231_Group5.Assignment1.Repo.VIewModels.OrderDetail;

namespace PRN231_Group5.Assignment1.API.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(DateTime? startDate, DateTime? endDate)
        {
            var orders = await _unitOfWork.OrderRepository.GetAsync(
       filter: o => (!startDate.HasValue || o.OrderDate >= startDate) && (!endDate.HasValue || o.OrderDate <= endDate),
       orderBy: q => q.OrderByDescending(order => order.OrderDate));
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIDAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderViewModel orderViewModel)
        {
            DateTime currentDateTime = DateTime.Now;

            var order = _mapper.Map<Order>(orderViewModel);

            order.OrderDate = currentDateTime;
            order.RequiredDate = currentDateTime.AddDays(1);
            order.ShippedDate = order.RequiredDate.Value.AddDays(1);

            if (order.ShippedDate <= order.RequiredDate || order.RequiredDate <= order.OrderDate)
            {
                return BadRequest("ShippedDate must be greater than RequiredDate, and RequiredDate must be greater than OrderDate.");
            }
            Random random = new Random();
            order.OrderId = random.Next();

            _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.Save();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, orderViewModel);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderViewModel orderViewModel)
        {
            var existedOrder = await _unitOfWork.OrderRepository.GetByIDAsync(id);

            if (existedOrder is null)
            {
                return NotFound();
            }

            var updateOrder = _mapper.Map(orderViewModel, existedOrder);

            _unitOfWork.OrderRepository.Update(updateOrder);
            _unitOfWork.Save();

            return NoContent();
        }

        // GET: api/orders/orderdetails
        [HttpGet("{id}/order-details")]
        public async Task<ActionResult<IEnumerable<OrderDetailViewModel>>> GetOrderDetailByOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(o => o.OrderId == id);
            if (order == null || !order.Any())
            {
                return NotFound($"Order with id {id} not found");
            }

            var orderDetails = await _unitOfWork.OrderDetailRepository.GetAsync(od => od.OrderId == id);

            var orderDetailViewModels = _mapper.Map<IEnumerable<OrderDetailViewModel>>(orderDetails);

            return Ok(orderDetailViewModels);
        }

        // PUT: api/orders/5/orderdetails
        [HttpPut("{id}/order-details")]
        public async Task<IActionResult> UpdateOrderDetailByOrderId(int id, [FromBody] IEnumerable<UpdateOrderDetailViewModel> models)
        {
            var existingOrderDetails = await _unitOfWork.OrderDetailRepository.GetAsync(od => od.OrderId == id);

            if (!existingOrderDetails.Any())
            {
                return NotFound($"Not have order detail with Order id {id}");
            }

            foreach (var model in models)
            {
                var orderDetail = existingOrderDetails.FirstOrDefault(od => od.Id == model.Id);
                if (orderDetail != null)
                {
                    var updatedOrderDetail = _mapper.Map(model, orderDetail);
                    _unitOfWork.OrderDetailRepository.Update(updatedOrderDetail);
                }
            }

            _unitOfWork.Save();

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var existedOrder = await _unitOfWork.OrderRepository.GetByIDAsync(id);
            if (existedOrder is null)
            {
                return NotFound();
            }
            _unitOfWork.OrderRepository.Delete(existedOrder);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
