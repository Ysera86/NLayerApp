using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    // Decorator Design Pattern
    // Proxy Design Pattern
    public class ProductServiceWithCaching : IProductService
    {
        private const string _cacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;

            if (!_memoryCache.TryGetValue(_cacheProductKey, out _)) // _ memoryde allocate yapmasın dönen datayı istemiyorum çnk
            {
                //_memoryCache.Set(_cacheProductKey, _repository.GetAll().ToList());

                // ctor içinde asenkron kullanamam
                _memoryCache.Set(_cacheProductKey, _repository.GetProductsWithCategoryAsync().Result); 
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();

            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(_cacheProductKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(_cacheProductKey).FirstOrDefault(x => x.Id == id);
            // buna gerek yok, controllerda NotFoundFilter uyguladık bu action için zaten
            //if (product == null)
            //{
            //    throw new NotFoundException($"{typeof(T).Name} with Id with {id} not found");
            //}
            //  return Task.FromResult(product!); // product null değil biliyorum dedim
            return Task.FromResult(product);
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategoryAsync()
        {
            // ctor içinde hali hazırda cache e kategorileri de olan dataları bastık, nasılsa GetAll içinde dönülen tipte(ProductDto) kategoriler yok, dolayısıyla aynı olmamış oldu, burası da cacheten çalışır hale getirilince burada dönülen tipte kategori olduğundan mantıksız kaçmadı
            //var products = await _repository.GetProductsWithCategoryAsync();
            var products = _memoryCache.Get<IEnumerable<Product>>(_cacheProductKey);
            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsWithCategoryDto));
        }

        public async Task RemoveAsync(Product entity)
        {
             _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(_cacheProductKey).Where(expression.Compile()).AsQueryable();
        }



        public async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(_cacheProductKey, await _repository.GetAll().ToListAsync());
        }
    }
}
