using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadExpressApi.Persistence.Common
{
    public class FileStorageService : IFileStorageService
    {

        /* public async Task<ResponseMessageObject> UploadFile(UploadFilesRequest request, string AppID)
         {

             Guid identity = Guid.NewGuid();
             var useridentity = AppID.ToString() + "/" + identity;

             var response = _blobClient.UploadFile(AppID, "NEWACCOUNTOPENING/FILE/" + AppID, useridentity + request.DocumentName + "" + "." + request.Filetype, request.PostedFilebase64);

             //Log.Information("The response from file upload service is " + response);
             var ans = response.Split('|');
             if (ans[1].ToString() == "Success")
             {
                 return await Task.FromResult(new ResponseMessageObject { id = ans[2].ToString() });

             }
             return await Task.FromResult(new ResponseMessageObject { });
         }*/
    }
}
