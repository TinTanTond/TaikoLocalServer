using Application.Handlers.Api.Auth;
using Shared.Models.Requests;

namespace Server.Mappers;

[Mapper]
public static partial class ChangePasswordCommandMapper
{
    public static partial ChangePasswordCommand ToCommand(ChangePasswordRequest request);
}