using MediatR;
using InstaShare.Application.Files.Commands;
using InstaShare.Application.Files.Queries;

namespace InstaShare.WebApi.Endpoints;

public static class EndpointsInstaShareFilesExtension
{
    public static void RegisterEndpointsFiles(this IEndpointRouteBuilder app)
    {
        app.MapGet("/files", async (IMediator mediator) =>
        {
            var files = await mediator.Send(new GetAllFilesQuery());
            return Results.Ok(files.Select(f => new GetFileDto(f)).ToList());
        }).Produces<IList<GetFileDto>>();

        app.MapGet("/files/{id}", async (long id, IMediator mediator) =>
        {
            var file = await mediator.Send(new GetFileByIdQuery(id));
            return Results.Ok(new GetFileDto(file));
        }).Produces<GetFileDto>();

        app.MapPost("/files", async (CreateFileDto createFileDto, IMediator mediator) =>
        {
            var command = new CreateFileCommand(
                createFileDto.name,
                createFileDto.size,
                createFileDto.type);
            var file = await mediator.Send(command);
            return Results.Ok(new GetFileDto(file));
        }).DisableAntiforgery()
          .Accepts<CreateFileDto>("application/json")
          .Produces<GetFileDto>();

        app.MapPut("/files/{id}", async (long id, UpdateFileDto updateFileDto, IMediator mediator) =>
        {
            var command = new UpdateFileCommand(
                id,
                updateFileDto.name,
                updateFileDto.size,
                updateFileDto.type);
            var file = await mediator.Send(command);
            return Results.Ok(new GetFileDto(file));
        }).DisableAntiforgery()
          .Accepts<UpdateFileDto>("application/json")
          .Produces<GetFileDto>();

        app.MapDelete("/files/{id}", async (long id, IMediator mediator) =>
        {
            await mediator.Send(new RemoveFileCommand(id));
            return Results.NoContent();
        });
    }
}