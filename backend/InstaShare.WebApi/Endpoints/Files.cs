using MediatR;
using InstaShare.Application.Files.Commands;
using InstaShare.Application.Files.Queries;
using InstaShare.WebApi.Dtos;

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
                createFileDto.status,
                createFileDto.size);
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
                updateFileDto.status,
                updateFileDto.size,
                updateFileDto.blobUrl);
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

        app.MapPost("/files/upload", async (IFormFile file, IMediator mediator) =>
        {
            using var stream = file.OpenReadStream();
            var command = new UploadFileCommand(
                stream,
                file.FileName,
                file.ContentType,
                file.Length
            );
            
            var uploadedFile = await mediator.Send(command);
            return Results.Ok(new GetFileDto(uploadedFile));
        })
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<GetFileDto>();
    }
}