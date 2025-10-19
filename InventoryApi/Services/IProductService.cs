using InventoryApi.DTOs;

namespace InventoryApi.Services;

/// <summary>
/// Service interface for managing products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieves a product by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The <see cref="ProductDto"/> corresponding to the specified id.</returns>
    Task<ProductDto> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="createProductDto">The DTO containing product creation data.</param>
    /// <returns>The created <see cref="ProductDto"/>.</returns>
    Task<ProductDto> CreateAsync(CreateProductDto createProductDto);

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id">The identifier of the product to update.</param>
    /// <param name="updateProductDto">The DTO containing updated product data.</param>
    /// <returns>The updated <see cref="ProductDto"/>.</returns>
    Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto);

    /// <summary>
    /// Deletes a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves all products that are not deleted.
    /// </summary>
    /// <returns>A collection of <see cref="ProductDto"/> representing all products.</returns>
    Task<IEnumerable<ProductDto>> GetAllAsync();
}
