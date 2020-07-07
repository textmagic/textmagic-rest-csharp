using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     List of Schedule resources
    /// </summary>
    public class SchedulesResult : BaseModelList
    {
        [DeserializeAs(Name = "resources")] public List<Schedule> Schedules { get; set; }
    }
}