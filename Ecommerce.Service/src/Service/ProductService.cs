using System.Runtime.CompilerServices;
using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.Service
{
    public class ProductService : BaseService<Product, ProductReadDto, ProductCreateDto, ProductUpdateDto, ProductQueryOptions>, IProductService
    {
        private IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        private readonly ICloudinaryService _cloudinaryService;
        private IProductImageRepository _productImageRepository;

        public ProductService(IProductRepository productRepository, ICloudinaryService cloudinaryService, IMapper mapper, IProductImageRepository productImageRepository, ICategoryRepository categoryRepository) : base(productRepository, mapper)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _categoryRepository = categoryRepository;
            _cloudinaryService = cloudinaryService;
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
                throw CustomExeption.NotFoundException("Category not found");
            }
            var product = new Product(
               title: createDto.Title,
               description: createDto.Description,
               price: createDto.Price,
               inventory: createDto.Inventory,
               categoryId: createDto.CategoryId
           );

            var images = new List<ProductImage>();

            // Process ImageUrls
            if (createDto.ImageUrls != null)
            {
                foreach (var imageUrl in createDto.ImageUrls)
                {
                    var productImage = new ProductImage(product.Id, imageUrl);
                    images.Add(productImage);
                }
            }

            // Process ImageFiles
            if (createDto.ImageFiles != null)
            {
                foreach (var file in createDto.ImageFiles)
                {
                    var imageUrl = await _cloudinaryService.AddPhoto(file);
                    var productImage = new ProductImage(product.Id, imageUrl);
                    images.Add(productImage);
                }
            }

            product.SetImages(images);
            // Add the new Product entity to the repository
            await _productRepository.AddAsync(product);
            // Map the created Product entity back to a ProductReadDto and return it
            var productReadDto = _mapper.Map<ProductReadDto>(product);
            return productReadDto;
        }

        public override async Task<ProductReadDto> UpdateOneAsync(Guid id, ProductUpdateDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(id) ?? throw CustomExeption.NotFoundException("Product not found");
            // Update product information if provided in the update DTO

            if (!string.IsNullOrEmpty(updateDto.Title))
            {

                product.Title = updateDto.Title;
            }

            if (updateDto.Price.HasValue)
            {

                product.Price = updateDto.Price.Value;
            }

            if (!string.IsNullOrEmpty(updateDto.Description))
            {

                product.Description = updateDto.Description;
            }

            if (updateDto.CategoryId != null)
            {

                product.CategoryId = updateDto.CategoryId.Value;
            }

            if (updateDto.Inventory.HasValue)
            {
                product.Inventory = updateDto.Inventory.Value;
            }

            // Update product imagesURL
            if (updateDto.ImageUrls != null && updateDto.ImageUrls.Any())
            {
                foreach (var imageURL in updateDto.ImageUrls)
                {
                    {
                        var existingImage = await _productImageRepository.CheckImageAsync(imageURL);
                        if (existingImage == false)
                        {
                            var newImage = new ProductImage(productId: product.Id, url: imageURL);
                            await _productImageRepository.AddAsync(newImage);
                        }
                    }
                }
            }

            //Update producy imageFile
            if (updateDto.ImageFiles != null && updateDto.ImageFiles.Any())
            {
                var ImageProductList = await _productImageRepository.GetByProductIdAsync(product.Id);

                foreach (var imageFile in ImageProductList)
                {
                    await _cloudinaryService.DeletePhoto(imageFile.Id);
                    await _productImageRepository.DeleteAsync(imageFile.Id);
                }

                foreach (var file in updateDto.ImageFiles)
                {
                    var imageUrl = await _cloudinaryService.AddPhoto(file);
                    var productImage = new ProductImage(product.Id, imageUrl);
                    await _productImageRepository.AddAsync(productImage);
                }
            }

            Console.WriteLine($"Entity after update: {product}");

            var newProduct = await _repository.UpdateAsync(product);
            return _mapper.Map<ProductReadDto>(newProduct);
        }
    }
}