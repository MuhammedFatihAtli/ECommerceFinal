using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _commentRepository = unitOfWork.CommentRepository;
        }

        public async Task AddCommentAsync(CommentDTO dto, int? userId)
        {
            var comment = _mapper.Map<Comment>(dto);
            comment.UserId = userId;
            comment.CreatedAt = DateTime.Now;
            comment.UserName = dto.UserName;
            await _unitOfWork.CommentRepository.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<CommentDTO>> GetCommentsByProductIdAsync(int productId)
        {
            var comments = await _commentRepository.GetAllAsync(c => c.ProductId == productId) ?? new List<Comment>();
            return comments.Select(c => _mapper.Map<CommentDTO>(c)).ToList();
        }

    }

}
