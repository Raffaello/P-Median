using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
//using System.Web.Services;

namespace PMedLibWcfService
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract (SessionMode=SessionMode.NotAllowed)]
    //[WebService(Name="PMedLibWcfWebService",Description="")]
    public interface IPMedLibWCFService
    {
        // TODO: Add your service operations here
        [OperationContract]
        String[] GetListOfCapFile();

        [OperationContract]
        bool GetSolution(int fileindex, int m, int n, out int[] x, out int[] y, out uint WrapSol, out List<uint>[] WrapSolClu);
    }

}
