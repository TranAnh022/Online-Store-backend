using System.Runtime.CompilerServices;
using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class ProductService : BaseService<Product, ProductReadDto, ProductCreateDto, ProductUpdateDto, ProductQueryOptions>, IProductService
    {
        private IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;


        private IProductImageRepository _productImageRepository;

        public ProductService(IProductRepository productRepository, IMapper mapper, IProductImageRepository productImageRepository, ICategoryRepository categoryRepository) : base(productRepository, mapper)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ProductReadDto> SetProductImagesAsync(Guid productId, IEnumerable<ProductImageCreateDto> imageDtos)
        {
            var product = await _productRepository.GetByIdAsync(productId) ?? throw new KeyNotFoundException("Product not found.");
            var images = imageDtos.Select(dto => new ProductImage(productId, dto.Url));
            product.SetImages(images);
            await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductReadDto>(product);
        }

        public async Task<IEnumerable<ProductReadDto>> GetMostPurchased(int topNumber)
        {
            var products = await _productRepository.GetMostPurchasedProductsAsync(topNumber) ?? throw new InvalidOperationException("Unable to fetch the most purchased products.");
            if (!products.Any())
                throw new InvalidOperationException("No products found.");

            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<ProductReadDto> UpdateProductDetailsAsync(Guid productId, ProductUpdateDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(productId) ?? throw new KeyNotFoundException("Product not found.");
            product.UpdateDetails(updateDto.Title, updateDto.Price, updateDto.Description);
            await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductReadDto>(product);
        }

        public async Task<ProductReadDto> UpdateProductCategoryAsync(Guid productId, Guid newCategoryId)
        {
            var product = await _productRepository.GetByIdAsync(productId) ?? throw new KeyNotFoundException("Product not found.");
            var category = await _categoryRepository.GetByIdAsync(newCategoryId) ?? throw new KeyNotFoundException("Category not found.");
            product.UpdateCategory(category.Id);
            await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductReadDto>(product);
        }

        public async Task<ProductReadDto> AdjustProductInventoryAsync(Guid productId, int adjustment)
        {
            var product = await _productRepository.GetByIdAsync(productId) ?? throw new KeyNotFoundException("Product not found.");
            product.AdjustInventory(adjustment);
            await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductReadDto>(product);
        }

        public override async Task<ProductReadDto> CreateOneAsync(ProductCreateDto createDto)
        {
            
            var category = await _categoryRepository.GetByIdAsync(createDto.CategoryId);
            if (category == null)
            {
                // Handle the case where the category is not found
                throw new KeyNotFoundException("Category not found");
            }
            var product = new Product(
               title: createDto.Title,
               description: createDto.Description,
               price: createDto.Price,
               inventory: createDto.Inventory,
               categoryId: createDto.CategoryId
           );
            var createdProduct = await _productRepository.AddAsync(product);

            if (createDto.Images != null)
            {
                foreach (var imageUrl in createDto.Images)
                {
                    var image = new ProductImage(productId: product.Id, url: imageUrl);
                    await _productImageRepository.AddAsync(image);
                }
            }
            // Add the new Product entity to the repository

            // Map the created Product entity back to a ProductReadDto and return it
            var productReadDto = _mapper.Map<ProductReadDto>(createdProduct);
            return productReadDto;
        }

        public override async Task<ProductReadDto> UpdateOneAsync(Guid id, ProductUpdateDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Product not found for update");
            // Update product information if provided in the update DTO

            if (!string.IsNullOrEmpty(updateDto.Title))
            {

                product.Title = updateDto.Title;
            }

            if (updateDto.Price.HasValue)
            {

                product.Price = updateDto.Price.Value;
            }

            if (updateDto.CategoryId != null)
            {

                product.CategoryId = updateDto.CategoryId.Value;
            }

            if (updateDto.Inventory.HasValue)
            {
                product.Inventory = updateDto.Inventory.Value;
            }

            // Update product images
            if (updateDto.Images != null && updateDto.Images.Any())
            {
                Console.WriteLine("Work");
                foreach (var imageURL in updateDto.Images)
                {
                    Console.WriteLine("Work here");
                    {
                        var existingImage = await _productImageRepository.CheckImageAsync(imageURL);
                        if (existingImage == false)
                        {
                            Console.WriteLine("Work here");
                            var newImage = new ProductImage(productId: product.Id, url: imageURL);
                            await _productImageRepository.AddAsync(newImage);
                        }
                    }
                }
            }
            Console.WriteLine($"Entity after update: {product}");

            var newProduct = await _repository.UpdateAsync(product);
            return _mapper.Map<ProductReadDto>(newProduct);
        }
    }
}