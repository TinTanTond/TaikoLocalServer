using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public static partial class SongPlayDataMapper
{
    [MapperIgnoreSource(nameof(entity.Id))]
    [MapperIgnoreSource(nameof(entity.Baid))]
    [MapperIgnoreSource(nameof(entity.Ba))]
    public static partial SongPlayDatumDto MapToDto(SongPlayDatum entity);
}