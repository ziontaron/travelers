using Reusable;
using Reusable.Attachments;
using Reusable.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ReusableWebAPI.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AttachmentController : ApiController
    {
        [HttpPost, Route("api/attachment")]
        public object attachmentsPost()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var postedFile = HttpContext.Current.Request.Files["file"];
                    string attachmentKind = HttpContext.Current.Request["attachmentKind"];
                    if (attachmentKind == null || attachmentKind == "")
                    {
                        response.ErrorThrown = true;
                        response.ResponseDescription = "Attachment Kind was not specified.";
                        return response;
                    }
                    if (postedFile != null)
                    {
                        string fileName = postedFile.FileName;

                        string baseAttachmentsPath = ConfigurationManager.AppSettings[attachmentKind];

                        string currentPathAttachments;
                        string folderName = HttpContext.Current.Request["targetFolder"];
                        if (folderName != null && folderName.Trim() != "")
                        {
                            currentPathAttachments = baseAttachmentsPath + folderName + @"\";
                            if (!Directory.Exists(currentPathAttachments))
                            {
                                Directory.CreateDirectory(currentPathAttachments);
                            }
                        }
                        else
                        {
                            do
                            {
                                DateTime date = DateTime.Now;
                                folderName = date.ToString("yy") + date.Month.ToString("d2") +
                                                date.Day.ToString("d2") + "_" + MD5HashGenerator.GenerateKey(date);
                                currentPathAttachments = baseAttachmentsPath + folderName;
                            } while (Directory.Exists(currentPathAttachments));
                            Directory.CreateDirectory(currentPathAttachments);
                            currentPathAttachments += @"\";
                        }

                        if (postedFile.ContentLength > 0)
                        {
                            postedFile.SaveAs(currentPathAttachments + Path.GetFileName(postedFile.FileName));
                        }

                        List<Attachment> attachmentsResult = AttachmentsIO.getAttachmentsFromFolder(folderName, attachmentKind);

                        response.ErrorThrown = false;
                        response.ResponseDescription = folderName;
                        response.Result = attachmentsResult;
                    }
                }
            }
            catch (Exception e)
            {
                response.ErrorThrown = true;
                response.ResponseDescription = "ERROR: " + e.Message;
                response.Result = e;
            }

            return response;
        }

        [HttpGet, Route("api/attachment_download")]
        public HttpResponseMessage downloadAttachment()
        {
            HttpResponseMessage result = null;
            try
            {
                string strDirectory = HttpContext.Current.Request["directory"];
                string strFileName = HttpContext.Current.Request["fileName"];
                string appSettingsFolder = HttpContext.Current.Request["attachmentKind"];
                string baseAttachmentsPath = ConfigurationManager.AppSettings[appSettingsFolder];
                string filePath = baseAttachmentsPath + strDirectory + "\\" + strFileName;
                FileInfo file = new FileInfo(filePath);

                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = file.Name;
            }
            catch (Exception e)
            {
                result = null;
            }

            return result;
        }

        [HttpGet, Route("api/attachment_delete")]
        public HttpResponseMessage deleteAttachment()
        {
            HttpResponseMessage result = null;
            try
            {
                string strDirectory = HttpContext.Current.Request["directory"];
                string strFileName = HttpContext.Current.Request["fileName"];
                string appSettingsFolder = HttpContext.Current.Request["attachmentKind"];
                string baseAttachmentsPath = ConfigurationManager.AppSettings[appSettingsFolder];
                string filePath = baseAttachmentsPath + strDirectory + "\\" + strFileName;
                FileInfo file = new FileInfo(filePath);
                file.Delete();
                result = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                result = Request.CreateResponse(e);
            }

            return result;
        }
    }
}
