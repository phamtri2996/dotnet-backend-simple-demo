using FluentValidation;
using MediatR;
using Repository;

namespace Command;
public class UploadProfile : IRequest<IResult>
{
    public required string Name {get;set;}
    public required string PhoneNumber {get;set;}
    public required IFormFile File {get; set;}
}
public class UploadProfileValidator: AbstractValidator<UploadProfile>
{
    public UploadProfileValidator()
    {
        RuleFor(x => x.File).Must(ValidType).WithMessage("File must be valid type");
        RuleFor(x => x.File).Must(ValidSize).WithMessage("File must be valid size");
    }

    private bool ValidSize(IFormFile file)
    {
        long fileSize = file.Length;
        return fileSize < 20 * 1024 * 1024;
    }

    private bool ValidType(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        return fileExtension == ".pdf" || fileExtension == ".zip";
    }
}
public class UploadProfileHandler(IUserRepository repository) : IRequestHandler<UploadProfile, IResult>
{

    public async Task<IResult> Handle(UploadProfile request, CancellationToken cancellationToken)
    {
        var fileExtensionNoDot = Path.GetExtension(request.File.FileName).ToLower().Remove(0,1);
        using var ms = new MemoryStream();
        await request.File.OpenReadStream().CopyToAsync(ms, cancellationToken);
        var byteArr = ms.ToArray();
        await repository.UploadProfile(request.Name, request.PhoneNumber, byteArr, fileExtensionNoDot);
        return Results.Accepted();
    }
}
