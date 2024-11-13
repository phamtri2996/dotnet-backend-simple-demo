using MediatR;
using Repository;

namespace Query;
public class GetOnboardingFileQuery : IRequest<IResult>
{
    public required string Id {get;set;}
}
public class GetOnboardingFileHandler(IUserRepository repository) : IRequestHandler<GetOnboardingFileQuery, IResult>
{
    public async Task<IResult> Handle(GetOnboardingFileQuery request, CancellationToken cancellationToken)
    {
        var res = await repository.GetOnboardingFile(request.Id);
        string contentType = res.FileType switch
        {
            "pdf" => "application/pdf",
            "zip" => "application/zip",
            _ => ""
        };
        return Results.File(res.File, contentType, $"{res.Name}.{res.FileType}");
    }
}