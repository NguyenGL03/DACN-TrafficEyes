using Abp.Auditing;
using Abp.Extensions;
using Abp.MimeTypes;
using Common.gAMSPro.Intfs.Report;
using gAMSPro.Dto;
using gAMSPro.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace gAMSPro.Web.Controllers
{
    public class FileController : gAMSProControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IReportExporter _reportExporter;
        private readonly IMimeTypeMap _mimeTypeMap;


        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            IReportExporter reportExporter,
            IBinaryObjectManager binaryObjectManager,
            IMimeTypeMap mimeTypeMap
        )
        {
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _mimeTypeMap = mimeTypeMap;
            _reportExporter = reportExporter;
        }

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            //var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            var memoryStream = _reportExporter.GetFile(file);
            var fileBytes = memoryStream.ToArray();
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            if (fileName.IsNullOrEmpty())
            {
                if (!fileObject.Description.IsNullOrEmpty() &&
                    !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
                {
                    fileName = fileObject.Description;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            if (contentType.IsNullOrEmpty())
            {
                if (!Path.GetExtension(fileName).IsNullOrEmpty())
                {
                    contentType = _mimeTypeMap.GetMimeType(fileName);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            return File(fileObject.Bytes, contentType, fileName);
        }
    }
}
