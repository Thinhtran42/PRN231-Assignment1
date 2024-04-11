using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.Order;

namespace PRN231_Group5.Assignment1.API.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetAsync(includeProperties: "OrderDetails");
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        // GET: api/Order/5
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

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateProduct([FromBody] CreateOrderViewModel orderViewModel)
        {
            var order = _mapper.Map<Order>(orderViewModel);

            Random random = new Random();
            order.OrderId = random.Next();

            _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.Save();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, orderViewModel);
        }

        // PUT: api/Order/5
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

        // DELETE: api/Order/5
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
