using MediatR;
using Repository;

namespace Query;
public class GetProfileQuery : IRequest<IResult>
{
}
public class GetProfileHandler(IUserRepository repository) : IRequestHandler<GetProfileQuery, IResult>
{
    public async Task<IResult> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var res = await repository.GetProfiles();
        return Results.Ok(res);
    }
}