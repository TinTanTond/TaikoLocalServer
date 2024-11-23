using Application.Handlers.Api.Auth;
using Riok.Mapperly.Abstractions;
using Shared.Models.Requests;

namespace Server.Mappers;

[Mapper]
public static partial class ChangePasswordCommandMapper
{
    public static partial ChangePasswordCommand ToCommand(ChangePasswordRequest request);
}