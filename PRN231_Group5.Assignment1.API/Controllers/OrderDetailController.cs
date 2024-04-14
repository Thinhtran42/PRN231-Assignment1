using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.OrderDetail;

namespace PRN231_Group5.Assignment1.API.Controllers
{
    public class OrderDetailController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public OrderDetailController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/OrderDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailViewModel>>> GetOderDetails()
        {
            var orderDetails = await _unitOfWork.OrderDetailRepository.GetAsync();
            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            var orderDetailViewModels = new List<OrderDetailViewModel>();
            foreach (var orderDetail in orderDetails)
            {
                var orderDetailViewModel = _mapper.Map<OrderDetailViewModel>(orderDetail);
                orderDetailViewModels.Add(orderDetailViewModel);
            }
            return Ok(orderDetailViewModels);
        }

        // GET: api/OrderDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIDAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }
        
        // POST: api/OrderDetail
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> CreateOrderDetail([FromBody] IEnumerable<OrderDetailViewModel> orderDetailsViewModels)
        {
            var orderDetails = new List<OrderDetail>();
            foreach (var orderDetailViewModel in orderDetailsViewModels)
            {
                var orderDetail = _mapper.Map<OrderDetail>(orderDetailViewModel);
                orderDetails.Add(orderDetail);
            }
            
            await _unitOfWork.OrderDetailRepository.InsertRangeAsync(orderDetails);
            _unitOfWork.Save();

            var orderViewModels = orderDetails.Select(od => _mapper.Map<OrderDetailViewModel>(od));
            return CreatedAtAction("GetOrder", new { id = orderDetails.FirstOrDefault().OrderId }, orderViewModels);
        }

        // PUT: api/OrderDetail/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderByOrderId(int id, [FromBody] UpdateOrderDetailViewModel model)
        {
            var existedOrderDetail = await _unitOfWork.OrderDetailRepository.GetByIDAsync(id);

            if (existedOrderDetail is null)
            {
                return NotFound();
            }

            var updateOrderDetail = _mapper.Map(model, existedOrderDetail);

            _unitOfWork.OrderDetailRepository.Update(updateOrderDetail);
            _unitOfWork.Save();

            return NoContent();
        }

        // DELETE: api/OrderDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var existedOrderDetail = await _unitOfWork.OrderDetailRepository.GetByIDAsync(id);
            if (existedOrderDetail is null)
            {
                return NotFound();
            }
            _unitOfWork.OrderRepository.Delete(existedOrderDetail);
            _unitOfWork.Save();

            return NoContent();
        }
        
    }
}
