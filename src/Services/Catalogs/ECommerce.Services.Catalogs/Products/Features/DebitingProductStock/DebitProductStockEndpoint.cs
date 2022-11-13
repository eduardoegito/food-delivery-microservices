using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Commands;
using ECommerce.Services.Catalogs.Shared;

namespace ECommerce.Services.Catalogs.Products.Features.DebitingProductStock;

// POST api/v1/catalog/products/{productId}/debit-stock
public static class DebitProductStockEndpoint
{
    internal static IEndpointRouteBuilder MapDebitProductStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                $"{ProductsConfigs.ProductsPrefixUri}/{{productId}}/debit-stock",
                DebitProductStock)
            .WithTags(ProductsConfigs.Tag)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DebitProductStock")
            .WithDisplayName("Debit product stock")
            .WithApiVersionSet(SharedModulesConfiguration.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static async Task<IResult> DebitProductStock(
        long productId,
        int quantity,
        ICommandProcessor commandProcessor,
        CancellationToken cancellationToken)
    {
        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(DebitProductStockEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("ProductId", productId))
        {
            await commandProcessor.SendAsync(new DebitProductStock(productId, quantity), cancellationToken);

            return Results.NoContent();
        }
    }
}
