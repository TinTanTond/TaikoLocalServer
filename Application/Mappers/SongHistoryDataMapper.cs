using System.Diagnostics.CodeAnalysis;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
[SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member")]
[SuppressMessage("Mapper", "RMG012:Source member was not found for target member")]
public static partial class SongHistoryDataMapper
{
    public static partial SongHistoryData ToSongHistoryData(SongPlayDatum datum);
}