using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectorArchicad.Communication.Commands;
using Speckle.Core.Kits;
using Speckle.Newtonsoft.Json;

namespace Archicad.Communication.Commands;

internal sealed class GetObjectData : GetDataBase, ICommand<IEnumerable<ArchicadObject>>
{
  [JsonObject(MemberSerialization.OptIn)]
  private sealed class Result
  {
    [JsonProperty("objects")]
    public IEnumerable<ArchicadObject> Datas { get; private set; }
  }

  public GetObjectData(IEnumerable<string> applicationIds, bool sendProperties, bool sendListingParameters)
    : base(applicationIds, sendProperties, sendListingParameters) { }

  public async Task<IEnumerable<ArchicadObject>> Execute()
  {
    Result result = await HttpCommandExecutor.Execute<Parameters, Result>(
      "GetObjectData",
      new Parameters(ApplicationIds, SendProperties, SendListingParameters)
    );
    foreach (var @object in result.Datas)
    {
      @object.units = Units.Meters;
    }

    return result.Datas;
  }
}
