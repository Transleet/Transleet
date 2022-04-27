using Orleans;
using Orleans.Runtime;

namespace Transleet.Grains
{

    public interface IOrganizationGrain : IGrainWithGuidKey
    {

    }
    public class OrganizationGrain : Grain, IOrganizationGrain
    {
        private readonly OrganizationGrainProfileState _profile;



        public class OrganizationGrainProfileState
        {
            public string Name { get; set; }
        }
    }


}
